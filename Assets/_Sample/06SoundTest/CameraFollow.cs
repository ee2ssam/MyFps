using UnityEngine;

namespace MySample
{
    //플레이어를 offset 위치에서 쫓아간다
    public class CameraFollow : MonoBehaviour
    {
        #region Variables
        public Transform thePlayer;
        public Vector3 offset;
        #endregion

        #region Unity Event Method
        private void LateUpdate()
        {
            transform.position = thePlayer.position + offset;
        }
        #endregion
    }
}
