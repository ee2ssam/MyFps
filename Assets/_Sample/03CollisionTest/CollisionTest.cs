using Unity.VisualScripting;
using UnityEngine;

namespace MySample
{
    public class CollisionTest : MonoBehaviour
    {
        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter: {collision.transform.tag}");
            //플레이어를 왼쪽으로 3만큼 힘을 준다
            MoveObejct player = collision.transform.GetComponent<MoveObejct>();
            if(player)
            {
                player.MoveLeft();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            Debug.Log($"OnCollisionStay: {collision.transform.tag}");
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log($"OnCollisionExit: {collision.transform.tag}");
            //플레이어를 왼쪽으로 3만큼 힘을 준다
            MoveObejct player = collision.transform.GetComponent<MoveObejct>();
            if (player)
            {
                player.MoveLeft();
            }
        }
        #endregion
    }
}
