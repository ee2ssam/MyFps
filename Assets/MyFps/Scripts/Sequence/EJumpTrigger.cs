using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyFps
{
    public class EJumpTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject thePlayer;

        public GameObject activityObject;   //연출용 오브젝트
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(PlaySequence());
        }

        IEnumerator PlaySequence()
        {
            //플레이 캐릭터 비활성화(플레이 멈춤)
            thePlayer.GetComponent<FirstPersonController>().enabled = false;
            activityObject.SetActive(true);         //연출용 오브젝트 활성화

            yield return new WaitForSeconds(2f);

            //플레이 캐릭터 활성화(다시 플레이)
            thePlayer.GetComponent<FirstPersonController>().enabled = true;

            //연출용 오브젝트 킬
            Destroy(activityObject);

            //트리거 충돌체 비활성화 - 킬
            Destroy(gameObject);
        }
    }
}