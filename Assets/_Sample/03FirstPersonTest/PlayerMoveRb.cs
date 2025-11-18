using UnityEngine;

namespace MyFps
{
    public class PlayerMoveRb : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody rb;
        private CharacterInput _input;

        //점프
        [SerializeField]
        private float jumpForce = 10f;

        //이동
        [SerializeField]
        private float moveForce = 5f;

        [Header("Player Grounded")]
        //그라운드 체크
        [SerializeField]
        private bool grounded = false;

        [SerializeField]
        private float groundedOffset = -0.14f;  //체크 포지션 조정값
        [SerializeField]
        private float groundedRadius = 0.5f;    //체크 포지션을 기준으로 체크 범위 영역
        public LayerMask groundLayers;          //그라운드 레이어 설정
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb = GetComponent<Rigidbody>();
            _input = GetComponent<CharacterInput>();
        }

        private void FixedUpdate()
        {
            //그라운드 체크
            GroundedCheck();

            //항력
            if(grounded)    //지상에 있을때 값 설정
            {
                if (_input.Move != Vector2.zero)    //이동중
                {
                    rb.linearDamping = 5f;
                }
                else //멈추어 있을때
                {
                    rb.linearDamping = 10f;
                }
            }
            else //공중에 있을때 값 설정
            {
                rb.linearDamping = 2f;
            }

            //점프
            if (_input.Jump && grounded)
            {
                //위로 힘을 준다
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                _input.Jump = false;
            }

            //이동
            rb.AddForce(transform.right * _input.Move.x * moveForce, ForceMode.Force);
            rb.AddForce(transform.forward * _input.Move.y * moveForce, ForceMode.Force);
        }
        #endregion

        #region Custom Method
        //그라운드 체크
        private void GroundedCheck()
        {
            //체크 위치 설정
            Vector3 spherePosition = new Vector3(transform.position.x,
                transform.position.y - groundedOffset, transform.position.z);
            //체크 위치에서 그라운드 범위 안에 있는지 체크
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }
        #endregion
    }
}

/*
리지디바디
- 다이나믹 리지디바디 (키네틱 x)
: 내가 상대 오브젝트에게 물리적 영향을 주고 상대 오브젝트도 나에게 영향을 준다
- 키네틱 리지디바디 (키네틱 체크)
: 내가 상대 오브젝트에게 물리적 영향을 주지만 상대 오브젝트는 나에게 영향을 주지 않는다
: 외부에서 오는 모든 힘을 무시한다

ForceMode
ForceMode.Force (연속적인 힘, 질량 고려)
- 바람, 자기력 
ForceMode.Acceleration (연속적임 힘, 질량 무시)
- 중력, 질량에 상관없이 일정한 가속을 구현할때

ForceMode.Impulse (불연속적인 힘(1회성), 질량 고려)
- 폭발, 타격, 순간적으로 적용하는 힘

ForceMode.VelocityChange (불연속적인 힘(1회성), 질량 무시)
- 순간적으로 지정한 속도의 변화를 일으킬때

*/
