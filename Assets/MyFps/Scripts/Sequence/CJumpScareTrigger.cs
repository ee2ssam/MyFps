using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

namespace MyFps
{
    //적 등장 트리거 연출
    public class CJumpScareTrigger : MonoBehaviour
    {
        #region Variables
        //문열리는 애니메이션
        public Animator animator;

        //문열리는 사운드
        public AudioSource doorBang;

        //적 오브젝트
        public GameObject enemy;

        //적 등장 사운드
        public AudioSource jumpScare;

        //애니메이션 파라미터
        private string isOpen = "IsOpen";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //플레이어 체크
            if(other.tag == "Player")
            {
                //트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        //트리거 연출 구현
        IEnumerator SequencePlayer()
        {
            //문이 열린다
            animator.SetBool(isOpen, true);

            //문이 열리는 사운드
            doorBang.Play();

            //적 등장 
            enemy.SetActive(true);

            //1초 딜레이
            yield return new WaitForSeconds(1f);

            //적 등장 사운드 플레이
            jumpScare.Play();

            //로봇의 상태가 걷기 상태로 변경
            Robot robot = enemy.GetComponent<Robot>();
            if(robot)
            {
                robot.ChangeState(RobotState.R_Walk);
            }
        }
        #endregion
    }
}

