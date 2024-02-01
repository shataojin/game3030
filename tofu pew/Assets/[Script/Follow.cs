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
        // ����Ŀ�꣬��������ͷ�ĵ�ǰ�߶�
        Vector3 newPosition = target.transform.position;
        newPosition.y = transform.position.y; // ��������ͷ�ĸ߶Ȳ���
        transform.position = newPosition;
    }
}
