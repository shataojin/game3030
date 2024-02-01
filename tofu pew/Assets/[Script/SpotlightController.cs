using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform player; // 玩家位置
    public float offsetForward = 1f; // 聚光灯相对于玩家前方的偏移量
    public float offsetUp = 2f; // 聚光灯相对于玩家上方的偏移量

    void Update()
    {
        // 调整聚光灯位置和朝向
        transform.position = player.position + player.forward * offsetForward + Vector3.up * offsetUp;
        transform.forward = player.forward;
    }
}
