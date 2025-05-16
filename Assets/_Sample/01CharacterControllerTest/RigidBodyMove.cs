using UnityEngine;
using UnityEngine.InputSystem;

namespace MySample
{
    public class RigidBodyMove : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;

        //입력 - 이동
        private Vector2 inputMove;

        //이동 시키는 힘
        [SerializeField]
        private float movePower = 10f;

        //중력 체크
        private bool isGround = false;

        //점프
        [SerializeField]
        private float jumpPower = 10f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            rb = this.GetComponent<Rigidbody>();
            capsuleCollider = this.GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            //그라운드 체크
            isGround = GroundCheck();

            //
            if(isGround)
            {
                if(inputMove == Vector2.zero)
                {
                    rb.linearDamping = 20f;
                }
                else
                {
                    rb.linearDamping = 5f;
                }
            }
            else
            {
                rb.linearDamping = 5f;
            }

            //방향
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            //이동 - 이동할 방향으로 힘을 준다
            rb.AddForce(moveDir * movePower, ForceMode.Force);
        }
        #endregion

        #region Custom Method
        //Input 시스템에 등록할 함수
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && isGround)
            {
                //위쪽 방향으로 힘을 준다(1회성)
                rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            }
        }

        //그라운드 체크
        private bool GroundCheck()
        {
            RaycastHit hit; //캐스트 정보 담는 변수
            if(Physics.SphereCast(transform.position, capsuleCollider.radius, Vector3.down, out hit,
                (capsuleCollider.height/2 - capsuleCollider.radius) + 0.01f, Physics.AllLayers, QueryTriggerInteraction.Ignore))           
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
