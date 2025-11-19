using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// 플레이씬의 오프닝 연출 
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
        private string sequence01 = "I need get out of here";
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
            fader.FadeStart(2f);

            //2.화면 하단에 시나리오 텍스트 화면 출력(3초)
            sequenceText.text = sequence01;

            //3. 3초후에 시나리오 텍스트 없어진다
            yield return new WaitForSeconds(3f);
            sequenceText.text = "";

            //4.플레이 캐릭터 활성화
            thePlayer.SetActive(true);
        }
        #endregion
    }
}