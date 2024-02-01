using UnityEngine;

public class MouseToPlayerLine : MonoBehaviour
{
    public Transform playerTransform; // 玩家的Transform引用
    private LineRenderer lineRenderer; // LineRenderer组件的引用

    void Start()
    {
        // 尝试获取LineRenderer组件，如果不存在则添加一个
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // 配置LineRenderer
        lineRenderer.startWidth = 0.05f; // 设置线的起始宽度
        lineRenderer.endWidth = 0.05f; // 设置线的结束宽度
        // 可以添加更多的LineRenderer设置，如材质、颜色等
    }

    void Update()
    {
        // 计算鼠标在世界空间中的位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 假设地面或者跟随平面的y值为playerTransform.position.y
        float planeY = playerTransform.position.y;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            // 如果射线与场景中的对象相交，则使用击中点的位置
            mousePosition = hit.point;
        }
        else
        {
            // 如果没有击中任何对象，则将鼠标位置设置为玩家的y值，保持在同一水平面上
            // 根据射线与玩家平面的交点计算位置
            float enter = 0.0f;
            Plane plane = new Plane(Vector3.up, new Vector3(0, planeY, 0));
            if (plane.Raycast(ray, out enter))
            {
                mousePosition = ray.GetPoint(enter);
            }
        }

        // 锁定Y轴，让X和Z轴跟随鼠标
        Vector3 startPosition = new Vector3(mousePosition.x, planeY, mousePosition.z);

        // 设置LineRenderer的两个点
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, new Vector3(playerTransform.position.x, planeY, playerTransform.position.z));
    }
}
