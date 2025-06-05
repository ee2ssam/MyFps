using UnityEngine;

namespace MyFps
{
    //트리거에 들어오면 플레어가 안전지역에 없다 저장
    public class SafeZoneOutTrigger : MonoBehaviour
    {
        #region Variables
        //SafeZoneInTrigger 오브젝트
        public GameObject SafeZoneInTrigger;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerController.safeZoneIn = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                //SafeZoneInTrigger 활성화
                SafeZoneInTrigger.SetActive(true);

                //SafeZoneOutTrigger 비활성
                this.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}
