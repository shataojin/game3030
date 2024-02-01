using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Target
{
    public Transform transform;
}

[ExecuteInEditMode]
public class Follow : MonoBehaviour
{
    public Target target;
    public float sensitivityX = 8F;

    void Update()
    {
        // 跟随目标，保持摄像头的当前高度
        Vector3 newPosition = target.transform.position;
        newPosition.y = transform.position.y; // 保持摄像头的高度不变
        transform.position = newPosition;
    }
}
