using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    public class TorchLight : MonoBehaviour
    {
        #region Variables
        public Transform torchLight;
        private Animator animator;

        private int lightMode = 0;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            animator = torchLight.GetComponent<Animator>();
            lightMode = 0;

            InvokeRepeating("LightAnimation", 0f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            //1초마다 1번씩 랜덤한 라이트 애니메이션을  플레이
            /*if (lightMode == 0)
            {
                StartCoroutine(FlameAnimation());
            }*/
        }

        IEnumerator FlameAnimation()
        {
            lightMode = Random.Range(1, 4);
            animator.SetInteger("LightMode", lightMode);

            yield return new WaitForSeconds(1f);
            lightMode = 0;
        }

        //반복 함수
        private void LightAnimation()
        {
            lightMode = Random.Range(1, 4);
            animator.SetInteger("LightMode", lightMode);
        }
    }
}