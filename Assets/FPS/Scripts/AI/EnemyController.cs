using UnityEngine;
using UnityEngine.AI;
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
        private Actor actor;
        private Collider[] selfColliders;

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

        //패트롤
        private int pathDestinationNodeIndex;   //이동 목표 노드 인덱스
        [SerializeField] private float pathReachingRadius = 1f;  //도착 판정

        //디텍팅
        public UnityAction onDetectedTarget;    //디텍팅되면 등록되어 있는 함수 호출
        public UnityAction onLostTarget;        //타겟을 읽어버리면 등록되어 있는 함수 호출

        //공격
        public UnityAction onAttack;            //공격시 등록된 함수 호출
        private float orientSpeed = 10f;

        //무기
        public bool swapToNextWeapon = false;
        public float delayAfterWeaponSwap = 0f;

        private float lastTimeWeaponSwapped = Mathf.NegativeInfinity;
        private int currentWeaponIndex;
        private WeaponController currentWeapon;
        private WeaponController[] weapons;
        #endregion

        #region Property
        //패트롤
        public NavMeshAgent Agent { get; private set; }
        //패트롤 할 패스
        public PatrolPath PatrolPath { get; set; }

        //디텍팅
        public DetectionModule DetectionModule { get; private set; }
        public GameObject KnownDetectedTarget => DetectionModule.KnownDetectedTarget;
        public bool HadKnownTarget => DetectionModule.HadKnownTarget;
        public bool IsSeeingTarget => DetectionModule.IsSeeingTarget;
        public bool IsTargetInAttackRange => DetectionModule.IsTargetInAttackRange;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            health = GetComponent<Health>();
            Agent = GetComponent<NavMeshAgent>();

            actor = GetComponent<Actor>();
            selfColliders = GetComponentsInChildren<Collider>();

            DetectionModule = GetComponentInChildren<DetectionModule>();
        }

        private void Start()
        {
            //DetectionModule 이벤트 함수 등록
            DetectionModule.onDetectedTarget += OnDetectedTarget;
            DetectionModule.onLostTarget += OnLostTarget;

            //health 이벤트 함수 등록
            health.onDamaged += OnDamaged;
            health.onDeath += OnDie;

            //무장
            FindAndInitializeAllWeapon();
            var weapon = GetCurrentWeapon();
            weapon.ShowWeapon(true);

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
            //디텍팅
            DetectionModule.HandleTargetDetection(actor, selfColliders);

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

        //패트롤 패트 유효 체크
        public bool IsPathVaild()
        {
            return PatrolPath && PatrolPath.PathNodes.Count > 0;
        }

        //목표 지점 초기화
        public void ResetPathDestination()
        {
            pathDestinationNodeIndex = 0;
        }

        //가장 가까운 노드 찾아서 목표 노드로 셋팅
        public void SetPathDestinationToClosestNode()
        {
            //패스 체크
            if(IsPathVaild())
            {
                int closestPathNodeIndex = 0;
                for(int i = 0; i < PatrolPath.PathNodes.Count; i++)
                {
                    float distanceToNode = PatrolPath.GetDistanceToNode(transform.position, i);
                    if(distanceToNode < PatrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                    {
                        closestPathNodeIndex = i;
                    }
                }
                pathDestinationNodeIndex = closestPathNodeIndex;
            }
            else
            {
                pathDestinationNodeIndex = 0;
            }
        }

        //이동 목표 위치 반환
        public Vector3 GetDestinationOnPath()
        {
            //패스 체크
            if (IsPathVaild())
            {
                return PatrolPath.GetPositionOfPathNode(pathDestinationNodeIndex);
            }
            else
            {
                return transform.position;
            }
        }

        //Agent 목표지점 설정
        public void SetNavDestination(Vector3 destination)
        {
            //Agent 체크
            if (Agent)
            {
                Agent.SetDestination(destination);
            }
        }

        //패트롤
        public void UpdatePathDestination(bool inverseOrder = false)
        {
            //패스 체크
            if (IsPathVaild())
            {
                //도착 판정후 다음 노드 지정
                float distance = (transform.position - GetDestinationOnPath()).magnitude;
                if(distance < pathReachingRadius)
                {
                    pathDestinationNodeIndex = inverseOrder ?
                        (pathDestinationNodeIndex - 1) : (pathDestinationNodeIndex + 1);
                    if(pathDestinationNodeIndex < 0)
                    {
                        pathDestinationNodeIndex += PatrolPath.PathNodes.Count;
                    }
                    if(pathDestinationNodeIndex >= PatrolPath.PathNodes.Count)
                    {
                        pathDestinationNodeIndex -= PatrolPath.PathNodes.Count;
                    }
                }
            }
        }

        private void OnDetectedTarget()
        {
            onDetectedTarget?.Invoke();    //디텍팅되면 등록되어 있는 함수 호출

            //enemy의 디텍팅 연출 - eye 레드 컬러 변경
        }

        private void OnLostTarget()
        {
            onLostTarget?.Invoke();        //타겟을 읽어버리면 등록되어 있는 함수 호출

            //enemy의 디텍팅 연출 - eye 디폴트 컬러 변경
        }

        //무기 셋팅
        private void FindAndInitializeAllWeapon()
        {
            if (weapons == null)
            {
                weapons = GetComponentsInChildren<WeaponController>();
                for (int i = 0; i < weapons.Length; i++)
                {
                    weapons[i].Owner = this.gameObject;
                }
            }
        }

        //현재 들고 있는 무기 셋팅
        private void SetCurrentWeapon(int index)
        {
            currentWeaponIndex = index;
            currentWeapon = weapons[currentWeaponIndex];
            if(swapToNextWeapon)
            {
                lastTimeWeaponSwapped = Time.time;
            }
            else
            {
                lastTimeWeaponSwapped = Mathf.NegativeInfinity;
            }
        }

        //현재 들고 있는 무기 반환
        public WeaponController GetCurrentWeapon()
        {
            FindAndInitializeAllWeapon();
            if(currentWeapon == null)
            {
                SetCurrentWeapon(0);
            }

            return currentWeapon;
        }

        //타겟를 향해 모든 무기가 바라보도록 만든다
        public void OrientWeaponsTowards(Vector3 lookPosition)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                Vector3 weaponForward = (lookPosition - weapons[i].transform.position).normalized;
                weapons[i].transform.forward = weaponForward;
            }
        }

        public bool TryAttack(Vector3 targetPosition)
        {
            OrientWeaponsTowards(targetPosition);

            if((lastTimeWeaponSwapped + delayAfterWeaponSwap) >= Time.time)
            {
                return false;
            }

            bool didFire = GetCurrentWeapon().HandleShootInputs(true, false, false);

            if (didFire && onAttack != null)
            {
                onAttack?.Invoke();
            }

            return didFire;
        }
        #endregion
    }
}