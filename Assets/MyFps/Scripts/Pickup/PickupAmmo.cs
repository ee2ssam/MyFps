using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 아이템 줍기 - 탄환 7개를 지급 했습니다
    /// </summary>
    public class PickupAmmo : Pickup
    {
        #region Variables
        [SerializeField]
        private int giveAmmo = 7;   //ammo 지급 갯수
        #endregion

        protected override bool OnPickup()
        {
            PlayerStats.Instance.AddAmmo(giveAmmo);
            return true;
        }
    }
}