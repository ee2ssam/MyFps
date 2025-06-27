using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Unity.FPS.AI
{
    //랜더러 데이터를 관리하는 구조체
    public struct RendererIndexData
    {
        public Renderer renderer;
        public int materialIndex;

        //생성자 - 매개변수로 입력받은 데이터로 초기화
        public RendererIndexData(Renderer _renderer, int index)
        {            
            renderer = _renderer;
            materialIndex = index;
        }
    }

    //Enemy를 관리하는 클래스
    [RequireComponent(typeof(Health))]
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Health health;

        public GameObject deathVfxPrefab;
        public Transform deathVfxSpawnPosition;

        //데미지 처리
        public UnityAction onDamaged;           //데미지 입을때 호출되는 이벤트 함수

        public Material bodyMaterial;           //데미지 효과를 구현할 메터리얼
        [GradientUsage(true)]
        public Gradient onHitBodyGradient;      //데미지 효과를 그라이언트 색 변환 효과

        private float flashOnHitDuration = 0.5f;    //색 변환 플래시 효과 시간

        public AudioClip damageSfx;             //데미지 사운드 효과

        //bodyMaterial 을 가진 랜더러 리스트,
        private List<RendererIndexData> bodyRenderers = new List<RendererIndexData>();
        //Material 속성 변경
        private MaterialPropertyBlock bodyFlashMaterialPropertyBlock;

        private float lastTimeDamaged = float.NegativeInfinity;
        private bool wasDamagedThisFrame = false;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            health = this.GetComponent<Health>();
        }

        private void Start()
        {
            //health 이벤트 함수 등록
            health.OnDamaged += OnDamaged;
            health.OnDie += OnDie;

            //bodyMaterial 가져오기
            foreach(var renderer in GetComponentsInChildren<Renderer>(true))
            {
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    if (renderer.sharedMaterials[i] == bodyMaterial)
                    {
                        //리스트에 랜더러와 메터리얼 인덱스를 구조체 형식으로 저장
                        bodyRenderers.Add(new RendererIndexData(renderer, i));
                    }
                }
            }

            //메터릴얼 속성 변경을 위한 MaterialPropertyBlock 객체 만들기
            bodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            //데미지 컬러 효과
            Color currentColor = onHitBodyGradient.Evaluate((Time.time - lastTimeDamaged) /flashOnHitDuration);
            //메터릴얼 속성 변경 내용 샛팅
            bodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);
            //변경내용 메터리얼 적용
            foreach(var data in bodyRenderers)
            {
                data.renderer.SetPropertyBlock(bodyFlashMaterialPropertyBlock, data.materialIndex);
            }

            //한프레임에 데미지 사운드 한번 플레이
            wasDamagedThisFrame = false;
        }
        #endregion

        #region Custom Method
        //helath OnDamaged 실행시 호출되는 함수
        private void OnDamaged(float damage, GameObject damageSource)
        {
            if(damageSource && !damageSource.GetComponent<EnemyController>())
            {
                //onDamaged에 등록되어 있는 함수 호출
                onDamaged?.Invoke();

                //데미지 효과 - 데미지 입은 시간 저장
                lastTimeDamaged = Time.time;

                //데미지 효과 - Sfx
                if (damageSfx && wasDamagedThisFrame == false)
                {
                    AudioUtility.CreateSFX(damageSfx, transform.position, 0f);
                }
                wasDamagedThisFrame = true;
            }
        }

        //helath OnDid 실행시 호출되는 함수
        private void OnDie()
        {
            //죽었을때 효과 - Vfx
            GameObject effectGo = Instantiate(deathVfxPrefab, deathVfxSpawnPosition.position, Quaternion.identity);
            Destroy(effectGo, 5f);

            //...
        }
        #endregion
    }
}