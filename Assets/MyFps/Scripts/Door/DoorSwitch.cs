using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 문 스위치 구현, 인터랙트 구현
    /// 문 스위치는 토글, 문이 열려 있으면 빨간색, 문이 닫혀 있으면 원래색
    /// </summary>
    public class DoorSwitch : Interactive
    {
        #region Variables
        public Door door;   //문 등록
        #endregion

        #region Unity Event Method
        #endregion

        //추상메서드 - 구현하도록 강제하는 기능 정의
        #region abstract
        protected override void DoAction()
        {
            if (door == null)
                return;

            StartCoroutine(Toggle());
        }
        #endregion

        #region Custom Method
        IEnumerator Toggle()
        {
            //콜라이더 기능 제거
            this.GetComponent<BoxCollider>().enabled = false;

            if(door.IsActive)
            {
                door.DeActivate();
                //actionText.text = "Open The Door";
            }
            else
            {
                door.Activate();
                //actionText.text = "Close The Door";
            }

            yield return new WaitForSeconds(1f);

            //콜라이더 기능 복원
            this.GetComponent<BoxCollider>().enabled = true;
        }
        #endregion
    }
}
