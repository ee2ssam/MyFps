using TMPro;
using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 피스톨 아이템 획득하기
    /// </summary>
    public class PickupPistol : Interactive
    {
        #region Variables
        //액션
        [Header("Interactive Action")]
        public GameObject fakePistol;
        public GameObject realPistol;
        public GameObject theMaker;

        public GameObject ammoUI;

        public WeaponType weaponType = WeaponType.Pistol;
        #endregion

        #region Custom Method
        //Interactive Action
        protected override void DoAction()
        {
            //-테이블 위의 총은 없어지고 - 비활성화
            fakePistol.SetActive(false);
            //- 오른손 쪽의 총은 화면 출력 -활성화
            realPistol.SetActive(true);
            //- 책상위의 가이드 화살표는 없어진다
            theMaker.SetActive(false);

            //현재 소지무기 셋팅
            PlayerStats.Instance.SetWeaponType(weaponType);

            //UI
            ammoUI.SetActive(true);

            //아이템 킬
            Destroy(gameObject);
        }
        #endregion
    }
}