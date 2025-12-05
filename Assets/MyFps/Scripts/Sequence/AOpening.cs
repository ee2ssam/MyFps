using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

namespace MyFps
{
    /// <summary>
    /// 플레이01씬의 오프닝 연출 
    /// </summary>
    public class AOpening : MonoBehaviour
    {
        #region Variables
        //페이더 효과
        public SceneFader fader;

        //플레이어 오브젝트
        public GameObject thePlayer;

        //시퀀스 텍스트
        public TextMeshProUGUI sequenceText;

        //시나리오 텍스트
        [SerializeField]
        private string sequence01 = "... Where I am?";

        [SerializeField]
        private string sequence02 = "I need get out of here";

        //사운드
        public AudioSource line01;  //sequence01
        public AudioSource line02;  //sequence02
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //시작하자마자 오프닝 연출
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        //오프닝 시퀀스 연출
        IEnumerator SequencePlay()
        {
            //0.플레이 캐릭터 비 활성화
            thePlayer.SetActive(false);

            //1.페이드인 연출(1초 대기후 페인드인 효과) - 2초
            fader.FadeStart(2f + 3f);

            //2.화면 하단에 시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence01;
            //목소리 출력
            line01.Play();

            //3. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(3f);

            //4.화면 하단에 시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence02;
            //목소리 출력
            line02.Play();

            //5. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(3f);
            sequenceText.text = "";

            //4.플레이 캐릭터 활성화
            thePlayer.SetActive(true);
        }
        #endregion
    }
}