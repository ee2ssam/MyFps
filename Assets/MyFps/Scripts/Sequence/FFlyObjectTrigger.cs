using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 트리거에 걸리면 액티브 오브젝트를 이용하여 컵을 날린다
    /// </summary>
    public class FFlyObjectTrigger : MonoBehaviour
    {
        #region Variables
        //참조: 충돌체
        private BoxCollider collider;

        //시퀀스
        //플레이어 오브젝트
        public GameObject thePlayer;

        public GameObject activateObject;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //트리거 시퀀스 플레이
            StartCoroutine(SequencePlay());

            //충돌체 비활성화(또는 킬)
            collider.enabled = false;
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            thePlayer.SetActive(false);
            activateObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            activateObject.SetActive(false);
            thePlayer.SetActive(true);
        }
        #endregion
    }
}