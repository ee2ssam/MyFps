using UnityEngine;

namespace MyFps
{
    public class PauseUI : MonoBehaviour
    {
        #region Variables
        public GameObject pausedUI;

        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";

        //참조
        private GameObject thePlayer;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            thePlayer = FindFirstObjectByType<PlayerMove>().gameObject;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape) && thePlayer.activeSelf == true)
            {
                Toggle();
            }
        }
        #endregion

        #region Custom Method
        private void Toggle()
        {
            pausedUI.SetActive(!pausedUI.activeSelf);

            if (pausedUI.activeSelf)
            {
                Time.timeScale = 0.0f;
                //플레이어 인풋기능 제거
                thePlayer.GetComponent<CharacterInput>().enabled = false;

                //마우스 커서 초기화(UI 화면)
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                thePlayer.GetComponent<CharacterInput>().enabled = true;

                //마우스 커서 초기화(플레이 화면)
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void Continue()
        {
            Toggle();
        }

        public void MainMenu()
        {
            Time.timeScale = 1.0f;
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}