using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    public class AgentController : MonoBehaviour
    {
        #region Variables
        //참조
        private NavMeshAgent m_Agent;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            //마우스 좌 클릭한 지점으로 이동
            if (Input.GetMouseButton(0))
            {
                RayToWorld();
            }

        }
        #endregion

        #region Custom Method
        //마우스 포인터 위치에서 레이를 쏘아 히트한 지점으로 에이전트를 이동 시킨다
        private void RayToWorld()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //히트한 지점으로 에이전트를 이동 시킨다
                m_Agent.SetDestination(hit.point);
            }
        }
        #endregion
    }
}