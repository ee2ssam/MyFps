using UnityEngine;
using Unity.FPS.Game;

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
        [SerializeField] private float speed = 20f;             //이동 속도
        [SerializeField] private float gravityDown = 0f;        //중력 적용

        public Transform root;                  //발사체의 위치
        public Transform tip;                   //발사체의 머리 위치

        private Vector3 lastRootPosition;       //발사체의 지난 프레임에서의 위치
        private Vector3 velocity;               //발사체의 속도

        private float shootTime;
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



            //
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


        }
        #endregion

    }
}
