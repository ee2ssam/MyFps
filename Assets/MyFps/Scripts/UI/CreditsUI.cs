using UnityEngine;

namespace MyFps
{
    public class CreditsUI : MonoBehaviour
    {
        #region Variables
        public GameObject mainMenuUI;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideCreditsUI();
            }
        }
        #endregion

        #region Custom Method
        private void HideCreditsUI()
        {
            mainMenuUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}