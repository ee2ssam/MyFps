using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Utility;
using UnityEngine.AI;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 렌더러와 관련된 데이터 정의
    /// </summary>
    [Serializable]
    public struct RendereIndexData
    {
        public Renderer renderer;
        public int materialIndex;

        //생성자
        public RendereIndexData(Renderer _renderer, int index)
        {
            renderer = _renderer;
            materialIndex = index;
        }
    }

    /// <summary>
    /// 적의 공통적인 상태를 관리하는 클래스
    /// 적의 데미지 처리, 죽음 처리
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Health health;

        //damage
        public UnityAction onDamaged;       //적이 데미지 입었을때 등록된 함수 호출

        public Material bodyMaterial;                               //적의 몸체 메터리얼
        [GradientUsage(true)] public Gradient OnhitBodyGradient;    //데미지 연출되는 컬러 그라이언트

        //바디메터리얼이 있는 랜더러와 바디메터리얼 인덱스를 가진 구조체 리스트
        private List<RendereIndexData> bodyRenderers= new List<RendereIndexData>();
        private MaterialPropertyBlock bodyFlashMaterialPropertyBlock;

        [SerializeField] private float flashOnHitDuration = 0.5f;   //플레시 효과 지속 시간
        private float lastTimeDamaged = float.NegativeInfinity; //데미지를 입은 시간
        private bool wasDamagedThisFrame = false;               //이번 프레임에 데미지 입었는지 체크

        public AudioClip damageSfx;                             //데미지 사운드 클립

        //death
        public GameObject deathVfxPrefab;
        public Transform deathVfxSpawnPosition;
        public AudioClip deathSfx;

        //이동, 패트롤
        public NavMeshAgent Agent { get; private set; }

        //디텍팅
        public DetectionModule DetectionModule { get; private set; }

        //- 평상시의 eyeColor, 디텍팅 되었을때의 eyeColor (Red)
        public Material eyeMaterial;
        private RendereIndexData eyeRendererData;
        private MaterialPropertyBlock eyeColorMaterialPropertyBlock;

        //공격
        private WeaponController[] weapons;     //무기 슬롯
        private WeaponController currentWeapon; //현재 들고 있는 무기

        //EnemyManager
        private EnemyManager enemyManager;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //EnemyManager 등록
            enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();
            enemyManager.RegisterEnemy(this);

            //참조
            health = GetComponent<Health>();
            Agent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            //health 이벤트 함수 등록
            health.onDamaged += OnDamaged;
            health.onDeath += OnDeath;
            health.onHeal += OnHeal;
        }

        private void OnDisable()
        {
            //health 이벤트 함수 등록
            health.onDamaged -= OnDamaged;
            health.onDeath -= OnDeath;
            health.onHeal -= OnHeal;
        }

        private void Start()
        {
            //초기화
            foreach(var renderer in GetComponentsInChildren<Renderer>(true))
            {
                for(int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if(renderer.sharedMaterials[i] == bodyMaterial)
                    {
                        bodyRenderers.Add(new RendereIndexData(renderer, i));
                    }
                }
            }
            //MaterialPropertyBlock 객체 생성
            bodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            //데미지에 따른 메터리얼 컬러 변경
            Color currentColor = OnhitBodyGradient.Evaluate((Time.time - lastTimeDamaged)/flashOnHitDuration);
            bodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
            foreach(var data in bodyRenderers)
            {
                data.renderer.SetPropertyBlock(bodyFlashMaterialPropertyBlock, data.materialIndex);
            }

            wasDamagedThisFrame = false;
        }
        #endregion

        #region Custom Method
        //데미지 처리
        private void OnDamaged(float damage, GameObject damageSource)
        {
            //damageSource 체크
            if(damageSource && damageSource.GetComponent<EnemyController>())
            {
                return;
            }

            onDamaged?.Invoke();

            //데미지 효과(vfx, sfx)
            lastTimeDamaged = Time.time;

            //sfx
            if(damageSfx && wasDamagedThisFrame == false)
            {
                AudioUtility.CreateSFX(damageSfx, transform.position, 0f);
            }
            wasDamagedThisFrame = true;
        }

        //죽음 처리
        private void OnDeath()
        {
            //EnemyManager 제거
            enemyManager.RemoveEnemy(this);


            //이펙트 효과
            GameObject vfxGo = Instantiate(deathVfxPrefab, deathVfxSpawnPosition.position, Quaternion.identity);
            Destroy(vfxGo, 3f);

            //sfx
            if (deathSfx)
            {
                AudioUtility.CreateSFX(deathSfx, deathVfxSpawnPosition.position, 0f);
            }

            //적 킬
            Destroy(gameObject);
        }

        //힐 처리
        private void OnHeal(float amount)
        {
            
        }
        #endregion

    }
}