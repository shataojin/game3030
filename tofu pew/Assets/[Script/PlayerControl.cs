using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    public Transform firePoint; // ����㣬�ӵ�������������
    public GameObject bulletPrefab; // �ӵ���Ԥ����

    public float fireRate = 2f; // ���Ƶ�ʣ�ÿ��������ӵ���
    private float nextFireTime = 0f; // ��һ�������ʱ��
    public float bulletSpeed = 20.0f; // �ӵ��ٶ�

    private InputActions playerInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new InputActions();

        // �ƶ�
        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMoveCanceled;

        // ���
        playerInput.Player.Shoot.performed += OnShootPerformed;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Move.performed -= OnMovePerformed;
        playerInput.Player.Move.canceled -= OnMoveCanceled;
        playerInput.Player.Shoot.performed -= OnShootPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        FireBullet();
    }

    private void FixedUpdate()
    {
        // ��Vector2������ת��ΪVector3
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Ӧ���ƶ�
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void FireBullet()
    {
        if (Time.time < nextFireTime) return; // ����Ƿ񵽴����Ƶ������

        if (bulletPrefab != null && firePoint != null)
        {
            // ʵ�����ӵ��ڷ����λ�ú���ת
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // Use Quaternion.identity for initial rotation

            // �������������ռ��е�λ��
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Vector3 targetPoint = firePoint.position + firePoint.forward * 100; // Ĭ�Ϸ����������δ�����κ�����
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }

            // ����ӷ���㵽Ŀ���ķ���
            Vector3 direction = (targetPoint - firePoint.position).normalized;

            // ��ȡ�ӵ���Rigidbody���
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // ���ӵ�ʩ����ʹ�䳯�������ķ����ƶ�
            bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse);

            // ������һ�����ʱ��
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Update()
    {
        RotatePlayerToMouse();
    }

    private void RotatePlayerToMouse()
    {
        // ��ȡ���λ�õ�����
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // ������Ϸƽ����Y=0��ƽ��
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // ������ҵ������λ�õķ�������
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0; // ȷ������Y������ת

            // �����µ���ת����
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

            // ��ת���
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }


}
