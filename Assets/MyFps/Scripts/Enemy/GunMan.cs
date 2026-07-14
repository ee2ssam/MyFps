using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    //적 캐릭터 상태 정의
    public enum EnemyState
    {
        E_Idle,         //대기
        E_Walk,         //걷기 (패트롤)
        E_Chase,        //추격
        E_Attack,       //공격
        E_Death         //죽기
    }

    /// <summary>
    /// 총을 쏘는 적 캐릭터를 관리하는 클래스
    /// </summary>
    public class GunMan : MonoBehaviour, IDamageable
    {
        #region Variables
        //참조
        private Animator animator;
        private NavMeshAgent agent;
        private Transform thePlayer;

        //상태
        [SerializeField] private EnemyState currentState;       //현재 상태
        private EnemyState beforeState;                         //이전 상태

        //체력
        [SerializeField] private float maxHealth = 20f;
        private float currentHealth = 0f;
        private bool isDeath = false;       //죽음 체크

        //대기 상태
        [SerializeField] private float idleTimer = 2f;          //웨이 포인트 도착시 다음 포인트 출발전 2초 대기
        private float countdown = 0f;

        //패트롤 여부
        [SerializeField] private bool isPatrol = false;       
        public Transform[] wayPoints;                           //웨이 포인트
        private int wayPointIndex = 0;                          //다음 포인트 지점 인덱스

        //처음 스폰 위치
        private Vector3 startPosition = Vector3.zero;

        //추격 상태
        [SerializeField] private float detectDistance = 10f;   //적이 디텍팅 거리에 들어오면 추격 시작

        //공격 상태
        [SerializeField] private float attackRange = 5f;        //적이 사거리 안에 들어오면 추격을 멈추고 공격 시작
        [SerializeField] private float attackTimer = 2f;        //총 발사 간격
        private float attackCountdown = 0f;
        [SerializeField] private float attackDamage = 5f;       //공격 데미지

        //애니메이션 파라미터
        private const string MoveSpeed = "MoveSpeed";
        private const string IsDeath = "IsDeath";
        private const string FireTrigger = "FireTrigger";
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
            currentHealth = maxHealth;
            startPosition = this.transform.position;            
            ChangeState(EnemyState.E_Idle);

            isPatrol = wayPoints.Length >= 2 ? true : false;
            wayPointIndex = 1;
        }

        private void Update()
        {
            //죽음 체크
            if (isDeath)
                return;

            //상태 구현
            switch(currentState)
            {
                case EnemyState.E_Idle:
                    if(isPatrol)
                    {
                        countdown += Time.deltaTime;
                        if(countdown > idleTimer)
                        {
                            //다음 포인트 지점으로 이동(패트롤)
                            ChangeState(EnemyState.E_Walk);

                            //초기화
                            countdown = 0f;
                        }
                    }
                    break;
                case EnemyState.E_Walk: //패트롤
                    //도착 판정
                    if(agent.remainingDistance < 0.1f)
                    {
                        //인덱스 증가
                        if(isPatrol)
                        {
                            wayPointIndex++;
                            if(wayPointIndex >= wayPoints.Length)
                            {
                                wayPointIndex = 0;
                            }
                        }

                        ChangeState(EnemyState.E_Idle);
                    }
                    break;

                case EnemyState.E_Chase:
                    break;
                case EnemyState.E_Attack:
                    break;
                case EnemyState.E_Death:
                    break;
            }
        }
        #endregion


        #region Custom Method
        public void ChangeState(EnemyState newState)
        {
            //상태 변경전에 현재상태를 이전상태에 저장
            beforeState = currentState;

            //새로운 상태로 변경
            currentState = newState;

            //새로운 상태변경에 따른 처리사항 구현
            switch (newState)
            {
                case EnemyState.E_Idle:
                    //타이머 초기화
                    countdown = 0f;
                    break;
                case EnemyState.E_Walk:
                    //이동 목표 지점 설정
                    if (isPatrol)
                    {
                        agent.SetDestination(wayPoints[wayPointIndex].position);
                    }
                    break;
            }

            //...
        }

        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }


        #endregion

    }
}