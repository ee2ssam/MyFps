using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 애니메이터 bool타입 파라미터 값을 설정하는 클래스
    /// 애니메이션 상태(state)에서 들어갈때와 나갈때 값 설정
    /// </summary>
    public class SetBoolStateBehaviour : StateMachineBehaviour
    {
        #region Variables
        public string boolName;     //bool타입 파라미터 이름

        public bool enterValue;     //애니메이션 상태(state)에서 들어갈때 값
        public bool exitValue;      //애니메이션 상태(state)에서 나갈때 값
        #endregion

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolName, enterValue);
        }

        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolName, exitValue);
        }
    }
}