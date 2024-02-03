using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Start is called before the first frame update
    public float detectionRadius = 5f; // 敌人感知玩家的范围
    public Transform player; // 玩家的Transform
    public Color detectionColor = Color.red; // Gizmo的颜色，用于可视化检测范围

    void OnDrawGizmos()
    {
        // 使用Gizmos在编辑模式下绘制敌人的检测范围
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        // 计算玩家与敌人之间的距离
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 检查玩家是否在检测范围内
        if (distanceToPlayer <= detectionRadius)
        {
            // 玩家在检测范围内，执行射击
            print("Shoot"); // 这里可以替换成射击的具体实现代码
        }
    }
}
