using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어 인풋을 관리하는 클래스 : 올드인풋
    /// </summary>
    public class CharacterInput : MonoBehaviour
    {
        #region Variables
        //이동 입력 값
        private Vector2 move;
        //마우스 입력 값
        [SerializeField]
        private Vector2 look;

        //뛰기 입력 값
        private bool sprint = false;
        #endregion

        #region Property
        public Vector2 Move => move;    //이동 입력 값 읽기 전용
        public Vector2 Look => look;    //마우스 입력 값 읽기 전용
        public bool Sprint => sprint;   //뛰기 입력 값 읽기 전용
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //wsad 이동 입력값 처리
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            MoveInput(moveX, moveY);

            //마우스 입력 처리
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            LookInput(mouseX, mouseY);
        }
        #endregion

        #region Custom Method
        private void MoveInput(float x, float y)
        {
            move = new Vector2(x, y);
        }

        private void LookInput(float x, float y)
        {
            look = new Vector2(x, y);
        }
        #endregion
    }
}