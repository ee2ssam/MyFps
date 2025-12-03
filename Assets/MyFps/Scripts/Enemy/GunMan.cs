using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    //적 공통 상태 정의
    public enum EnemyState
    {
        E_Idle,         //대기
        E_Walk,         //패트롤
        E_Chase,        //추격        
        E_Attack,       //공격
        E_Death         //죽임
    }


    /// <summary>
    /// GunMan Enemy를 관리하는 클래스
    /// </summary>
    public class GunMan : MonoBehaviour, IDamageable
    {
        #region Variables
        //참조
        private Animator animator;
        private NavMeshAgent agent;
        private Transform thePlayer;

        //상태 관리
        [SerializeField]
        private EnemyState currentState;    //현재 상태
        private EnemyState beforeState;     //이전 상태

        //체력
        private float health;
        [SerializeField]
        private float maxHealth = 20f;

        private bool isDeath = false;
        [SerializeField]
        private float destoryDelay = 6f;

        //상태 - 대기
        private float idleTimer = 2f; //2~3초
        private float countdown = 0f;

        //상태 - 패트롤
        public Transform[] wayPoints;
        [SerializeField]
        private int wayPointIndex = 0;

        //상태 - 추격
        [SerializeField]
        private float detectDistance = 10f;     //적이 디텍트 거리안에 들어오면 추격 시작

        //상태 - 공격
        [SerializeField]
        private float attackRange = 5f;         //적이 사거리 안에 들어오면 추격을 멈추고 공격 시작
        [SerializeField]
        private float attackTimer = 2f;         //2초에 한번씩 발사
        [SerializeField]
        private float attackDamage = 5f;        //발사시 플레이어에게 attackDamage(5) 준다

        private bool isBack = false;

        //애니메이터 파라미터
        const string MoveSpeed = "MoveSpeed";
        const string IsDeath = "IsDeath";
        const string Fire = "Fire";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();

            thePlayer = FindFirstObjectByType<Player>().transform;
        }

        private void Start()
        {
            //초기화
            health = maxHealth;
            wayPointIndex = 1;

            SetState(EnemyState.E_Idle);
        }

        private void Update()
        {
            //죽음 체크
            if (isDeath)
                return;

            //디텍팅 
            float distacne = Vector3.Distance(thePlayer.position, transform.position);
            if (distacne <= attackRange && isBack == false)            //공격 거리 체크
            {
                SetState(EnemyState.E_Attack);
            }
            else if (distacne <= detectDistance && isBack == false)    //디텍팅 거리 체크
            {
                SetState(EnemyState.E_Chase);
            }

            //상태 처리
            switch (currentState)
            {
                case EnemyState.E_Idle:
                    countdown += Time.deltaTime;
                    if(countdown >= idleTimer)
                    {
                        //타이머 기능
                        SetState(EnemyState.E_Walk);
                    }
                    break;

                case EnemyState.E_Walk: //패트롤                
                    //도착 판정
                    if (agent.remainingDistance < 0.1f)
                    {
                        wayPointIndex++;
                        if(wayPointIndex >= wayPoints.Length)
                        {
                            wayPointIndex = 0;
                        }

                        SetState(EnemyState.E_Idle);
                    }
                    break;

                case EnemyState.E_Chase: //추격
                    agent.SetDestination(thePlayer.position);

                    if (distacne > detectDistance)    //디텍팅 거리 체크
                    {
                        SetState(EnemyState.E_Walk);
                    }
                    break;

                case EnemyState.E_Attack:
                    countdown += Time.deltaTime;
                    if (countdown >= attackTimer)
                    {
                        //타이머 기능
                        Shoot();

                        //타이머 초기화
                        countdown = 0;
                    }
                    break;
            }

            //애니메이터 파라미터 처리
            animator.SetFloat(MoveSpeed, agent.velocity.magnitude);
        }

        //디텍팅 거리 기즈모 그리기
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, detectDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
        #endregion

        #region Custom Method
        //상태 전환
        public void SetState(EnemyState newState)
        {
            //현재 상태 체크
            if(currentState == newState)
                return;

            beforeState = currentState; //현재 상태를 바로 이전 상태에 저장
            currentState = newState;    //현재 상태를 새로운 상태로 전환

            //상태별 초기값 설정
            switch (currentState)
            {
                case EnemyState.E_Idle:
                    //타이머 초기화
                    idleTimer = Random.Range(2f, 3f);
                    break;

                case EnemyState.E_Walk:
                    //이동 목표 지점 설정
                    agent.SetDestination(wayPoints[wayPointIndex].position);
                    break;

                case EnemyState.E_Attack:
                    //멈춤 - 이동 목표 지점을 현재 위치로 지정
                    agent.SetDestination(this.transform.position);
                    break;

                case EnemyState.E_Death:
                    animator.SetBool(IsDeath, true);
                    break;
            }

            if (currentState == EnemyState.E_Chase || currentState == EnemyState.E_Attack)
            {
                animator.SetLayerWeight(1, 1f);
            }
            else
            {
                animator.SetLayerWeight(1, 0f);
            }

            //타이머 초기화
            countdown = 0f;
        }

        //데미지 처리
        public void TakeDamage(float damage)
        {
            health -= damage;

            //효과(vfx, sfx), UI, 애니메이션 

            if(health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽음 처리
        private void Die()
        {
            isDeath = true;

            SetState(EnemyState.E_Death);

            //효과(vfx, sfx), UI, 애니메이션, 보상

            //킬
            Destroy(gameObject, destoryDelay);
        }

        //Enemy 공격
        private void Shoot()
        {
            //애니메이션
            animator.SetTrigger(Fire);

            //효과 vfx, sfx

            //타겟에게 데미지 주기
            IDamageable damageable = thePlayer.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }

        //문이 열릴때 호출되는 함수
        public void OnActive()
        {
            isBack = false;
        }

        //문이 닫힐때 호출되는 함수
        public void GoBack()
        {
            isBack = true;
            SetState(EnemyState.E_Walk);
        }
        #endregion
    }
}
