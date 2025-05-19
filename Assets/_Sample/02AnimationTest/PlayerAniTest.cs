using UnityEngine;

namespace MySample
{
    public class PlayerAniTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        //이동
        [SerializeField]
        private float moveSpeed = 5f;

        //인풋 
        private float moveX;
        private float moveY;

        //애니메이션 파라미터
        [SerializeField]
        private string moveMode = "MoveMode";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            animator = this.GetComponent<Animator>();

        }


        private void Update()
        {
            //인풋
            moveX = Input.GetAxis("Horizontal");       //ad, 좌우 화살표
            moveY = Input.GetAxis("Vertical");         //ws, 위아래 화살표  -1 ~ 1

            //방향
            Vector3 dir = new Vector3(moveX, 0f, moveY);
            transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);

            //애니메이션
            //AnimationStateTest();
            AnimationBlendTest();
        }

        //
        private void AnimationBlendTest()
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }

        //2. 앞뒤좌우 이동시 앞뒤좌우 애니메이션 플레이 시켜준다
        //3. 이동이 없을 때에는 대기 애니메이션을 플레이 한다
        private void AnimationStateTest()
        {
            if(moveX == 0f && moveY == 0f)
            {
                animator.SetInteger(moveMode, 0);
            }
            else
            {
                //앞뒤좌우
                if (moveX < 0)
                {
                    animator.SetInteger(moveMode, 3);
                }
                else if (moveX > 0)
                {
                    animator.SetInteger(moveMode, 4);
                }
                if (moveY > 0)
                {
                    animator.SetInteger(moveMode, 1);
                }
                else if (moveY < 0)
                {
                    animator.SetInteger(moveMode, 2);
                }
            }
        }
        #endregion
    }


}

/*
1. WASD 입력 받아 플레이어 앞뒤좌우 이동하고 (old input)

2. 앞뒤좌우 이동시 앞뒤좌우 애니메이션 플레이 시켜준다
3. 이동이 없을 때에는 대기 애니메이션을 플레이 한다

4. 이동속도 5
*/

