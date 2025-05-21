using UnityEngine;
using TMPro;

namespace MyFps
{
    //문열기 인터렉티브 액션 구현
    public class DoorCellOpen : Interactive
    {
        #region Variables        
        //애니메이션
        public Animator animator;
        //애니 파라미터 스트링
        private string  paramIsOpen = "IsOpen";

        //문여는 소리
        public AudioSource audioSource;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //문열고, 충돌체 제거
            animator.SetBool(paramIsOpen, true);        //문 여는 애니메이션 연출

            //문여는 소리
            audioSource.Play();

            this.GetComponent<BoxCollider>().enabled = false; //문 충돌체 제거
        }
        #endregion
    }
}
