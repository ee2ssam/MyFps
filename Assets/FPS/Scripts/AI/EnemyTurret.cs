using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.AI
{
    //이동하지 못하는 Enemy 상태를 구현하는 클래스
    [RequireComponent(typeof(EnemyController))]
    public class EnemyTurret : MonoBehaviour
    {
        #region Variables
        //참조
        private EnemyController enemyController;
        private AudioSource audioSource;
        public Animator animator;

        //데미지 입을때 효과
        public ParticleSystem[] randomHitSparks;

        //디텍팅
        public ParticleSystem[] onDetectedVfx;
        public AudioClip onDetectedSfx;

        //공격        

        //애니메이터 파라미터 값
        const string k_AminOnDamagedParameter = "OnDamaged";
        const string k_AminIsActiveParameter = "IsActive";
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
            aiState = AIState.Ready;
            
            //enemyController 이벤트 함수에 등록
            enemyController.onDamaged += OnDamaged;
            enemyController.onDetectedTarget += OnDetectedTarget;
            enemyController.onLostTarget += OnLostTarget;
        }

        private void Update()
        {
            //적 상태 구현
            UpdateCurrentAiState();
        }
        #endregion

        #region Custom Method
        private void UpdateCurrentAiState()
        {
            switch (aiState)
            {   
                case AIState.Attack:                    
                    //총구를 타겟을 향해 돌린다
                    enemyController.OrientWeaponsTowards(enemyController.KnownDetectedTarget.transform.position);

                    //타겟을 향해 공격
                    enemyController.TryAttack(enemyController.KnownDetectedTarget.transform.position);
                    break;
            }
        }

        

        //데미지를 입으면 hit spark중 랜덤하게 하나 플레이, 애니메이션 처리
        private void OnDamaged()
        {
            //파티클 플레이
            if (randomHitSparks.Length > 0)
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
            Debug.Log("적을 찾았다 - 공격 시작");
            //상태 변경
            if (aiState == AIState.Ready)
            {
                aiState = AIState.Attack;
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
            animator.SetBool(k_AminIsActiveParameter, true);
        }

        //적을 잃어버리면 호출되는 함수
        private void OnLostTarget()
        {
            Debug.Log("적을 잃어버렸다 - 대기");
            //상태 변경
            if (aiState == AIState.Attack)
            {
                aiState = AIState.Ready;
            }

            //연출 효과: Vfx
            for (int i = 0; i < onDetectedVfx.Length; i++)
            {
                onDetectedVfx[i].Stop();
            }

            //애니 설정
            animator.SetBool(k_AminIsActiveParameter, false);
        }
        #endregion
    }
}
