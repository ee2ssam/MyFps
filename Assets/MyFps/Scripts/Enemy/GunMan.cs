using UnityEngine;
using UnityEngine.AI;

namespace MyFps
{
    //건맨
    public class GunMan : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        private NavMeshAgent agent;
        public Transform thePlayer; //타겟        

        private RobotHealth gunmanHealth;

        //로봇의 현재 상태
        private RobotState gunmanState;
        //로봇의 이전 상태
        private RobotState beforeState;

        //패트롤
        public Transform[] wayPoints;
        private int nowWayPointIndex = 0;

        //대기 타이머
        [SerializeField] private float idleTime = 2f;
        private float idleCountdown = 0f;

        //디텍팅
        private Vector3 target;                 //플레이어의 위치

        [SerializeField]
        private float detectingRange = 15f;
        private float distance;

        private Vector3 originPosition;

        //공격 : 총 발사 : 멈추고 Fire 애니메이션
        [SerializeField]
        private float attackRange = 5f;

        [SerializeField] private float attackTime = 2f;
        private float attackCountdown = 0f;
        //공격 데미지
        [SerializeField]
        private float attackDamage = 5f;

        //애니메이션 파라미터
        private string enemyState = "EnemyState";
        private string fire = "Fire";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = this.GetComponent<Animator>();
            agent = this.GetComponent<NavMeshAgent>();
            gunmanHealth = this.GetComponent<RobotHealth>();
        }

        private void OnEnable()
        {
            //이벤트 함수 등록
            gunmanHealth.OnDie += OnDie;
        }

        private void OnDisable()
        {
            //이벤트 함수 해제
            gunmanHealth.OnDie -= OnDie;
        }

        private void Start()
        {
            //초기화
            originPosition = transform.position;

            ChangeState(RobotState.R_Idle);
        }

        private void Update()
        {
            //죽음 체크
            if (gunmanHealth.IsDeath)
                return;

            //플레이어 위치 확인 - 안전지역에 있으면
            if(PlayerController.safeZoneIn)
            {
                if(gunmanState == RobotState.R_Attack || gunmanState == RobotState.R_Chase)
                {
                    Debug.Log("제자리에 돌아가라");
                    //제자리에 돌아가라
                    BackHome();
                    return;
                }
            }
            else //Enemy Zone에 있는 경우
            {
                //적 디텍팅
                target = new Vector3(thePlayer.position.x, 0f, thePlayer.position.z);
                distance = Vector3.Distance(transform.position, target);
                if (distance <= attackRange) //5
                {
                    //공격 상태로 변경                
                    ChangeState(RobotState.R_Attack);
                }
                else if (distance <= detectingRange) //15
                {
                    //추격 상태로 변경
                    ChangeState(RobotState.R_Chase);
                }
            }

            //상태 구현
            switch (gunmanState)
            {
                case RobotState.R_Idle:
                    if (wayPoints.Length > 1)
                    {
                        idleCountdown += Time.deltaTime;
                        if (idleCountdown >= idleTime)
                        {
                            //패트롤 상태 변환
                            ChangeState(RobotState.R_Patrol);

                            //타이머 초기화
                            idleCountdown = 0f;
                        }
                    }
                    break;
                case RobotState.R_Walk:
                    //웨이포인트 도착 판정
                    if (agent.remainingDistance <= 0.2f)
                    {
                        ChangeState(RobotState.R_Idle);
                    }
                    break;
                case RobotState.R_Attack:
                    //공격 타이머
                    attackCountdown += Time.deltaTime;
                    if (attackCountdown >= attackTime)
                    {
                        //발사
                        animator.SetTrigger(fire);

                        //타이머 초기화
                        attackCountdown = 0f;
                    }
                    //타겟을 바라본다
                    transform.LookAt(target);
                    break;
                case RobotState.R_Death:
                    break;
                case RobotState.R_Patrol:
                    //웨이포인트 도착 판정
                    if (agent.remainingDistance <= 0.2f)
                    {
                        ChangeState(RobotState.R_Idle);
                    }
                    break;
                case RobotState.R_Chase:
                    //타겟을 향해 이동 (실시간으로 목표 지점을 변경)
                    agent.SetDestination(target);

                    //플레이어가 디텍팅 거리에서 벗어나면
                    if (distance > detectingRange) //15
                    {
                        //제자리로 되돌아가기
                        BackHome();
                    }
                    break;
            }
        }

        //적 디텍팅 범위 그리기
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectingRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        #endregion

        #region Custom Method
        //새로운 상태를 매개변수로 받아 새로운 상태로 셋팅
        public void ChangeState(RobotState newState)
        {
            //현재 상태 체크
            if (gunmanState == newState)
            {
                return;
            }

            //현재 상태를 이전 상태로 저장
            beforeState = gunmanState;
            //새로운 상태를 현재 상태로 저장
            gunmanState = newState;

            //상태 변경에 따른 구현 내용
            if (gunmanState == RobotState.R_Patrol)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Walk);
                //다음 웨이포인트로 이동
                GoNextWayPoint();
            }
            else if (gunmanState == RobotState.R_Idle)
            {
                animator.SetInteger(enemyState, (int)gunmanState);
                idleTime = Random.Range(2f, 5f);

                //네비게이션 패스 초기화
                agent.ResetPath();
            }
            else if (gunmanState == RobotState.R_Chase)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Walk);
                //타겟을 향해 이동
                agent.SetDestination(target);
                //조준 애니 적용
                animator.SetLayerWeight(1, 1);
            }
            else if (gunmanState == RobotState.R_Attack)
            {
                animator.SetInteger(enemyState, (int)RobotState.R_Idle);
                //조준 애니 적용
                animator.SetLayerWeight(1, 1);

                //타겟을 향한 이동 멈춤
                agent.ResetPath();
            }
            else
            {
                animator.SetInteger(enemyState, (int)gunmanState);
            }   
        }

        //다음 웨이포인트로 이동
        private void GoNextWayPoint()
        {
            nowWayPointIndex++;
            if(nowWayPointIndex >= wayPoints.Length)
            {
                nowWayPointIndex = 0;
            }
            agent.SetDestination(wayPoints[nowWayPointIndex].position);
        }

        //제자리로 돌아가기
        private void BackHome()
        {
            //패트롤 여부 체크
            if(wayPoints.Length > 1)
            {
                ChangeState(RobotState.R_Patrol);
            }
            else
            {
                ChangeState(RobotState.R_Walk); //지정한 위치로 이동
                agent.SetDestination(originPosition);
            }
            //조준 애니 푼다
            animator.SetLayerWeight(1, 0);
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

            //추가 구현 내용..
            agent.enabled = false;

            this.GetComponent<BoxCollider>().enabled = false;
        }
        #endregion

    }
}
