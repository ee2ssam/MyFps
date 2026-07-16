using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 메인메뉴씬을 관리하는 클래스
    /// 메인메뉴(버튼 5개) 기능
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioManager audioManager;

        //씬이동
        public SceneFader fader;
        [SerializeField] private string loadToScene = "PlayScene01";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            audioManager = AudioManager.Instance;

            //배경음 플레이
            audioManager.PlayBgm("MenuBgm");

            //마우스 커서 초기화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        #endregion

        #region Custom Method
        public void NewGame()
        {
            //사운드 처리
            audioManager.Stop("MenuBgm");
            audioManager.Play("MenuButton");
;
            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("Load PlayScene!");
        }

        public void Options()
        {
            Debug.Log("Show Options");
        }

        public void Credits()
        {
            Debug.Log("Show Credits");
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game");
            Application.Quit();
        }
        #endregion

    }
}