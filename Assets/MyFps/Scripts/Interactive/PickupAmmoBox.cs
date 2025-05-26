using UnityEngine;

namespace MyFps
{
    //AmmoBox 아이템 획득 : Ammo 7개 지급
    public class PickupAmmoBox : Interactive
    {
        #region Variables
        [SerializeField]
        private int giveAmmo = 7;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //Debug.Log("탄환 7개를 지급 했습니다");
            PlayerDataManager.Instance.AddAmmo(giveAmmo);

            //이펙트 Vfx, Sfx

            //아이템 제거 : 트리거 비활성화, 킬
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        #endregion
    }
}