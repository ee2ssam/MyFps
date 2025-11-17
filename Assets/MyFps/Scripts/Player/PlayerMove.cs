using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어 이동을 관리하는 클래스
    /// </summary>
    public class PlayerMove : MonoBehaviour
    {
        #region Variables
        //참조
        private CharacterController _controller;    //캐릭터 컨트롤러
        private CharacterInput _input;              //플레이어 인풋

        //이동
        [SerializeField]
        private float moveSpeed = 4f;       //걷는 속도
        [SerializeField]
        private float sprintSpeed = 6f;     //뛰는 속도

        private float speed;                //이동 속도

        //점프
        [SerializeField]
        private float jumpHeight = 1.2f;    //점프 입력시 점프할 높이
        [SerializeField]
        private float gravity = -15.0f;     //중력, 물리 기본값(-9.81f)
        [SerializeField]
        private float jumpTimeout = 0.1f;   //점프 키입력 처리

        [SerializeField]
        private float verticalVelocity;     //y축 속도 연산 결과

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
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<CharacterInput>();
        }

        private void Update()
        {
            JumpGravity();
            GroundedCheck();
            Move();
        }
        #endregion

        #region CustomMethod
        //캐릭터 점프
        private void JumpGravity()
        {
            if(grounded)
            {
                //점프 입력 체크
                if(_input.Jump && jumpTimeout <= 0f)
                {
                    //jumpHeight(1.2f) 만큼 뛰기 위한 속도갑 구하기
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                }

                //점프 타이머
                if(jumpTimeout >= 0f)
                {
                    jumpTimeout -= Time.deltaTime;
                }
            }
            else
            {
                jumpTimeout = 0.1f;
                _input.Jump = false;
            }

            //중력 적용 - 평상시
            verticalVelocity += gravity * Time.deltaTime;
        }

        //그라운드 체크
        private void GroundedCheck()
        {
            //체크 위치 설정
            Vector3 spherePosition = new Vector3(transform.position.x, 
                transform.position.y - groundedOffset, transform.position.z);
            //체크 위치에서 그라운드 범위 안에 있는지 체크
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        //캐릭터 이동
        private void Move()
        {
            speed = _input.Sprint ? sprintSpeed : moveSpeed;

            //이동 인풋값 체크
            if (_input.Move == Vector2.zero)
                speed = 0f;

            //방향값
            Vector3 inputDirection = new Vector3(_input.Move.x, 0f, _input.Move.y).normalized;

            //플레이어 로컬 방향 구하기
            if(_input.Move != Vector2.zero)
            {
                inputDirection = transform.right * _input.Move.x + transform.forward * _input.Move.y;
            }

            //이동 : 방향 * Time.deltatime * speed + 점프(중력) 연산 결과(verticalVelocity)
            _controller.Move(inputDirection.normalized * Time.deltaTime * speed 
                + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

        }
        #endregion
    }
}