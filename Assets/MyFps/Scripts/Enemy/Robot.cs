using UnityEngine;

namespace MyFps
{
    //로봇 상태
    public enum RobotState
    {
        R_Idle = 0,
        R_Walk,
        R_Attack,
        R_Death
    }

    //enemy(로봇)을 제어하는 클래스
    public class Robot : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        public Transform thePlayer; //타겟

        //로봇의 현재 상태
        private RobotState robotState;
        //로봇의 이전 상태
        private RobotState beforeState;

        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

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
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            animator = this.GetComponent<Animator>();

            //초기화
            currentHealth = maxHealth;
        }

        private void OnEnable()
        {
            //초기화
            ChangeState(RobotState.R_Idle);
        }

        private void Update()
        {
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

        //데미지 입기
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"robot currentHealth: {currentHealth}");

            //데미지 연출 (Sfx, Vfx)

            if (currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽기
        private void Die()
        {
            isDeath = true;

            //죽음 처리
            ChangeState(RobotState.R_Death);

            //보상처리..
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
            PlayerController playerController = thePlayer.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.TakeDamage(attackDamage);
            }
        }
        #endregion
    }
}