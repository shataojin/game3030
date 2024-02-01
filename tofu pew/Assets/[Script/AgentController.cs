#define DEBUG_AGENT

using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    private bool m_bDebugRotate = false;
    [SerializeField]
    private Transform m_playerTransform = null;
    [SerializeField]
    private float m_fHorHeadingTurningSpeed = 2.0f;

    [SerializeField]
    private float m_fMoveSpeed = 5.0f; // 控制AGENT移动速度

    private Vector3 m_vDebugTargetDir = Vector3.zero;

    private void Update()
    { 
        // "Button" to start looking at agent
        if (m_bDebugRotate)
        {
            RotateTowardsPlayer();
            MoveToPlayer();
        }
        
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

    private void MoveToPlayer()
    {
        if (m_playerTransform != null)
        {
            // 平滑移动AGENT到玩家位置
            transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, m_fMoveSpeed * Time.deltaTime);
        }
    }
    public void SetDebugRotate(bool value)
    {
        m_bDebugRotate = value;
    }


    private float GetLookAtValue()
    {
        Vector3 vTargetDir = m_playerTransform.position - transform.position;
        vTargetDir.y = 0.0f;

#if DEBUG_AGENT
        m_vDebugTargetDir = vTargetDir;
#endif

        // Manual Calculation of theta between player and agent
        // vA dot vB = [(ax*bx)+(ay*by)+(az*bz)]
        // theta = cos^-1((vA dot vB) / (mag(A) * mag(B)))

        float fDotResult = (vTargetDir.x * Vector3.forward.x) + (vTargetDir.y * Vector3.forward.y) + (vTargetDir.z * Vector3.forward.z);

        return Mathf.Acos((fDotResult) / (vTargetDir.magnitude * Vector3.forward.magnitude)) * Mathf.Sign(vTargetDir.x);
    }

    private void OnDrawGizmos()
    {
#if DEBUG_AGENT
        //draw line towards player and sphere on player
        if (m_vDebugTargetDir != Vector3.zero)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + m_vDebugTargetDir);
            Gizmos.DrawWireSphere(transform.position + m_vDebugTargetDir, 1);
        }
#endif
    }
}
