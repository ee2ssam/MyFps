using UnityEngine;

namespace MyFps
{
    //메인 메뉴 씬을 관리하는 클래스
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioManager audioManager;

        //씬 변경
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainScene01";

        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            audioManager = AudioManager.Instance;

            //씬 시작시 페이드인 효과
            fader.FadeStart();

            //메뉴 배경음 플레이
            audioManager.PlayBgm("MenuMusic");
        }
        #endregion

        #region Custom Method
        public void NewGame()
        {
            //메뉴 선택 사운드
            audioManager.Play("MenuSelect");

            //새게임 하러 가기
            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("Load Game!!!");
        }

        public void Options()
        {
            Debug.Log("Show Options!!!");
        }

        public void Credits()
        {
            Debug.Log("Show Cridist!!!");
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game!!!");
            Application.Quit();
        }
        #endregion
    }
}
