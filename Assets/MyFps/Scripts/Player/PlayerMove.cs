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
            Move();
        }
        #endregion

        #region CustomMethod
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

            //이동 : 방향 * Time.deltatime * speed
            _controller.Move(inputDirection.normalized * Time.deltaTime * speed);

        }
        #endregion
    }
}