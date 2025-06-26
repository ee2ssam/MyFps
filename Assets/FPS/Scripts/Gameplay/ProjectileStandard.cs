using UnityEngine;
using Unity.FPS.Game;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    //발사체의 기본형
    public class ProjectileStandard : ProjectileBase
    {
        #region Variables
        //생성
        private ProjectileBase projectileBase;  //자신의 부모클래스 객체
        [SerializeField] private float maxLifeTime = 5f;         //발사체 라이프 타임

        //이동
        public float speed = 20f;             //이동 속도
        public float gravityDown = 0f;        //중력 적용

        public Transform root;                  //발사체의 위치
        public Transform tip;                   //발사체의 머리 위치

        private Vector3 lastRootPosition;       //발사체의 지난 프레임에서의 위치
        private Vector3 velocity;               //발사체의 속도

        private float shootTime;

        //충돌
        public float radius = 0.01f;           //충돌체크 반경

        public LayerMask hittableLayers = -1;   //충돌 레이어
        private List<Collider> ignoredColliders;    //충돌 체크 무시 콜라이더 리스트

        //충돌처리
        public GameObject impactVfxPrefab;
        private float impactVfxLifeTime = 5f;       //이펙트 라이프 타임
        private float impactVfxSpawnOffset = 0.1f;

        public AudioClip impactSfxClip;

        //데미지
        public float damage = 40f;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;

            //생성후 라이프 타임 후 킬
            //Destroy(gameObject, maxLifeTime);
        }

        private void Update()
        {
            //이동
            transform.position += velocity * Time.deltaTime;

            //중력
            if(gravityDown > 0f)
            {
                velocity += Vector3.down * gravityDown * Time.deltaTime;
            }

            //충돌 체크
            bool foundHit = false;  //가장 가까운 hit 충돌체를 찾았나?

            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;

            //SphereCastAll
            Vector3 displacementLastFrame = tip.position - lastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(lastRootPosition, radius,
                displacementLastFrame.normalized, displacementLastFrame.magnitude,
                hittableLayers, QueryTriggerInteraction.Collide);

            foreach (var hit in hits)
            {
                //가장 가까운 hit 찾기
                if(IsHitValid(hit) && hit.distance < closestHit.distance)
                {
                    closestHit = hit;
                    foundHit = true;
                }
            }

            //충돌체를 찾았다
            if(foundHit)
            {
                if(closestHit.distance < 0f)
                {
                    closestHit.point = root.position;
                    closestHit.normal = -transform.forward;
                }

                //충돌 처리
                OnHit(closestHit.point, closestHit.normal, closestHit.collider);
            }

            //이전 프레임의 마지막 위치
            lastRootPosition = root.position;
        }
        #endregion

        #region Custom Method
        private new void OnShoot()
        {
            //초기화
            velocity = transform.forward * speed;
            transform.position += projectileBase.InheritedMuzzleVelocity * Time.deltaTime;  //총구 속도 적용

            lastRootPosition = root.position;

            //쏘는 자신 충돌체들을 가져와서 충돌 체크 무시 리스트에 등록
            ignoredColliders = new List<Collider>();
            Collider[] ownerColliders = projectileBase.Owner.GetComponentsInChildren<Collider>();
            ignoredColliders.AddRange(ownerColliders);

            //쏘는 순간 벽(충돌체) 체크하여 벽 뚫는 버그 수정
            PlayerWeaponManager playerWeaponManager = projectileBase.Owner.GetComponent<PlayerWeaponManager>();
            if(playerWeaponManager)
            {
                Vector3 cameraToMuzzle = projectileBase.InitialPosition - playerWeaponManager.weaponCamera.transform.position;
                if(Physics.Raycast(playerWeaponManager.weaponCamera.transform.position,
                    cameraToMuzzle.normalized, out RaycastHit hit, cameraToMuzzle.magnitude,
                    hittableLayers, QueryTriggerInteraction.Collide))
                {
                    if(IsHitValid(hit))
                    {
                        OnHit(hit.point, hit.normal, hit.collider);
                    }
                }
            }
        }

        //hit한 충돌체가 유효한 충돌체인가?
        private bool IsHitValid(RaycastHit hit)
        {
            //IgnoreHitDetection 컴포넌트를 가진 충돌체는 무효
            if(hit.collider.GetComponent<IgnoreHitDetection>())
            {
                return false;
            }

            //trriger && damageable 없는 충돌체
            if (hit.collider.isTrigger && hit.collider.GetComponent<Damageable>() == null)
            {
                return false;
            }

            //ignoredColliders 리스트에 있으면 무효
            if(ignoredColliders != null && ignoredColliders.Contains(hit.collider))
            {
                return false;
            }

            return true;
        }

        //충돌처리
        private void OnHit(Vector3 point, Vector3 normal, Collider collider)
        {
            //데미지
            Damageable damageable = collider.GetComponent<Damageable>();
            if(damageable)
            {
                damageable.InflictDamage(damage, false, projectileBase.Owner);
            }

            //Vfx
            if(impactVfxPrefab)
            {
                GameObject impactObejct = Instantiate(impactVfxPrefab, point + (normal * impactVfxSpawnOffset),
                    Quaternion.LookRotation(normal));
                if(impactVfxLifeTime > 0f)
                {
                    Destroy(impactObejct, impactVfxLifeTime);
                }
            }

            //Sfx
            if(impactSfxClip)
            {
                AudioUtility.CreateSFX(impactSfxClip, point, 1f, 3f);
            }

            //발사체 킬
            Destroy(gameObject);
        }
        #endregion

    }
}
