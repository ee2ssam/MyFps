using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 애니메이터 블랜드 트리 테스트
    /// </summary>
    public class BlendTreeTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        //이동속도
        [SerializeField]
        private float moveSpeed = 5f;

        //입력값
        private float moveX;
        private float moveY;

        //
        private string MoveState = "MoveState";
        private string MoveX = "MoveX";
        private string MoveY = "MoveY";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //wasd 입력값 처리
            moveX = Input.GetAxis("Horizontal");    // -1 ~ 0 ~ 1
            moveY = Input.GetAxis("Vertical");      // -1 ~ 0 ~ 1

            //이동, 방향과 속도
            Vector3 dir = new Vector3(moveX, 0f, moveY);
            transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);

            //애니메이션 파라미터 셋팅
            //AnimateState();
            AnimateBlendTree();
        }
        #endregion

        #region Custom Method
        private void AnimateBlendTree()
        {
            animator.SetFloat(MoveX, moveX);
            animator.SetFloat(MoveY, moveY);
        }

        private void AnimateState()
        {
            if (moveX == 0f && moveY == 0f)
            {
                animator.SetInteger(MoveState, 0);      //대기
            }
            if(moveY > 0f)
            {
                animator.SetInteger(MoveState, 1);      //앞
            }
            if (moveY < 0f)
            {
                animator.SetInteger(MoveState, 2);      //뒤
            }
            if (moveX > 0f)
            {
                animator.SetInteger(MoveState, 3);      //우
            }
            if (moveX < 0f)
            {
                animator.SetInteger(MoveState, 4);      //좌
            }
        }
        #endregion
    }
}