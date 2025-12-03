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
    public class Robot : MonoBehaviour, IDamageable
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
        private Transform thePlayer;

        //대기
        [SerializeField]
        private float idleTimer = 2f;
        private float countdown = 0f;

        //이동
        [SerializeField]
        private float moveSpeed = 5f;

        //공격
        [SerializeField]
        private float attackRange = 2f;
        [SerializeField]
        private float attackTimer = 2f;
        [SerializeField]
        private float attackDamage = 5f;

        //리워드
        [SerializeField]
        private int rewardGold = 0;
        [SerializeField]
        private float rewardExp = 0f;
        //private Item rewardItem;
        public GameObject rewardItemPrefab;

        //애니메이션 파라미터
        private const string EnemyState = "EnemyState";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            //thePlayer = GameObject.Find("Player").transform;
            thePlayer = FindFirstObjectByType<PlayerMove>().transform;
        }

        private void Start()
        {
            //초기화
            health = maxHealth;
            countdown = 0f;

            SetState(RobotState.R_Idle);
        }

        private void Update()
        {
            //타겟팅
            Vector3 targetPosition = new Vector3(thePlayer.position.x, transform.position.y, thePlayer.position.z);
            Vector3 dir = targetPosition - transform.position;
            float distance = Vector3.Distance(targetPosition, transform.position);

            //상태 구현
            switch (robotState)
            {
                //3초후에 걷기로 상태 전환
                case RobotState.R_Idle:
                    countdown += Time.deltaTime;
                    if(countdown >= idleTimer)
                    {
                        //타이머 기능
                        SetState(RobotState.R_Walk);

                        //타이머 초기화
                        countdown = 0f;
                    }
                    break;

                //플레이를 향해서 걷기, 플레이어와의 거리가 2이내가 되면 공격 상태로 전환
                case RobotState.R_Walk:
                    transform.Translate(dir.normalized * Time.deltaTime * moveSpeed, Space.World);

                    //플레이어와의 거리가 attackRange(2)이내가 되면 공격 상태로 전환
                    if (distance <= attackRange)
                    {
                        SetState(RobotState.R_Attack);
                    }

                    //타겟을 바라본다
                    transform.LookAt(targetPosition);
                    break;

                //2초마다 데미지를 5씩 준다, 플레이어와의 거리가 1.5가 넘어가면 걷기 상태로 전환
                case RobotState.R_Attack:
                    /*countdown += Time.deltaTime;
                    if (countdown >= attackTimer)
                    {
                        //타이머 기능
                        Attack();

                        //타이머 초기화
                        countdown = 0f;
                    }*/

                    //플레이어와의 거리가 attackRange(2)가 넘어가면 걷기 상태로 전환
                    if(distance > attackRange)
                    {
                        SetState(RobotState.R_Walk);
                    }
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

            //데스 킬 처리
            if(robotState == RobotState.R_Death)
            {
                Destroy(gameObject, 6f);
            }
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

            //리워드 처리
            //AddGold(rewardGold);
            //AddExp(rewardExp);
            //AddInventory(rewardItem);
            Debug.Log("보상을 지급하였습니다");
            //필드에 아이템 떨구기
            if(rewardItemPrefab != null)
            {
                Instantiate(rewardItemPrefab, this.transform.position + new Vector3(0f,1f,0f), Quaternion.identity);
            }

            //Death 상태 변경
            SetState(RobotState.R_Death);
        }

        //공격
        public void Attack()
        {
            //Debug.Log($"플레이어에게 데미지 {attackDamage}를 준다");
            /*PlayerHealth playerHealth = thePlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }*/
            IDamageable damageable = thePlayer.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }


        }
        #endregion
    }
}