using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyFps
{
    /// <summary>
    /// 플레이02씬의 오프닝 연출 
    /// 페이드인 효과, 배경음 플레이, 시퀀스 텍스트 초기화
    /// </summary>
    public class EOpening : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;

        //플레이어 오브젝트
        public GameObject thePlayer;

        //시퀀스 텍스트
        public TextMeshProUGUI sequenceText;
        #endregion


        #region Unity Event Method
        private void Start()
        {            
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //0. 플레이 캐릭터 비 활성화
            thePlayer.SetActive(false);

            //1. 페이드인 연출(1초)
            fader.FadeStart();

            //2. 시나리오 텍스트 없어진다
            sequenceText.text = "";

            //3. 배경음 플레이
            AudioManager.Instance.PlayBGM("Bgm01");

            yield return new WaitForSeconds(1f);

            //4. 플레이 캐릭터 활성화
            thePlayer.SetActive(true);
        }
        #endregion

    }
}