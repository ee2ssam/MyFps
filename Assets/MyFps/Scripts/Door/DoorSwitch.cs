using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 등록된 문의 열기, 닫기 구현
    /// 인터랙티브 액션으로 이벤트 구현, 인터랙티브 상속 받는다
    /// </summary>
    public class DoorSwitch : Interactive
    {
        #region Variables
        public Door door;       //문닫기, 열기할 문 게임오브젝트
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            throw new System.NotImplementedException();
        }
        #endregion

    }
}
