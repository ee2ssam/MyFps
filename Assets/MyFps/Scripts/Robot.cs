using UnityEngine;

namespace MyFps
{
    //로봇 상태 정의
    public enum RobotState
    {
        R_Idle,
        R_Walk,
        R_Attack,
        R_Death
    }

    /// <summary>
    /// 로봇을 관리하는 클래스
    /// 애니메이션, 체력, 이동
    /// </summary>
    public class Robot : MonoBehaviour
    {
        #region Variables
        //참조
        public Animator animator;

        //로봇의 현재 상태
        [SerializeField]
        private RobotState robotState;
        //바로 이전 상태
        private RobotState beforeState;

        //체력
        private float health;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //플레이어 오브젝트
        public Transform thePlayer;

        //애니메이션 파라미터
        private const string EnemyState = "EnemyState";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            health = 20f;
            SetState(RobotState.R_Idle);
        }

        private void Update()
        {
            //상태 구현
            switch (robotState)
            {
                case RobotState.R_Idle:
                    break;

                case RobotState.R_Walk:
                    break;

                case RobotState.R_Attack:
                    break;

                case RobotState.R_Death:
                    break;
            }
        }
        #endregion

        #region Custom Method
        //로봇의 상태 변경
        private void SetState(RobotState newState)
        {
            //현재 상태 체크
            if (newState == robotState)
                return;

            //이전 상태 저장
            beforeState = robotState;

            //새로운 상태로 변경
            robotState = newState;

            //새로운 상태 변경에 따른 구현 내용
            animator.SetInteger(EnemyState, (int)robotState);
        }

        //데미지 주기
        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log($"Robot Health: {health}");

            //죽음 체크 - 두번 죽이지 마라
            if (health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽음 처리
        private void Die()
        {
            isDeath = true;

            //Death 상태 변경
            SetState(RobotState.R_Death);
        }
        #endregion
    }
}