using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 충돌체의 collision 충돌 체크
    /// </summary>
    public class CollisionTest : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"OnCollisionEnter: {collision.gameObject.name}");
        }

        private void OnCollisionStay(Collision collision)
        {
            Debug.Log($"OnCollisionStay: {collision.gameObject.name}");
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log($"OnCollisionExit: {collision.gameObject.name}");
        }
    }
}