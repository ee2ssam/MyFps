using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 게임오버 UI 처리
    /// 플레이씬 다시하기, 메인메뉴 가기(Debug.Log)
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        #region Variables
        //씬 이동
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";
        [SerializeField]
        private string backToScene = "PlayScene";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //마우스 커서 초기화(UI 화면)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //페이드인
            fader.FadeStart();
        }
        #endregion

        #region Custom Method
        public void Retry()
        {
            fader.FadeTo(backToScene);
        }

        public void MainMenu()
        {
            Debug.Log("Goto MainMenu");
            //fader.FadeTo(loadToScene);
        }
        #endregion
    }
}