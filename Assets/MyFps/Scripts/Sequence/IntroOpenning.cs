using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 인트로 씬을 관리하는 클래스
    /// 인트로 연출, ...
    /// </summary>
    public class IntroOpenning : MonoBehaviour
    {
        #region Variables
        //참조
        public SceneFader fader;
        [SerializeField] private string loadToScene = "PlayScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //페이드 인
            fader.FadeStart();

        }

        private void Update()
        {
            //Esc 키를 누르면 강제로 플레이씬 가기

        }
        #endregion


        #region Custom Method
        //오픈닝 시퀀스

        //
        public void Exit()
        {
            StartCoroutine(ExitSequence());
        }

        IEnumerator ExitSequence()
        {
            yield return new WaitForSeconds(1f);
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}