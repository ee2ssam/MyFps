using UnityEngine;

namespace MyFps
{
    public class AmmoUI : MonoBehaviour
    {
        #region Variables
        public GameObject ammoUI;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            ShowAmmoUI();
        }

        private void ShowAmmoUI()
        {
            ammoUI.SetActive(PlayerStats.Instance.HasGun);
        }
    }
}