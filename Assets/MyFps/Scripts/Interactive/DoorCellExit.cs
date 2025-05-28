using UnityEngine;

namespace MyFps
{
    //다음씬 넘어가기: 문을 열고 문여는 소리 플레이, 배경음 종료, 다음씬으로 이동
    public class DoorCellExit : Interactive
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainScene02";

        public Animator animator;   //문여는 애니메이션
        [SerializeField]
        private string isOpen = "IsOpen";

        public AudioSource doorBang;    //문여는 소리
        public AudioSource bgm01;       //배경음
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //문을 열고
            animator.SetBool(isOpen, true);

            //배경음 종료
            bgm01.Stop();
            //문여는 소리 플레이
            doorBang.Play();

            //씬 종료시 처리할 내용 구현
            //....

            //다음씬으로 이동
            fader.FadeTo(loadToScene);

            //충돌체 제거
            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
    }
}
