using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// Ammo UI 그리기
    /// 손에 총을 들고 있으면 Ammo UI 그리고, 손에 총이 없으면 Ammo UI 안그리기
    /// </summary>
    public class DrawAmmoUI : MonoBehaviour
    {
        #region Unity Event Method
        private void Start()
        {
            //Ammo UI 그리기
            bool isShow = PlayerStats.Instance.WeaponType != WeaponType.None;
            ShowAmmoUI(isShow);
        }
        #endregion

        #region Custom Method
        public void ShowAmmoUI(bool isShow)
        {
            gameObject.SetActive(isShow);
        }
        #endregion
    }
}
