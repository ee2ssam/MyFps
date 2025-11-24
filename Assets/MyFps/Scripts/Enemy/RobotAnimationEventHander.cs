using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 로봇 애니메에션에서 호출되는 이벤트 함수 정의
    /// </summary>
    public class RobotAnimationEventHander : MonoBehaviour
    {
        #region Variables
        //참조
        private Robot robot;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조 - 부모 오브젝트의 컴포넌트 가져오기
            robot = this.GetComponentInParent<Robot>();
        }
        #endregion

        #region Custom Method
        public void RobotAttack()
        {
            robot.Attack();
        }
        #endregion
    }
}
