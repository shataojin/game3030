using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Start is called before the first frame update
    public float detectionRadius = 5f; // ���˸�֪��ҵķ�Χ
    public Transform player; // ��ҵ�Transform
    public Color detectionColor = Color.red; // Gizmo����ɫ�����ڿ��ӻ���ⷶΧ

    void OnDrawGizmos()
    {
        // ʹ��Gizmos�ڱ༭ģʽ�»��Ƶ��˵ļ�ⷶΧ
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        // ������������֮��ľ���
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // �������Ƿ��ڼ�ⷶΧ��
        if (distanceToPlayer <= detectionRadius)
        {
            // ����ڼ�ⷶΧ�ڣ�ִ�����
            print("Shoot"); // ��������滻������ľ���ʵ�ִ���
        }
    }
}
