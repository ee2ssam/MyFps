using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 메인메뉴 씬을 관리하는 클래스
    /// 메인메뉴 버튼기능, 신페이더 기능
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "PlayScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //페이드인 시작
            fader.FadeStart();

            //배경음 플레이
            AudioManager.Instance.PlayBGM("MenuMusic");

            //초기화

        }
        #endregion

        #region Custom Method
        //버튼 대응 함수 구현, Debug.Log("  버튼 클릭")
        public void NewGame()
        {
            //버튼 효과음
            AudioManager.Instance.Play("ButtonHit");

            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("Goto Save Scene");
        }

        public void Options()
        {
            Debug.Log("Goto Option Menu");
        }

        public void Credits()
        {
            Debug.Log("Goto Credits Menu");
        }

        public void QuitGame()
        {
            //게임종료
            Application.Quit();

            Debug.Log("QuitGame");
        }
        #endregion
    }
}