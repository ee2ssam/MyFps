using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 충돌체의 Trigger 충돌 체크
    /// </summary>
    public class TriggerTest : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.name}");
            //오른쪽으로 힘(200f), 컬러를 빨간색
            MoveObejct moveObejct = other.GetComponent<MoveObejct>();
            if (moveObejct)
            {
                moveObejct.MoveRigth();
                moveObejct.ChangeMoveColor();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log($"OnTriggerStay: {other.name}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerExit: {other.name}");
            //오른쪽으로 힘(200f), 컬러를 오리진
            MoveObejct moveObejct = other.GetComponent<MoveObejct>();
            if (moveObejct)
            {
                moveObejct.MoveRigth();
                moveObejct.ResetMoveColor();
            }
        }
    }
}