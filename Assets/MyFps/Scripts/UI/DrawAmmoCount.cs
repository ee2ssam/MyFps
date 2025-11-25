using UnityEngine;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// UI - AmmoCount 갯수 보여주기
    /// </summary>
    public class DrawAmmoCount : MonoBehaviour
    {
        #region Variables
        //UI 텍스트
        public TextMeshProUGUI ammoCountText;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //ammo UI
            ammoCountText.text = PlayerStats.Instance.AmmonCount.ToString();
        }
        #endregion
    }
}