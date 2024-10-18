using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyFps
{
    public class BFirstTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject thePlayer;
        public GameObject theArrow;

        //sequence UI
        public TextMeshProUGUI textBox;
        [SerializeField]
        private string sequence = "Looks like a weapon on that table";

        public AudioSource line03;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(PlaySequence());
        }

        //트리거 작동시 플레이
        IEnumerator PlaySequence()
        {
            //플레이 캐릭터 비활성화(플레이 멈춤)
            thePlayer.GetComponent<FirstPersonController>().enabled = false;

            //대사 출력: "Looks like a weapon on that table.", 음성 출력
            textBox.gameObject.SetActive(true);
            textBox.text = sequence;
            line03.Play();

            //1초 딜레이
            yield return new WaitForSeconds(2f);

            //화살표 활성화
            theArrow.SetActive(true);

            //1초 딜레이
            yield return new WaitForSeconds(1f);

            //초기화
            textBox.text = "";
            textBox.gameObject.SetActive(false);

            //플레이 캐릭터 활성화(다시 플레이)
            thePlayer.GetComponent<FirstPersonController>().enabled = true;

            //트리거 충돌체 비활성화 - 킬
            Destroy(gameObject);
            //transform.GetComponent<BoxCollider>().enabled = false;
        }
    }
}