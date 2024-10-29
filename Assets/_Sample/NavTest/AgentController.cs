using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    // 마우스로 그라운드를 클릭하면 클릭한 지점으로 Agent를 이동시킨다
    public class AgentController : MonoBehaviour
    {
        #region Variables
        private NavMeshAgent agent;

        [SerializeField] private Vector3 worldPosition; //이동 목표지점
        #endregion

        private void Start()
        {
            //참조
            agent = GetComponent<NavMeshAgent>();            
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                SetDestinationToMousePosition();
            }
        }

        private void SetDestinationToMousePosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

    }
}
