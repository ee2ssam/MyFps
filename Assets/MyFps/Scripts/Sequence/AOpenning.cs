using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

namespace MyFps
{
    //플레이 씬 오프닝 연출
    public class AOpenning : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        //페이더 객체
        public SceneFader fader;

        //시나리오 대사 처리
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence01 = "...Where am I?";

        [SerializeField]
        private string sequence02 = "I need get out of here";

        //배경음
        public AudioSource bgm01;

        //대사 음성
        public AudioSource line01;
        public AudioSource line02;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //오프닝 연출 시작
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        //오프닝 연출 코루틴 함수
        IEnumerator SequencePlay()
        {
            //0.플레이 캐릭터 비 활성화
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //1. 페이드인 연출 (1초 대기후 페인드인 효과)
            fader.FadeStart(4f);

            //2.화면 하단에 시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence01;
            line01.Play();
            yield return new WaitForSeconds(3f);

            sequenceText.text = sequence02;
            line02.Play();
            yield return new WaitForSeconds(3f);
            //3. 3초후에 시나리오 텍스트 없어진다
            sequenceText.text = "";

            //배경음 플레이 시작
            bgm01.Play();

            //4.플레이 캐릭터 활성화
            //thePlayer.SetActive(true);
            input.enabled = true;
        }
        #endregion
    }
}