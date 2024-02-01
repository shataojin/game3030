#define DEBUG_VISIONCONE

using Unity.VisualScripting;
using UnityEngine;

public class AgentPerception : MonoBehaviour
{
    [SerializeField]
    private bool m_bDebugRangeCheck = false;
    [SerializeField]
    private bool m_bDebugSideCheck = false;
    //[SerializeField]
    //private bool m_bDebugVisionCheck = false;
    [SerializeField]
    private float m_fDistanceThreshold = 5.0f;
    [SerializeField]
    private float m_fLookAngleThreshold = 45.0f;
    [SerializeField]
    private Transform m_playerTransform = null;
    [SerializeField]
    private GameObject m_agentEyes;
    [SerializeField]
    private GameObject m_agentDetect;
    [SerializeField]
    private AgentController m_agentController;

    [SerializeField]
    public float AttackRange=5.0f;

    private void Start()
    {
        m_agentEyes.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        if (m_agentDetect != null) m_agentDetect.SetActive(false);
    }

    private void Update()
    {
        // "Buttons" to start calling our functions
        if (m_bDebugRangeCheck && IsPlayerWithinRange())
        {
            print("Player within range!");
            if (m_agentController != null)
            {
                m_agentController.SetDebugRotate(true);
            }
            if (m_bDebugSideCheck)
            {
                IsPlayerLeftOrRight();
            }
            //if (m_bDebugVisionCheck && IsPlayerWithinAgentVision())
            //{
            //    print("Player is within my view!");
            //    print("attack!");
            //}

            // Calculate the absolute difference in X positions
            float distanceX = Mathf.Abs(transform.position.x - m_playerTransform.position.x);
            float distanceY = Mathf.Abs(transform.position.y - m_playerTransform.position.y);

            //print(distanceX);
            if (distanceX <= AttackRange || distanceY <= AttackRange)
            {
                print("attack!");
            }
           
        }


    }

    


    //check if player is within m_fDistanceThreshold
    private bool IsPlayerWithinRange()
    {
        //fDist = square magnitude to get rid of negatives
        float fDist = (transform.position - m_playerTransform.position).sqrMagnitude;
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



    //eye will be green when player is in the angles of the vision cone, regardless of distance
    private bool IsPlayerWithinAgentVision()
    {
        //get the vector from player's position to agent's position
        Vector3 vTargetDir = m_playerTransform.position - transform.position;
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
        Vector3 vPlayerLocalPosNorm = transform.InverseTransformPoint(m_playerTransform.position).normalized;
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

    private void OnDrawGizmos()
    {
#if DEBUG_VISIONCONE
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

        // 设置Gizmos颜色
        Gizmos.color = Color.green;

        // 绘制圆圈围绕玩家位置
        // 注意：Unity没有直接绘制圆圈的Gizmos方法，所以我们使用Gizmos.DrawWireSphere
        if (m_playerTransform != null)
        {
            Gizmos.DrawWireSphere(m_playerTransform.position, m_fDistanceThreshold);
        }
#endif
    }
}
