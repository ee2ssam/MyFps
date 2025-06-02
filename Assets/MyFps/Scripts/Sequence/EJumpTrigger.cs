using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyFps
{
    //날아가는 컵 연출 트리거
    public class EJumpTrigger : MonoBehaviour
    {
        #region Variables
        //플레이어 제어 관리
        public PlayerInput playerInput;
        //연출 오브젝트
        public GameObject activityObejct;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //플레이어 체크
            if (other.tag == "Player")
            {
                //트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlayer()
        {
            playerInput.enabled = false;    //인풋 제어
            activityObejct.SetActive(true); //연출 시작

            yield return new WaitForSeconds(0.2f);
            activityObejct.SetActive(false);

            yield return new WaitForSeconds(2f);

            playerInput.enabled = true;    //인풋 제어
        }
        #endregion


    }
}
