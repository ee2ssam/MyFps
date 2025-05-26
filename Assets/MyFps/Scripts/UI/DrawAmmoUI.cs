using UnityEngine;
using TMPro;

namespace MyFps
{
    //Ammo 갯수를 UI와 연결
    public class DrawAmmoUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI ammoCountText;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Ammo 갯수를 UI와 연결
            ammoCountText.text = PlayerDataManager.Instance.AmmoCount.ToString();
        }
        #endregion
    }
}
