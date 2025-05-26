using TMPro;
using UnityEngine;

namespace MyFps
{
    //아이템(권총) 획득 인터랙티브 구현
    public class PickupPistol : Interactive
    {
        #region Variables        
        //인터랙티브 액션 연출
        public GameObject realPistol;
        public GameObject ammoUI;
        public GameObject ammoBox;
        public GameObject secondTrigger;

        public GameObject theArrow;
        #endregion

        #region Custom Method  
        protected override void DoAction()
        {
            //무기획득, 충돌체 제거
            theArrow.SetActive(false);

            realPistol.SetActive(true);
            ammoUI.SetActive(true);
            ammoBox.SetActive(true);
            secondTrigger.SetActive(true);

            //무기 데이터 저장
            PlayerDataManager.Instance.Weapon = WeaponType.Pistol;

            this.gameObject.SetActive(false);   //fake pistol 및 충돌체 제거                    
        }
        #endregion
    }
}
