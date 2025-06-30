using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.AI;

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

    //Enemy health, patrol 데이터를 관리하는 클래스
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

        //Patrol
        private int pathDestinationNodeIndex;   //이동할 목표 노드 인덱스
        [SerializeField]
        private float pathReachingRadius = 1f;       //도착 체크 범위
        #endregion

        #region Property
        //Patrol
        public NavMeshAgent Agent { get; private set; } 
        //패트롤 할 path - enemy 등록되어 있는 path
        public PatrolPath patrolPath { get; set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            health = this.GetComponent<Health>();
            Agent = this.GetComponent<NavMeshAgent>();

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

        //패트롤 path 유효 여부 체크
        private bool IsPathVaild()
        {
            return patrolPath && patrolPath.pathNodes.Count > 0;
        }

        //패트롤 path 목표 노드 리셋
        private void ResetPathDestination()
        {
            pathDestinationNodeIndex = 0;
        }

        //현재 위치에서 가장 가까운 노드 구해서 목표 노드 설정
        private void SetPathDestinationToClosestNode()
        {
            //패스 체크
            if(IsPathVaild() == false)
            {
                ResetPathDestination();
                return;
            }

            //최단거리에 있는 노드
            int closestPathNodeIndex = 0;
            float closetDistance = float.PositiveInfinity;
            for (int i = 0; i < patrolPath.pathNodes.Count; i++)
            {
                float distance = patrolPath.GetDistanceToNode(transform.position, i);
                if(distance < closetDistance)
                {
                    closetDistance = distance;
                    closestPathNodeIndex = i;
                }
            }

            pathDestinationNodeIndex = closestPathNodeIndex;
        }

        //이동할 목표 위치 구하기
        public Vector3 GetDestinationOnPath()
        {
            //패스 체크
            if(IsPathVaild() == false)
            {
                return this.transform.position;
            }

            return patrolPath.GetPositionOfPathNode(pathDestinationNodeIndex);
        }

        //Agent 이동 목표 설정하기
        public void SetNavDestination(Vector3 destination)
        {
            //Agent 체크
            if (Agent == null)
                return;

            Agent.SetDestination(destination);
        }

        //도착 체크 및 다음 목표 지점 설정하기 - 매개변수로 패트롤 방향(오름차순, 내리차순) 설정
        public void UpdatePathDestination(bool inversOrder = false)
        {
            //패스 체크
            if (IsPathVaild() == false)
                return;

            //도착 판정
            float distance = (transform.position - GetDestinationOnPath()).magnitude;            
            if(distance <= pathReachingRadius)
            {
                //도착 했으면 다음 목표 인덱스 설정
                pathDestinationNodeIndex = inversOrder ? pathDestinationNodeIndex - 1 : pathDestinationNodeIndex + 1;
                //pathDestinationNodeIndex가 범위를 벗어났을때 처리
                if (pathDestinationNodeIndex < 0)
                {
                    pathDestinationNodeIndex += patrolPath.pathNodes.Count;
                }
                if(pathDestinationNodeIndex >= patrolPath.pathNodes.Count)
                {
                    pathDestinationNodeIndex -= patrolPath.pathNodes.Count;
                }
            }
        }
        #endregion
    }
}