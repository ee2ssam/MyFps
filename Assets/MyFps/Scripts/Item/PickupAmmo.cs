using UnityEngine;

namespace MyFps
{
    public class PickupAmmo : PickupItem
    {
        #region Variables
        [SerializeField] private int giveAmount = 7;
        #endregion

        protected override bool OnPickup()
        {
            //탄환 7개 지급
            PlayerStats.Instance.AddAmmo(giveAmount);

            return true;
        }
    }
}