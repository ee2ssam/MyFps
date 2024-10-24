using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        #region Variables
        public GameObject fakeObject;       //온전한 오브젝트
        public GameObject breakObject;      //깨진 오브젝트
        public GameObject effectObject;     //깨지는 움직임 효과 오브젝트

        public GameObject hiddenItem;       //숨겨진 아이템

        private bool isBreak = false;
        [SerializeField] private bool unBreakable = false;   //true:깨지지 않는다
        #endregion

        //총을 맞으면
        public void TakeDamage(float damage)
        {
            //깨짐 여부 체크
            if (unBreakable)
                return;

            //1 shot 1 kill
            if (!isBreak)
            {
                StartCoroutine(BreakObject());
            }
        }


        //페이크 -> 브레이크 -> 히든 아이템 등장, 깨지는 사운드 재생
        IEnumerator BreakObject()
        {
            isBreak = true;
            this.GetComponent<BoxCollider>().enabled = false;

            fakeObject.SetActive(false);
            yield return new WaitForSeconds(0.05f);

            AudioManager.Instance.Play("PotterySmash");
            breakObject.SetActive(true);

            if (effectObject != null)
            {
                effectObject.SetActive(true);

                yield return new WaitForSeconds(0.05f);
                effectObject.SetActive(false);
            }

            //숨겨진 아이템 있으면 아이템 보여주기
            if (hiddenItem != null)
            {
                hiddenItem.SetActive(true);
            }
        }
    }
}
