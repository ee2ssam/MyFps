using UnityEngine;

namespace MySample
{
    public class TriggerTest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            //오른쪽으로 힘을 준다, 컬러를 빨간색으로 바꾼다
            MoveObject moveObject = other.GetComponent<MoveObject>();
            if(moveObject != null)
            {
                moveObject.MoveRightByForce();
                moveObject.ChangeRedColor();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log($"OnTriggerStay: {other.name}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerExit: {other.name}");
            //오른쪽으로 힘을 준다, 컬러를 오리지널 색으로 바꾼다
            MoveObject moveObject = other.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.MoveRightByForce();
                moveObject.ResetColor();
            }
        }
    }
}