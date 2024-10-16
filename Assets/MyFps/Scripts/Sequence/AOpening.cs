using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyFps
{
    public class AOpening : MonoBehaviour
    {
        #region Variables
        public GameObject thePlayer;
        public SceneFader fader;

        //sequence UI
        public TextMeshProUGUI textBox; 
        [SerializeField]
        private string sequence = "I need get out of here";
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //마우스 커서 상태 설정
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(PlaySequence());
        }

        //오프닝 시퀀스
        IEnumerator PlaySequence()
        {
            //0.플레이 캐릭터 비 활성화
            thePlayer.SetActive(false);

            //1.페이드인 연출(1초 대기후 페인드인 효과)            
            fader.FromFade(1f); //2초동안 페이드 효과

            //2.화면 하단에 시나리오 텍스트 화면 출력(3초)
            //(I need get out of here)
            textBox.gameObject.SetActive(true);
            textBox.text = sequence;

            //3. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(3f);
            textBox.text = "";
            textBox.gameObject.SetActive(false);

            //4.플레이 캐릭터 활성화
            thePlayer.SetActive(true);
        }

    }
}