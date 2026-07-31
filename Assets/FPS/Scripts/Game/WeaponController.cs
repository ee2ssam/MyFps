using UnityEngine;
using UnityEngine.Audio;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 조준점 데이터 정의
    /// 이미지, 크기, 컬러 
    /// </summary>
    [System.Serializable]
    public struct CrossHairData
    {
        public Sprite CrossHairSprite;
        public float CrossHairSize;
        public Color CrossHairColor;
    }

    /// <summary>
    /// 무기별 슛 타입 정의
    /// </summary>
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
        Sniper,
        //..
    }

    /// <summary>
    /// 총기류 무기를 관리하는 클래스
    /// </summary>
    [RequireComponent (typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //무기 활성화, 비활성
        public GameObject weaponRoot;

        public GameObject Owner { get; set; }               //무기 주인
        public GameObject SourcePrefab { get; set; }        //무기를 생성한 프리팹
        public bool IsWeaponActive { get; private set; }    //무기 활성화 여부

        //슛팅 오디오
        private AudioSource shootAudioSource;
        public AudioClip switchWeaponSfx;           //무기 교체 효과음

        //크로스헤어
        public CrossHairData crossHairDefault;          //기본(평상시)
        public CrossHairData crossHairTargetInSight;    //적 포착시(타겟팅)

        //조준
        [Range(0, 1)] public float aimZoomratio = 1f;   //조준시 줌 비율
        public Vector3 aimOffset = Vector3.zero;        //조준 위치 이동시 무기별 위치 조정값

        //슛팅
        [SerializeField] private WeaponShootType shootType; //슛팅 타입

        [SerializeField] private float maxAmmo = 8f;        //최대 탄환 갯수
        private float currentAmmo;                          //현재 탄환 갯수
        public float CurrentAmmoRatio => currentAmmo / maxAmmo;  //ammo 게이지 Ratio

        [SerializeField] private float delayBetweenShots = 0.5f;    //연사 방지, 초당 발사 갯수 
        private float lastTimeShot;

        //슛 연출
        public Transform weaponMuzzle;          //총구, 파이어포인트
        public GameObject muzzleFlashPrefab;    //총구 발사 이펙트 프리팹
        public AudioClip shootSfx;              //슛 사운드 클립(소스)

        //슛 반동 Recoil
        public float recoilForce = 0.5f;

        //발사체 Projectile
        public Vector3 MuzzleWorldVelocity { get; private set; }    //총구 이동 속도
        private Vector3 lastMuzzlePosition;
        public float CurrentCharge { get; private set; }            //충전 량

        public ProjectileBase ProjectilePrefab;     //발사체 프리팹
        public int bulletsPerShot = 1;              //한번 발사할때 마다 생성되는 발사체의 갯수
        public float bulletSpreadAngle = 0f;        //발사각
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

            //이번 프레임의 총구 이동 속도는
            if (Time.deltaTime > 0)
            {   
                MuzzleWorldVelocity = (weaponMuzzle.position - lastMuzzlePosition) / Time.deltaTime;
                //이번 프레임의 위치 저장
                lastMuzzlePosition = weaponMuzzle.position;
            }
        }
        #endregion

        #region Custom Method
        //무기 활성화, 비활성화
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            if(show == true && switchWeaponSfx != null)
            {
                //무기 교체 효과음 플레이
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }
            IsWeaponActive = show;
        }

        //인풋에 따른 발사 처리
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
                    if (inputHeld == true)
                    {
                        return TryShoot();
                    }
                    break;

                case WeaponShootType.Charge:
                    break;

                case WeaponShootType.Sniper:
                    break;
            }

            return false;
        }

        //발사 처리
        private bool TryShoot()
        {
            //ammo 체크, 연사방지 체크
            if(currentAmmo >= 1f && lastTimeShot + delayBetweenShots < Time.time)
            {
                currentAmmo -= 1f;
                Debug.Log($"currentAmmo: {currentAmmo}");

                HandleShoot();

                return true;
            }

            return false;
        }

        //슛 연출 처리
        private void HandleShoot()
        {
            //발사체 생성
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
                ProjectileBase projectileInstance = Instantiate(ProjectilePrefab, weaponMuzzle.position,
                    Quaternion.LookRotation(shotDirection));
                projectileInstance.Shoot(this);
            }

            //효과(vfx, sfx)
            if(muzzleFlashPrefab)
            {
                GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab,
                    weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
                Destroy(muzzleFlashInstance, 2f);
            }
            if(shootSfx)
            {
                shootAudioSource.PlayOneShot(shootSfx);
            }

            lastTimeShot = Time.time;
        }

        //발사각 설정
        private Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            float spreadAngleRation = bulletSpreadAngle / 180f;            
            return Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
                spreadAngleRation);
        }
        #endregion
    }
}