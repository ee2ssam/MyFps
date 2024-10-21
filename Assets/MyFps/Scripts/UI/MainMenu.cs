using UnityEngine;

namespace MyFps
{
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField] private string loadToScene = "MainScene01";
        #endregion

        private void Start()
        {
            //씬 페이드인 효과
            fader.FromFade();
        }

        public void NewGame()
        {
            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            Debug.Log("Goto LoadGame");
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
    }
}