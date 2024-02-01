using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    public Transform firePoint; // 发射点，子弹将从这里生成
    public GameObject bulletPrefab; // 子弹的预制体

    public float fireRate = 2f; // 射击频率，每秒射击的子弹数
    private float nextFireTime = 0f; // 下一次射击的时间
    public float bulletSpeed = 20.0f; // 子弹速度

    private InputActions playerInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new InputActions();

        // 移动
        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMoveCanceled;

        // 射击
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
        // 将Vector2的输入转换为Vector3
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // 应用移动
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void FireBullet()
    {
        if (Time.time < nextFireTime) return; // 检查是否到达射击频率条件

        if (bulletPrefab != null && firePoint != null)
        {
            // 实例化子弹在发射点位置和旋转
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // Use Quaternion.identity for initial rotation

            // 计算鼠标在世界空间中的位置
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            Vector3 targetPoint = firePoint.position + firePoint.forward * 100; // 默认方向，如果射线未击中任何物体
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }

            // 计算从发射点到目标点的方向
            Vector3 direction = (targetPoint - firePoint.position).normalized;

            // 获取子弹的Rigidbody组件
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // 向子弹施加力使其朝向计算出的方向移动
            bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse);

            // 更新下一次射击时间
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Update()
    {
        RotatePlayerToMouse();
    }

    private void RotatePlayerToMouse()
    {
        // 获取鼠标位置的射线
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // 假设游戏平面是Y=0的平面
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 计算玩家到鼠标点击位置的方向向量
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0; // 确保不在Y轴上旋转

            // 计算新的旋转方向
            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

            // 旋转玩家
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }


}
