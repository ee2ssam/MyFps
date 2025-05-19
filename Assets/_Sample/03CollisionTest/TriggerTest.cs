using UnityEngine;

namespace MySample
{
    public class TriggerTest : MonoBehaviour
    {
        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter : {other.tag}");
            //플레이어를 오른쪽으로 3만큼 힘을 준다, 빨간색으로 바꾼다
            MoveObejct player = other.GetComponent<MoveObejct>();
            if(player)
            {
                player.MoveRight();
                player.ChangeColorRed();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log($"OnTriggerStay : {other.tag}");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"OnTriggerExit : {other.tag}");
            //플레이어를 오른쪽으로 3만큼 힘을 준다, 오리지널 컬러로 바꾼다
            MoveObejct player = other.GetComponent<MoveObejct>();
            if (player)
            {
                player.MoveRight();
                player.ChangeColorOrigin();
            }
        }
        #endregion
    }
}
