using UnityEngine;

namespace MyFps
{
    //로봇 상태
    public enum RobotState
    {
        R_Idle = 0,
        R_Walk,
        R_Attack,
        R_Death,
        R_Patrol,
        R_Chase
    }

    //enemy(로봇)의 상태 제어하는 클래스
    public class Robot : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        public Transform thePlayer; //타겟

        private RobotHealth robotHealth;

        //로봇의 현재 상태
        private RobotState robotState;
        //로봇의 이전 상태
        private RobotState beforeState;

        //이동
        [SerializeField]
        private float moveSpeed = 5f;

        //공격
        [SerializeField]
        private float attackRange = 1.5f;

        //공격력
        [SerializeField]
        private float attackDamage = 5f;

        //공격 타이머
        [SerializeField]
        private float attackTime = 2f;
        private float countdown;

        //애니메이션 파라미터
        private string enemyState = "EnemyState";

        //배경음 전환
        public AudioSource jumpScare;
        public AudioSource bgm01;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = this.GetComponent<Animator>();
            robotHealth = this.GetComponent<RobotHealth>();
        }

        private void OnEnable()
        {
            //이벤트 함수 등록
            robotHealth.OnDie += OnDie;

            //초기화
            ChangeState(RobotState.R_Idle);
        }

        private void OnDisable()
        {
            //이벤트 함수 해제
            robotHealth.OnDie -= OnDie;
        }

        private void Update()
        {
            if (robotHealth.IsDeath)
                return;

            //이동
            Vector3 target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
            Vector3 dir = target - this.transform.position;
            float distance = Vector3.Distance(transform.position, target);
            //공격범위 체크
            if(distance <= attackRange)
            {
                ChangeState(RobotState.R_Attack);
            }            

            //상태 구현
            switch(robotState)
            {
                case RobotState.R_Idle:
                    break;

                case RobotState.R_Walk:
                    transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);
                    transform.LookAt(target);
                    break;

                case RobotState.R_Attack:                    
                    //공격 범위 체크
                    if(distance > attackRange)
                    {
                        ChangeState(RobotState.R_Walk);
                    }
                    transform.LookAt(target);
                    break;

                case RobotState.R_Death:
                    break;
            }
        }
        #endregion

        #region Custom Method
        //새로운 상태를 매개변수로 받아 새로운 상태로 셋팅
        public void ChangeState(RobotState newState)
        {
            //현재 상태 체크
            if(robotState == newState)
            {
                return;
            }

            //현재 상태를 이전 상태로 저장
            beforeState = robotState;
            //새로운 상태를 현재 상태로 저장
            robotState = newState;

            //상태 변경에 따른 구현 내용
            animator.SetInteger(enemyState, (int)robotState);
        }

        //2초마다 데미지를 5씩 준다
        private void OnAttackTimer()
        {
            countdown += Time.deltaTime;
            if(countdown >= attackTime)
            {
                //타이머 내용
                Attack();

                //타이머 초기화
                countdown = 0;
            }
        }

        public void Attack()
        {
            Debug.Log($"플레이어에게 {attackDamage}를 준다");
            IDamageable damageable = thePlayer.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }

        //죽음시 호출되는 함수
        private void OnDie()
        {
            ChangeState(RobotState.R_Death);

            //배경음 변경
            jumpScare.Stop();
            bgm01.Play();

            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion
    }
}