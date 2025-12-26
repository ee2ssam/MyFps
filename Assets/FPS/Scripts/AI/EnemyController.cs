using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Game;
using System;
using System.Collections.Generic;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 렌더러를 관리하는 구조체
    /// </summary>
    [Serializable]
    public struct RedererIndexData
    {
        public Renderer renderer;
        public int materialIndex;

        //생성자
        public RedererIndexData(Renderer _renderer, int index)
        {
            renderer = _renderer;
            materialIndex = index;
        }
    }

    /// <summary>
    /// Enemy의 공통적인 상태를 관리하는 클래스
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Health health;

        //데미지 효과
        public UnityAction onDamaged;           //데미지를 입었을때 등록된 함수 호출

        public Material bodyMaterial;           //데미지를 입는 메터리얼
        [GradientUsage(true)] public Gradient onHotBodyGradient;    //데미지 입었을때 변하는 그라디언트 컬러

        [SerializeField] private float flashOnHitDuration = 0.5f;   //데미지 플래시하는 시간
        private float lastTimeDamaged = float.NegativeInfinity;

        //사운드
        public AudioClip damageSfx;     //데미지 사운드
        private bool wasDamageThisFrame = false;    

        //bodyMaterial를 가지고 있는 모든 렌더러 리스트
        private List<RedererIndexData> bodyRenderers = new List<RedererIndexData>();
        private MaterialPropertyBlock bodyFlashMaterialProperyBlock;    //플래시 효과 특성값을 가진 블럭

        //죽음 효과
        public GameObject deathVfxPrefab;           //죽음 효과 폭발 VFX
        public Transform deathVfxSpawnPosition;     //VFX 스폰 위치
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            health = GetComponent<Health>();
        }

        private void Start()
        {
            //health 이벤트 함수 등록
            health.onDamaged += OnDamaged;
            health.onDeath += OnDie;

            //bodyMaterial를 가진 렌더러 찾아 리스트 추가
            foreach (var rend in GetComponentsInChildren<Renderer>(true))
            {
                for (int i = 0; i < rend.sharedMaterials.Length; i++)
                {
                    if (rend.sharedMaterials[i] == bodyMaterial)
                    {
                        bodyRenderers.Add(new RedererIndexData(rend, i));
                    }
                }
            }

            bodyFlashMaterialProperyBlock = new MaterialPropertyBlock();
        }

        private void Update()
        {
            //body 메터리얼 컬러 변경
            Color currentColor = onHotBodyGradient.Evaluate((Time.time - lastTimeDamaged)/flashOnHitDuration);
            bodyFlashMaterialProperyBlock.SetColor("_EmissionColor", currentColor);
            foreach (var data in bodyRenderers)
            {
                data.renderer.SetPropertyBlock(bodyFlashMaterialProperyBlock, data.materialIndex);
            }

            //
            wasDamageThisFrame = false;
        }
        #endregion

        #region Custom Method
        private void OnDamaged(float damage, GameObject damageSource)
        {
            //팀킬 여부 체크
            if(damageSource && !damageSource.GetComponent<EnemyController>())
            {
                //등록된 함수 호출
                onDamaged?.Invoke();

                //데미지 효과
                //VFX
                lastTimeDamaged = Time.time;

                //SFX
                if(damageSfx && wasDamageThisFrame == false)
                {
                    AudioUtility.CreatSFX(damageSfx, transform.position, 0f);
                }
                //
                wasDamageThisFrame = true;
            }
        }

        private void OnDie()
        {
            //VFX - 폭발 효과
            GameObject effectGo = Instantiate(deathVfxPrefab, deathVfxSpawnPosition.position, 
                Quaternion.identity);
            Destroy(effectGo, 5);

        }
        #endregion
    }
}