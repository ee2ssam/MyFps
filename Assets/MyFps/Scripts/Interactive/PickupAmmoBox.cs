using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// AmmoBox 아이템 획득하기
    /// </summary>
    public class PickupAmmoBox : PickupItem
    {
        #region Variablse
        [SerializeField]
        private int giveAmmo = 7;   //ammo 지급 갯수
        #endregion

        protected override void DoAction()
        {
            //Debug.Log("탄환 7개를 지급 했습니다");
            PlayerStats.Instance.AddAmmo(giveAmmo);

            //아이템 킬
            Destroy(gameObject);
        }
    }
}