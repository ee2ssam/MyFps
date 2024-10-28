using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    //메인2번씬 클리어
    public class FExitTrigger : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField] private string loadToScene = "MainMenu";
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            PlaySequence();
        }

        void PlaySequence()
        {
            //씬 클리어 처리
            //배경음 종료
            AudioManager.Instance.StopBgm();

            //씬 클리어 보상, 데이터 처리
            //...

            //메인 메뉴로 이동
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            fader.FadeTo(loadToScene);
        }
    }
}