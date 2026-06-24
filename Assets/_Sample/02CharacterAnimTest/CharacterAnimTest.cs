using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    /// <summary>
    /// 캐릭터 애니메이션을 제어하는 예제 클래스
    /// 뉴 인풋시스템
    /// 기본이 대기 상태
    /// W키가 들어오면 걷기 상태
    /// + Shift 키를 누르면 뛰기 상태
    /// </summary>
    public class CharacterAnimTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        [SerializeField] private bool isMove;
        [SerializeField] private bool isRun;

        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 7f;
        private float moveSpeed = 0f;

        //애니 파라미터 스트링
        private string isMoving = "IsMove";
        private string isRunning = "IsRun";

        //인풋 액션
        public InputActionReference moveAction;
        public InputActionReference sprintAction;
        #endregion

        #region Property
        public bool IsMove
        {
            get { return isMove; }
            private set
            {
                isMove = value;
                animator.SetBool(isMoving, value);
            }
        }

        public bool IsRun
        {
            get { return isRun; }
            private set
            {
                isRun = value;
                animator.SetBool(isRunning, value);
            }
        }
        #endregion

        #region Unity Event Method 
        //참조
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            //인풋 액션 활성화
            moveAction.action.Enable();
            sprintAction.action.Enable();
        }

        private void OnDisable()
        {
            //인풋 액션 비활성화
            moveAction.action.Disable(); 
            sprintAction.action.Disable();
        }

        private void Update()
        {
            //인풋 처리
            Vector2 inputMove = moveAction.action.ReadValue<Vector2>();
            IsMove = inputMove != Vector2.zero;

            if(sprintAction.action.WasPressedThisFrame())
            {
                IsRun = true;
            }
            else if (sprintAction.action.WasReleasedThisFrame())
            {
                IsRun = false;
            }
        }
        #endregion
    }
}