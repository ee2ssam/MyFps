using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    public class AgentController : MonoBehaviour
    {
        #region Variables
        //참조
        private NavMeshAgent agent;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            agent = this.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            //마우스 좌 클릭한 지점으로 Agent 이동
            if(Input.GetMouseButtonDown(0))
            {
                //클릭한 지점 구하기
                Vector3 worldPositon = RayToWorld();
                //Agent의 이동 목표지점 설정
                agent.SetDestination(worldPositon);
            }
        }
        #endregion

        #region Custom Method
        //월드 포지션값 얻어오기 - 마우스의 위치에 쏘는 Ray를 이용
        private Vector3 RayToWorld()
        {
            Vector3 worldPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                worldPos = hit.point;
            }

            return worldPos;
        }
        #endregion
    }
}
