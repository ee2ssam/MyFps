using UnityEngine;
using System;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 크로스헤어 데이터 정의, 직렬화 구조체
    /// </summary>
    [Serializable]
    public struct CrosshairData
    {
        public Sprite CrossHairSprite;
        public float CorssHairSize;
        public Color CrossHairColor;
    }

    /// <summary>
    /// 무기 슛 타입 정의
    /// </summary>
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        
    }

    /// <summary>
    /// 총기류 무기를 관리하는 클래스
    /// </summary>
    [RequireComponent (typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioSource shootAudioSource;

        //무기 활성화, 비활성
        public GameObject weaponRoot;

        //무기 교체 효과음
        public AudioClip switchWeaponSfx;

        //크로스헤어 - 기본
        public CrosshairData defalutCrossHair;      //평상시의 크로스 헤어
        public CrosshairData targetInSightCrossHair;         //적이 타겟팅 되었을때

        //조준 - Aim
        [Range(0f, 1f)]
        public float aimZoomRatio = 1f;                 //조준시 줌인 비율
        public Vector3 aimOffset = Vector3.zero;        //조준시 위치 이동 조정 값

        //발사 - shoot
        [SerializeField] private WeaponShootType shootType;     //무기 발사 타입

        private float currentAmmo;                      //현재 소지하고 있는 탄환 갯수
        [SerializeField] private float maxAmmo = 8f;    //탄환 최대 소지 갯수

        [SerializeField] private float delayBeteenShots = 0.5f; //발사 시간 간격, 연사 방지
        private float lastTimeShot;                             //마지막 발사 시간

        //발사 연출
        public Transform weaponMuzzle;                          //총구, 파이어포인트
        public GameObject muzzleFlashPrefab;                    //총구 발사 이펙트 프리팹
        public AudioClip shootSfx;                              //발사 사운드 소스

        //반동 - Recoil
        [Range(0f, 2f)]
        public float recoilForec = 1f;          //뒤로 밀리는 힘

        //발사체 - projectile
        public ProjectileBase prejectilePrefab; //ProjectileBase를 상속받는 발사체 클래스가 있는 프리팹 오브젝트

        private Vector3 lastMuzzlePosition;     //지난 프레임에서의 총구 위치

        [SerializeField] private int bulletsPerShot = 1;         //한번 쏠때 나오는 총알의 갯수
        [SerializeField] private float bulletSpreadAngle = 0f;   //총알이 퍼져나가는 각도, 0:정면으로 나간다
        #endregion

        #region Property
        public GameObject Owner { get; set; }           //무기 주체(주인)
        public GameObject SoucePrefab { get; set; }     //무기 프리팹 오브젝트
        public bool IsWeaponActive { get; private set; }    //무기 활성화 여부, true 이면 현재 들고 있는 무기
        public Vector3 MuzzleWorldVelocity { get; private set; }    //이번 프레임에서의 총구 속도
        public float CurrentCharge { get; private set; }        //현재 충전 량

        public float CurrentAmmoRatio => currentAmmo / maxAmmo; //Ammo 게이지바 율
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            shootAudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            //초기화
            currentAmmo = maxAmmo;
            lastTimeShot = Time.time;
            lastMuzzlePosition = weaponMuzzle.position;
        }

        private void Update()
        {
            //이번 프레임에서의 총구 속도 계산
            if(Time.deltaTime > 0f)
            {
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;
                lastMuzzlePosition = weaponMuzzle.position;
            }
        }
        #endregion

        #region Custom Method
        //무기 교체 - show: 활성화, 비활성
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            //무기 교체 효과음
            if (show == true && switchWeaponSfx != null)
            {
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }
            IsWeaponActive = show;
        }

        //발사 인풋 처리 - 매개변수로 파이어 인풋을 받는다
        public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            switch(shootType)
            {
                case WeaponShootType.Manual:
                    if(inputDown == true)
                    {
                        return TryShoot();
                    }
                    break;

                case WeaponShootType.Automatic:
                    if(inputHeld == true)
                    {
                        return TryShoot();
                    }
                    break;

                case WeaponShootType.Charge:
                    break;
            }

            return false;
        }

        //발사: 성공, 실패
        private bool TryShoot()
        {
            //Debug.Log("Shoot!!!");
            if(currentAmmo >= 1f && lastTimeShot + delayBeteenShots <= Time.time)
            {
                currentAmmo -= 1f;
                Debug.Log($"currentAmmo: {currentAmmo}");

                //슛 연출
                HandleShoot();

                return true;
            }

            return false;
        }

        //발사 연출
        private void HandleShoot()
        {
            //발사체 생성
            int bulletsPerShotFinal = bulletsPerShot;   //생성할 발사체 갯수

            for (int i = 0; i < bulletsPerShotFinal; i++)
            {
                Vector3 shotDirection = GetShotDirectionWithInSpread(weaponMuzzle);
                ProjectileBase objectProjectile = Instantiate(prejectilePrefab, weaponMuzzle.position,
                    Quaternion.LookRotation(shotDirection));
                objectProjectile.Shoot(this);
            }


            //vfx: muzzle 이펙트
            if(muzzleFlashPrefab)
            {
                GameObject effectGo = Instantiate(muzzleFlashPrefab, 
                    weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
                Destroy(effectGo, 2f);
            }

            //sfx: 슛
            if(shootSfx)
            {
                shootAudioSource.PlayOneShot(shootSfx);
            }

            //슛 타임 저장
            lastTimeShot = Time.time;
        }

        //발사체가 퍼져 나가는 랜덤한 방향 구하기
        private Vector3 GetShotDirectionWithInSpread(Transform shootTransform)
        {
            float spreadAngleRatio = bulletSpreadAngle / 180f;
            return Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);
        }
        #endregion
    }
}