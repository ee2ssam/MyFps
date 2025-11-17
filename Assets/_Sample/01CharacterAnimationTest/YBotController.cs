using UnityEngine;

namespace MySample
{
    /// <summary>
    /// YBot을 제어하는 클래스
    /// 인풋 시스템 - 올드 인풋 시스템, 
    /// W키를 누르고 있으면 걷기 애니메이션 플레이
    /// Shift 키를 누르고 있으면 런 애니메이션 플레이
    /// 대기 -> 걷기만 변환 가능, 걷기에서 뛰기로 변환 가능, 대기->뛰기 안됨
    /// </summary>
    public class YBotController : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        //이동
        private float walkSpeed = 4f;
        private float runSpeed = 8f;

        private float speed = 0f;
        [SerializeField]
        private float accelate = 1f;


        //애니메이터 파라미터 이름
        private string IsWalk = "IsWalk";
        private string IsRun = "IsRun";
        private string MoveSpeed = "MoveSpeed";
        #endregion

        #region Property
        public bool Walk
        {
            get
            {
                return animator.GetBool(IsWalk);
            }
        }

        public bool Run
        {
            get
            {
                return animator.GetBool(IsRun);
            }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                animator.SetFloat(MoveSpeed, speed);
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //걷기
            if(Input.GetKeyDown(KeyCode.W))
            {
                animator.SetBool(IsWalk, true);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                animator.SetBool(IsWalk, false);
            }

            //뛰기
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetBool(IsRun, true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.SetBool(IsRun, false);
            }

            //이동 속도 적용
            if(Walk)
            {
                if(Run)
                {
                    Speed += Time.deltaTime * accelate;
                    if(Speed > runSpeed)
                        speed = runSpeed;
                }
                else
                {
                    if(Speed > walkSpeed)
                    {
                        Speed -= Time.deltaTime * accelate;
                        if(Speed < walkSpeed)
                        {
                            Speed = walkSpeed;
                        }
                    }
                    else if (Speed < walkSpeed)
                    {
                        Speed += Time.deltaTime * accelate;
                        if (Speed > walkSpeed)
                        {
                            Speed = walkSpeed;
                        }
                    }
                }
            }
            else
            {
                Speed -= Time.deltaTime * accelate;
                if (Speed < 0)
                {
                    Speed = 0;
                }
            }


        }
        #endregion
    }
}
