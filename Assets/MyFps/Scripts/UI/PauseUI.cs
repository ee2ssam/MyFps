using UnityEngine;
using StarterAssets;

namespace MyFps
{
    public class PauseUI : MonoBehaviour
    {
        #region Variables
        public GameObject pauseUI;

        public SceneFader fader;
        [SerializeField] private string loadToScene = "MainMenu";

        private GameObject thePlayer;
        #endregion

        private void Start()
        {
            //참조
            thePlayer = GameObject.Find("Player");
        }

        private void Update()
        {
            //
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            pauseUI.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf) //pause 창이 오픈 될때, 사운드? && !isSequence
            {
                thePlayer.GetComponent<FirstPersonController>().enabled = false;
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
            }
            else //pause 창이 클로즈 될때
            {
                thePlayer.GetComponent<FirstPersonController>().enabled = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Time.timeScale = 1f;
            }
        }

        public void Menu()
        {
            Time.timeScale = 1f;

            //씬 종료 처리 ...
            AudioManager.Instance.StopBgm();

            fader.FadeTo(loadToScene);
        }
    }
}