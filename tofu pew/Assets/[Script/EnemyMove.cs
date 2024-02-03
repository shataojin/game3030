using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyMove : MonoBehaviour
{
    //[SerializeField]
    //private bool m_bDebugRangeCheck = false;
    //[SerializeField]
    //private bool m_bDebugSideCheck = false;
    //[SerializeField]
    //private bool m_bDebugVisionCheck = false;
    [SerializeField]
    private float m_fDistanceThreshold = 5.0f;
    [SerializeField]
    private float m_fLookAngleThreshold = 30.0f;
    [SerializeField]
    private GameObject m_playerTransform ;
    [SerializeField]
    private GameObject m_agentEyes;
    [SerializeField]
    private GameObject m_agentDetect;
    [SerializeField]
    private bool m_bDebugRotate = false;
    [SerializeField]
    private float m_fHorHeadingTurningSpeed = 2.0f;
    private Vector3 m_vDebugTargetDir = Vector3.zero;

    [SerializeField]
    private bool m_EnemyDectCheck = false;
    [SerializeField]
    private float m_EnemyDectPlayerRange = 0.8f;

    private NavMeshAgent agent; // ���˵�NavMesh Agent
    [SerializeField]
    private bool m_bDebugMoveCheck = false;
    [SerializeField]
    private bool m_bDebugFireCheck = false;
    [SerializeField]
    public Transform firePoint; // ����㣬�ӵ�������������
    [SerializeField]
    public GameObject bulletPrefab; // �ӵ���Ԥ����
    [SerializeField]
    public float fireRate = 2f; // ���Ƶ�ʣ�ÿ��������ӵ���
    private float nextFireTime = 0f; // ��һ�������ʱ��
    [SerializeField]
    public float bulletSpeed = 20.0f; // �ӵ��ٶ�


    
    void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>(); // ��ȡ���˵�NavMesh Agent���
    }

    void Update()
    {
       
        if (m_playerTransform != null)
        {
            // ���ȼ������Ƿ��ڵ��˵���Ұ��
            if (!IsPlayerWithinAgentVision())
            {
                // ��Ҳ�����Ұ�ڣ����ڼ������Ƿ�ӽ�
                IsPlayerCloseMe();
                if (m_EnemyDectCheck == true)
                {
                    RotateTowardsPlayer();
                }
            }
            if ( IsPlayerWithinRange()&& IsPlayerWithinAgentVision())
            {
                print("PlayerWithinAgentVision");
                StartCoroutine(RotateTowardsPlayerWithDelay(3f));
                IsPlayerLeftOrRight();
            
                if(m_bDebugMoveCheck == true)
                {
                    agent.SetDestination(m_playerTransform.transform.position);
                }
                if(m_bDebugFireCheck == true)
                {
                    FireBullet();
                    print("shoot!shoot!");
                }
            }

            //    // �����ļ���߼�
            //    if (m_bDebugRangeCheck && IsPlayerWithinRange())
            //{
            //    print("Player within range!");
            //    if (m_bDebugRotate)
            //    {
            //        RotateTowardsPlayer();
            //    }
            //    if (m_bDebugSideCheck)
            //    {
            //        IsPlayerLeftOrRight();
            //    }
            //    if (m_bDebugVisionCheck && IsPlayerWithinAgentVision())
            //    {
            //        print("Player is within my view!");
            //        print("shoot!shoot!");
            //    }
            //}
        }
    }





    private void IsPlayerCloseMe()
    {
        
        if (Mathf.Abs(m_playerTransform.transform.position.x - transform.position.x) < m_EnemyDectPlayerRange||
            Mathf.Abs(m_playerTransform.transform.position.z - transform.position.z) < m_EnemyDectPlayerRange)
        {
            m_EnemyDectCheck = true;
            print("enemy hear player close!");
        }
        else
        {
            m_EnemyDectCheck = false;
            print("enemy cant hear it anymore!");
        }
    }

    //check if player is within m_fDistanceThreshold
    private bool IsPlayerWithinRange()
    {
        //fDist = square magnitude to get rid of negatives
        float fDist = (transform.position - m_playerTransform.transform.position).sqrMagnitude;
        if (fDist < m_fDistanceThreshold * m_fDistanceThreshold)
        {
            if (m_agentDetect != null) m_agentDetect.SetActive(true);
            return true;
        }
        else
        {
            if (m_agentDetect != null) m_agentDetect.SetActive(false);
            return false;
        }

    }

    private bool IsPlayerWithinAgentVision()
    {
        //get the vector from player's position to agent's position
        Vector3 vTargetDir = m_playerTransform.transform.position - transform.position;
        //get the angle from vTargetDir to agent's forward vector
        float fAngleToPlayer = Vector3.Angle(vTargetDir, transform.forward);
        //is the angle greater than -lookangle but less than +lookangle?
        if (fAngleToPlayer >= -m_fLookAngleThreshold && fAngleToPlayer <= m_fLookAngleThreshold)
        {
            m_agentEyes.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            return true;
        }
        m_agentEyes.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        return false;
    }

    private bool IsPlayerLeftOrRight()
    {
        //gets local position of the player relative to the agent
        Vector3 vPlayerLocalPosNorm = transform.InverseTransformPoint(m_playerTransform.transform.position).normalized;
        //calculates dot product between Vector3.right and vPlayerLocalPosNorm
        float fDotResult = Vector3.Dot(Vector3.right, vPlayerLocalPosNorm);
        //the dot product will be negative if player is to left positive if player is to right
        if (fDotResult > 0)
        {
            print("Player is on my right");
        }

        if (fDotResult < 0)
        {
            print("Player is on my left");
        }

        return fDotResult > 0 || fDotResult < 0;
    }

    private void RotateTowardsPlayer()
    {
        if (m_playerTransform != null)
        {
            float fVal = GetLookAtValue();
            //convert to Deg
            fVal *= Mathf.Rad2Deg;
            // Debug.Log(fVal);
            //rotate towards the player along the y axis
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, fVal, 0), m_fHorHeadingTurningSpeed * Time.deltaTime);
        }
    }

    public void SetDebugRotate(bool value)
    {
        m_bDebugRotate = value;
    }


    private float GetLookAtValue()
    {
        Vector3 vTargetDir = m_playerTransform.transform.position - transform.position;
        vTargetDir.y = 0.0f;


        m_vDebugTargetDir = vTargetDir;


        // Manual Calculation of theta between player and agent
        // vA dot vB = [(ax*bx)+(ay*by)+(az*bz)]
        // theta = cos^-1((vA dot vB) / (mag(A) * mag(B)))

        float fDotResult = (vTargetDir.x * Vector3.forward.x) + (vTargetDir.y * Vector3.forward.y) + (vTargetDir.z * Vector3.forward.z);

        return Mathf.Acos((fDotResult) / (vTargetDir.magnitude * Vector3.forward.magnitude)) * Mathf.Sign(vTargetDir.x);
    }

    private void FireBullet()
    {
        if (Time.time < nextFireTime) return; // ����Ƿ񵽴����Ƶ������

        if (bulletPrefab != null && firePoint != null && m_vDebugTargetDir != Vector3.zero)
        {
            // ʵ�����ӵ��ڷ����λ�ú���ת
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(m_vDebugTargetDir)); // ʹ�� m_vDebugTargetDir Ϊ�ӵ�����

            // ��ȡ�ӵ���Rigidbody���
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // ���ӵ�ʩ����ʹ�䳯�� m_vDebugTargetDir ָ���ķ����ƶ�
            bulletRb.AddForce(m_vDebugTargetDir.normalized * bulletSpeed, ForceMode.Impulse);

            // ������һ�����ʱ��
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    IEnumerator RotateTowardsPlayerWithDelay(float delay)
    {
        // �ȴ�ָ�����ӳ�ʱ��
        yield return new WaitForSeconds(delay);

        // �ӳ�֮��ִ�еĲ���
        RotateTowardsPlayer();
    }

    private void OnDrawGizmos()
    {

        //Draw the vision cone via up down left right vectors of default 45 degrees, length 5 units
        Vector3 vLineLeft = transform.TransformPoint(Quaternion.Euler(0, -m_fLookAngleThreshold, 0) * Vector3.forward * m_fDistanceThreshold);
        Vector3 vLineRight = transform.TransformPoint(Quaternion.Euler(0, m_fLookAngleThreshold, 0) * Vector3.forward * m_fDistanceThreshold);
        Vector3 vLineUp = transform.TransformPoint(Quaternion.Euler(-m_fLookAngleThreshold, 0, 0) * Vector3.forward * m_fDistanceThreshold);
        Vector3 vLineDown = transform.TransformPoint(Quaternion.Euler(m_fLookAngleThreshold, 0, 0) * Vector3.forward * m_fDistanceThreshold);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(vLineLeft, transform.position);
        Gizmos.DrawLine(vLineRight, transform.position);
        Gizmos.DrawLine(vLineUp, transform.position);
        Gizmos.DrawLine(vLineDown, transform.position);

       

        //draw line towards player and sphere on player
        if (m_vDebugTargetDir != Vector3.zero)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + m_vDebugTargetDir);
            Gizmos.DrawWireSphere(transform.position + m_vDebugTargetDir, 1);
        }

    }
}
