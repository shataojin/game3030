using UnityEngine;

public class MouseToPlayerLine : MonoBehaviour
{
    public Transform playerTransform; // ��ҵ�Transform����
    private LineRenderer lineRenderer; // LineRenderer���������

    void Start()
    {
        // ���Ի�ȡLineRenderer�������������������һ��
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // ����LineRenderer
        lineRenderer.startWidth = 0.05f; // �����ߵ���ʼ���
        lineRenderer.endWidth = 0.05f; // �����ߵĽ������
        // ������Ӹ����LineRenderer���ã�����ʡ���ɫ��
    }

    void Update()
    {
        // �������������ռ��е�λ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���������߸���ƽ���yֵΪplayerTransform.position.y
        float planeY = playerTransform.position.y;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            // ��������볡���еĶ����ཻ����ʹ�û��е��λ��
            mousePosition = hit.point;
        }
        else
        {
            // ���û�л����κζ��������λ������Ϊ��ҵ�yֵ��������ͬһˮƽ����
            // �������������ƽ��Ľ������λ��
            float enter = 0.0f;
            Plane plane = new Plane(Vector3.up, new Vector3(0, planeY, 0));
            if (plane.Raycast(ray, out enter))
            {
                mousePosition = ray.GetPoint(enter);
            }
        }

        // ����Y�ᣬ��X��Z��������
        Vector3 startPosition = new Vector3(mousePosition.x, planeY, mousePosition.z);

        // ����LineRenderer��������
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, new Vector3(playerTransform.position.x, planeY, playerTransform.position.z));
    }
}
