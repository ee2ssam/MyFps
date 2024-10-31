using UnityEngine;

namespace MyFps
{
    public class CreditUI : MonoBehaviour
    {
        #region Variables
        public GameObject mainMenu;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideCredits();
            }
        }

        private void HideCredits()
        {
            mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}