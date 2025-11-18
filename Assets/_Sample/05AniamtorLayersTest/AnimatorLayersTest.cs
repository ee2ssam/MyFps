using UnityEngine;


namespace MySample
{
    public class AnimatorLayersTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        //애니메이터 파라미터 이름
        private string IsWalk = "IsWalk";
        #endregion

        #region Property
        public bool Walk
        {
            get
            {
                return animator.GetBool(IsWalk);
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetBool(IsWalk, true);
                animator.SetLayerWeight(1, 1f); //조준 그리기
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                animator.SetBool(IsWalk, false);
                animator.SetLayerWeight(1, 0f); //조준 그리지 않기
            }
        }
        #endregion
    }
}
