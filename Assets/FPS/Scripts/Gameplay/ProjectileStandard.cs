using System.Collections.Generic;
using System.Linq;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ProjectileStandard : ProjectileBase
    {
        #region Variables
        //참조
        private ProjectileBase projectileBase;  //부모 객체

        //라이프 타임
        [SerializeField] private float maxLifeTime = 5f;

        //이동
        public float speed = 20f;
        public Transform root;                          //발사체 오브젝트의 기준점
        public Transform tip;                           //발사체의 맨 앞 기준점

        private Vector3 lastRootPosition;               //이전 프레임에서의 root 위치
        private Vector3 velocity;                       //속도

        public float gravityDown = 0f;    //중력 계산값

        //충돌
        public float radius = 0.01f;     //충돌 체크 범위 (구의 반경)

        public LayerMask HittableLayers = -1;               //충돌 레이어마스크
        private List<Collider> ignoredColliders;            //충돌 체크에 제외되는 충돌체 리스트

        //충돌 효과
        public GameObject impactVfxPrefab;                      //충돌 효과 vfx 이펙트 프리팹
        [SerializeField] private float impactVfxLifeTime = 5f;  //이펙트 라이프타임
        [SerializeField] private float impactVfxSpanwOffset = 0.1f; //이펙트 생성위치 조정값

        public AudioClip impactSfxClip;                         //충돌 효과 sfx 사운드

        //데미지 량
        public float damage = 10f;
        private DamageArea damageArea;                          //범위 공격 객체
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //참조
            projectileBase = GetComponent<ProjectileBase>();
            //이벤트 함수 등록
            projectileBase.onShoot += OnShoot;

            //범위 공격 객체 가져오기
            damageArea = GetComponent<DamageArea>();

            //킬 예약
            Destroy(gameObject, maxLifeTime);
        }

        private void Update()
        {
            //이동
            transform.position += velocity * Time.deltaTime;

            //중력
            if (gravityDown > 0f)
            {
                velocity += Vector3.down * Time.deltaTime * gravityDown;
            }

            //충돌 - Casting한 충돌체중 가장 가까운 hit를 찾는다
            bool isFoundHit = false;                 //충돌했을때 true;
            RaycastHit closetHit = new RaycastHit();
            closetHit.distance = Mathf.Infinity;

            //Sphere Cast
            Vector3 dispalcementSinceLastFrame = tip.position - lastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(lastRootPosition, radius,
                dispalcementSinceLastFrame.normalized, dispalcementSinceLastFrame.magnitude,
                HittableLayers, QueryTriggerInteraction.Collide);
            foreach (RaycastHit hit in hits)
            {
                if (IsHitValid(hit) && hit.distance < closetHit.distance)
                {
                    isFoundHit = true;
                    closetHit = hit;
                }
            }

            if (isFoundHit)
            {
                if (closetHit.distance < 0f)
                {
                    closetHit.point = root.position;
                    closetHit.normal = -transform.forward;
                }

                //Hit 처리
                OnHit(closetHit.point, closetHit.normal, closetHit.collider);
            }

            //이전 프레임에서의 root 위치 저장
            lastRootPosition = root.position;
        }
        #endregion

        #region Custom Method
        //발사체 생성시 초기값 설정
        private void OnShoot()
        {
            velocity = transform.forward * speed;
            transform.position += projectileBase.InheritedMuzzleVelocity * Time.deltaTime;
            lastRootPosition = root.position;

            //쏘는 객체(주체)의 충돌체를 가져와 무시 충돌 리스트 등록
            ignoredColliders = new List<Collider>();
            Collider[] ownerCollider = projectileBase.Owner.GetComponentsInChildren<Collider>();
            ignoredColliders.AddRange(ownerCollider);
        }

        //충돌 유효 체크
        private bool IsHitValid(RaycastHit hit)
        {
            //hit.collider 무효 경우
            //충돌 체크에 제외되는 충돌체 리스트에 있으면 무효
            if (ignoredColliders != null && ignoredColliders.Contains(hit.collider))
            {
                return false;
            }

            //트리거 충돌체가 데미지어블을 가지고 있지 않으면 무효
            if(hit.collider.isTrigger && hit.collider.GetComponent<Damageable>() == null)
            {
                return false;
            }

            //특정(IgnoreHitDetection) 컴포넌트를 가지고 있으면 무효
            if (hit.collider.GetComponent<IgnoreHitDetection>() != null)
            {
                return false;
            }

            return true;
        }

        //Hit 처리
        private void OnHit(Vector3 point, Vector3 normal, Collider collider)
        {
            //데미지 처리
            //범위 공격 여부 체크
            if(damageArea)
            {
                //범위 공격 데미지 계산
                damageArea.InflictDamageArea(damage, point, HittableLayers,
                    QueryTriggerInteraction.Collide, projectileBase.Owner);
            }
            else
            {
                Damageable damageable = collider.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.InflictDamage(damage, false, projectileBase.Owner);
                }
            }   

            //Vfx
            if(impactVfxPrefab)
            {
                GameObject effectGo = Instantiate(impactVfxPrefab, 
                    point + (normal * impactVfxSpanwOffset), Quaternion.LookRotation(normal));
                //킬
                if(impactVfxLifeTime > 0)
                {
                    Destroy(effectGo, impactVfxLifeTime);
                }
            }

            //Sfx
            if(impactSfxClip)
            {
                //Debug.Log("Paly impactSfxClip");
                AudioUtility.CreatSFX(impactSfxClip, transform.position, 1f, 3f);
            }
            
            //킬 (발사체 제거)
            Destroy(gameObject);
        }
        #endregion
    }
}