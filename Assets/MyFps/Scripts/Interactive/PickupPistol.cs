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
        public GameObject theArrow;
        #endregion

        #region Custom Method  
        protected override void DoAction()
        {
            //무기획득, 충돌체 제거
            realPistol.SetActive(true);
            theArrow.SetActive(false);

            this.gameObject.SetActive(false);   //fake pistol 및 충돌체 제거                    
        }
        #endregion
    }
}
