using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 이동하는 Enemy를 관리하는 클래스
    /// </summary>
    public class EnemyMobile : MonoBehaviour
    {
        //이동하는 Enemy AI 상태 정의
        public enum AIState
        {
            Patrol,
            Follow,
            Attack
        }

        #region Variables
        //참조
        private EnemyController enemyController;
        private AudioSource audioSource;
        public Animator animator;

        //데미지 VFX
        public ParticleSystem[] randomHitSparks;

        //이동
        public AudioClip movementSound;         //이동 사운드 소스
        public MinMaxFloat pitchMovementSpeed;  //이동속도에 따른 재생 속도 최소,최대값

        //애니메이터 파라미터
        private const string k_AnimOnDamagedParameter = "OnDamaged";
        #endregion

        #region Property
        public AIState AiState { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            enemyController = GetComponent<EnemyController>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            //enemyController 이벤트 함수 등록
            enemyController.onDamaged += OnDamaged;

            //초기화
            AiState = AIState.Patrol;
            //사운드 셋팅
            audioSource.clip = movementSound;
            audioSource.Play();
        }

        private void Update()
        {
            //AI 상태 구현
            UpdateAiState();
        }
        #endregion

        #region Custom Method
        //AI 상태 구현
        private void UpdateAiState()
        {
            switch (AiState)
            {
                case AIState.Patrol:
                    break;

                case AIState.Follow:
                    break;

                case AIState.Attack:
                    break;
            }
        }

        private void OnDamaged()
        {
            //데이지 효과 구현
            //스파크 파티클 랜덤 플레이
            if(randomHitSparks.Length > 0)
            {
                int randNumber = Random.Range(0, randomHitSparks.Length);
                randomHitSparks[randNumber].Play();
            }

            //애니메이터
            animator.SetTrigger(k_AnimOnDamagedParameter);
        }
        #endregion
    }
}