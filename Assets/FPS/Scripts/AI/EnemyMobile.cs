using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.Audio;
using TMPro;

namespace Unity.FPS.AI
{
    //Enemy 상태 enum
    public enum AIState
    {
        Patrol,     
        Follow,
        Attack,
        Ready
    }

    //이동하는 Enemy 상태를 구현하는 클래스
    [RequireComponent(typeof(EnemyController))]
    public class EnemyMobile : MonoBehaviour
    {
        #region Variables
        //참조
        private EnemyController enemyController;
        private AudioSource audioSource;
        public Animator animator;

        //데미지 입을때 효과
        public ParticleSystem[] randomHitSparks;

        //이동 사운드 효과
        public AudioClip moveSound;
        public MinMaxFloat pitchMovementSpeed;      //이동 속도에 따른 재생 속도 min, max 설정값

        //디텍팅
        public ParticleSystem[] onDetectedVfx;
        public AudioClip onDetectedSfx;

        //공격
        //공격상태에서 얼마만큼 목표에 이동하는 설정 값
        [Range(0f, 1f)]
        public float attackStopDistanceRatio = 0.5f;

        //애니메이터 파라미터 값
        const string k_AminAttackParameter = "Attack";
        const string k_AminMoveSpeedParamter = "MoveSpeed";
        const string k_AminAlertedParameter = "Alerted";
        const string k_AminOnDamagedParameter = "OnDamaged";
        const string k_AminOnDeathParameter = "Death";
        #endregion

        #region Property
        //enemy 상태
        public AIState aiState { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            enemyController = this.GetComponent<EnemyController>();
            audioSource = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            //초기화
            aiState = AIState.Patrol;

            //사운드
            audioSource.clip = moveSound;
            audioSource.Play();

            //enemyController 이벤트 함수에 등록
            enemyController.onDamaged += OnDamaged;
            enemyController.onDetectedTarget += OnDetectedTarget;
            enemyController.onLostTarget += OnLostTarget;

        }

        private void Update()
        {
            //적 상태 구현
            UpdateCurrentAiState();
            UpdateAiStateTransition();

            //이동 속도에 따른 애니 설정, 사운드 재생 속도 설정
            float moveSpeed = enemyController.Agent.velocity.magnitude;
            animator.SetFloat(k_AminMoveSpeedParamter, moveSpeed);
            audioSource.pitch = pitchMovementSpeed.GetValueRatio(moveSpeed / enemyController.Agent.speed);
        }
        #endregion

        #region Custom Method
        private void UpdateCurrentAiState()
        {
            switch(aiState)
            {
                case AIState.Patrol:
                    enemyController.UpdatePathDestination(true);    //노드 도착 체크 및 다음 목표 노드 설정
                    enemyController.SetNavDestination(enemyController.GetDestinationOnPath());  //이동할 목표 위치를 구해 Agent 이동 위치 설정
                    break;

                case AIState.Follow:
                    enemyController.SetNavDestination(enemyController.KnownDetectedTarget.transform.position);  //Agent가 타겟을 이동 목표로 설정
                    enemyController.OrientTowards(enemyController.KnownDetectedTarget.transform.position);      //Agent가 타겟을 향해 바란본다
                    enemyController.OrientWeaponsTowards(enemyController.KnownDetectedTarget.transform.position); //타겟를 향해 총구를 돌린다
                    break;

                case AIState.Attack:
                    if(Vector3.Distance(enemyController.KnownDetectedTarget.transform.position, enemyController.detectionModule.detectionSourcePoint.position) 
                        >= attackStopDistanceRatio * enemyController.detectionModule.attackRange)
                    {
                        //Agent가 타겟을 이동 목표로 설정
                        enemyController.SetNavDestination(enemyController.KnownDetectedTarget.transform.position);
                    }
                    else
                    {
                        //Agent의 이동목표를 자신의 위치로 하여 이동을 멈춘다
                        enemyController.SetNavDestination(transform.position);
                    }
                    //총구를 타겟을 향해 돌린다
                    enemyController.OrientWeaponsTowards(enemyController.KnownDetectedTarget.transform.position);

                    //타겟을 향해 공격
                    enemyController.TryAttack(enemyController.KnownDetectedTarget.transform.position);
                    break;
            }
        }

        //상태 변경
        private void UpdateAiStateTransition()
        {
            switch(aiState)
            {
                case AIState.Follow:
                    if(enemyController.IsSeeingTarget && enemyController.IsTargetInAttackRange)
                    {
                        aiState = AIState.Attack;
                    }
                    break;

                case AIState.Attack:
                    if(enemyController.IsTargetInAttackRange == false)
                    {
                        aiState = AIState.Follow;
                    }
                    break;
            }
        }

        //데미지를 입으면 hit spark중 랜덤하게 하나 플레이, 애니메이션 처리
        private void OnDamaged()
        {
            //파티클 플레이
            if(randomHitSparks.Length > 0)
            {
                int randNum = Random.Range(0, randomHitSparks.Length);
                randomHitSparks[randNum].Play();
            }

            //애니메이션
            animator.SetTrigger(k_AminOnDamagedParameter);
        }

        //적을 찾으면 호출되는 함수
        private void OnDetectedTarget()
        {
            Debug.Log("적을 찾았다 - 추격 시작");
            //상태 변경
            if(aiState == AIState.Patrol)
            {
                aiState = AIState.Follow;
            }

            //연출 효과: Vfx
            for (int i = 0; i < onDetectedVfx.Length; i++)
            {
                onDetectedVfx[i].Play();
            }
            //Sfx
            if (onDetectedSfx)
            {
                AudioUtility.CreateSFX(onDetectedSfx, transform.position, 1f);
            }

            //애니 설정
            animator.SetBool(k_AminAlertedParameter, true);
        }

        //적을 잃어버리면 호출되는 함수
        private void OnLostTarget()
        {
            Debug.Log("적을 잃어버렸다 - 패트롤로 다시 가기");
            //상태 변경
            if(aiState == AIState.Follow || aiState == AIState.Attack)
            {
                aiState = AIState.Patrol;
            }

            //연출 효과: Vfx
            for (int i = 0; i < onDetectedVfx.Length; i++)
            {
                onDetectedVfx[i].Stop();
            }

            //애니 설정
            animator.SetBool(k_AminAlertedParameter, false);
        }
        #endregion
    }
}
