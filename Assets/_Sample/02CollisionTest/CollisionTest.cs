using Unity.VisualScripting;
using UnityEngine;

namespace MySample
{
    public class CollisionTest : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter: {collision.gameObject.name}");
            //왼쪽으로 힘을 준다
            MoveObject moveObject = collision.gameObject.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.MoveLeftByForce();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            Debug.Log($"OnCollisionStay: {collision.gameObject.name}");
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log($"OnCollisionExit: {collision.gameObject.name}");
            //왼쪽으로 힘을 준다
            MoveObject moveObject = collision.gameObject.GetComponent<MoveObject>();
            if (moveObject != null)
            {
                moveObject.MoveLeftByForce();
            }
        }
    }
}