using UnityEngine;
using Unity.FPS.Game;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 발사체 기본형
    /// </summary>
    public class ProjectileStandard : ProjectileBase
    {
        #region Variables
        //참조
        private ProjectileBase projectileBase;      //부모 객체
                
        public float maxLifeTime = 5f;      //라이프 타임

        //이동
        public float speed = 20f;           //이동 속도
        public Transform root;              //발사체 오브젝트 기준점
        public Transform tip;               //발사체 맨 앞 기준점

        private Vector3 lastRootPosition;   //이전 프레임에서의 루트 위치
        private Vector3 velocity;           //속도

        public float gravityDown = 0f;      //중력 계수값

        //충돌
        public float radius = 0.01f;        //충돌 체크 범위 (구의 반경)

        public LayerMask hittableLayers = -1;       //충돌 레이어 마스크
        private List<Collider> ignoredColliders;    //충돌 체크에서 제외되는 충돌체 리스트

        //충돌 효과
        public GameObject impactVfxPrefab;          //충돌 이펙트 프리팹
        public float impactVfxLimeTime = 3f;        //충돌 이펙트 라이프 타임
        public float impactVfxSpawnOffset = 0.1f;   //충돌 이펙트 생성 위치 조정

        public AudioClip impactSfxClip;             //충돌 효과 사운드

        //데미지
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //참조
            projectileBase = GetComponent<ProjectileBase>();
            //이벤트 함수 등록
            projectileBase.onShoot += OnShoot;


            //킬 예약
            Destroy(gameObject, maxLifeTime);
        }

        #endregion

        #region Custom Method
        //발사체를 생성시 초기값 설정
        private void OnShoot()
        {
            velocity = transform.forward * speed;
            transform.position += projectileBase.InheritedMuzzleVelocity * Time.deltaTime;
            lastRootPosition = root.position;

            //쏘는 플레이어의 충돌체들 가져와서 충돌 제외 리스트 등록

        }
        #endregion
    }
}