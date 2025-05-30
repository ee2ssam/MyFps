using UnityEngine;

namespace MyFps
{
    public class PickupItemTheKey : PickupItem
    {
        #region Variable
        #endregion

        #region Custom Method
        protected override bool OnPickup()
        {
            //퍼즐 아이템(key) 획득
            Debug.Log("퍼즐 아이템(key) 획득");

            return true;
        }
        #endregion
    }
}
