using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 플레이어 쫓아가기
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public Transform player;
        public Vector3 offset;

        private void LateUpdate()
        {
            this.transform.position = player.position + offset;
        }
    }
}