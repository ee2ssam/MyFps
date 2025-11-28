using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이이어의 무기를 관리하는 클래스
    /// 무기 교체...
    /// </summary>
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        public GameObject pistol;
        public GameObject healMatic;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //현재 무장 무기 셋팅
            SetCurrentWeapon(PlayerStats.Instance.WeaponType);
        }
        #endregion

        #region Custom Method
        private void SetCurrentWeapon(WeaponType weaponType)
        {
            pistol.SetActive(false);
            healMatic.SetActive(false);

            if (weaponType == WeaponType.Pistol)
            {
                pistol.SetActive(true);
            }
            else if (weaponType == WeaponType.Healmatic)
            {
                healMatic.SetActive(true);
            }
        }
        #endregion
    }
}