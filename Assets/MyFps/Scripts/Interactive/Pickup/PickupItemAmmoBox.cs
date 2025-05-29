using UnityEngine;

namespace MyFps
{
    public class PickupItemAmmoBox : PickupItem
    {
        #region Variables
        //아이템 먹는 효과
        [SerializeField]
        private int giveAmmo = 7;
        #endregion

        #region Custom Method
        protected override bool OnPickup()
        {
            //Ammobox 아이템을 먹을수 있는지 체크
            //Ammobox 아이템 먹었을때의 효과 구현
            PlayerDataManager.Instance.AddAmmo(giveAmmo);

            return true;
        }
        #endregion

    }
}

/*
1. 플레이어가 부딪히는 충돌 체크 : 충돌하면
- 탄환 7개 지급
*/