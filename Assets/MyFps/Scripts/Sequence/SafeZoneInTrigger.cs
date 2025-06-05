using UnityEngine;

namespace MyFps
{
    //트리거에 들어가면 플레어가 안전지역에 있다 저장
    public class SafeZoneInTrigger : MonoBehaviour
    {
        #region Variables
        //SafeZoneOutTrigger 오브젝트
        public GameObject SafeZoneOutTrigger;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerController.safeZoneIn = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                //SafeZoneOutTrigger 활성화
                SafeZoneOutTrigger.SetActive(true);

                //SafeZoninTrigger 비활성
                this.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}
