using UnityEngine;

namespace MySample
{
    public class RetargetingTest : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        private const string Jump = "Jump";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger(Jump);
            }
        }
        #endregion
    }
}