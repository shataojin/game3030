using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform player; // ���λ��
    public float offsetForward = 1f; // �۹����������ǰ����ƫ����
    public float offsetUp = 2f; // �۹�����������Ϸ���ƫ����

    void Update()
    {
        // �����۹��λ�úͳ���
        transform.position = player.position + player.forward * offsetForward + Vector3.up * offsetUp;
        transform.forward = player.forward;
    }
}
