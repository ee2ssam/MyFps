using UnityEngine;

namespace MyFps
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField] private string loadToScene = "MainScene01";

        private AudioManager audioManager;
        #endregion

        private void Start()
        {
            //씬 페이드인 효과
            fader.FromFade();

            //참조
            audioManager = AudioManager.Instance;

            //Bgm 플레이
            audioManager.PlayBgm("MenuBgm");
        }

        public void NewGame()
        {
            audioManager.Stop(audioManager.BgmSound);            
            audioManager.Play("MenuButton");

            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("Goto LoadGame");
        }

        public void Options()
        {
            audioManager.PlayBgm("PlayBgm");

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
    }
}