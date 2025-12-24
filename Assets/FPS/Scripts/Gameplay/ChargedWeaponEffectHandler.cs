using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Audio;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전 무기의 충전 이펙트를 관리하는 클래스
    /// </summary>
    public class ChargedWeaponEffectHandler : MonoBehaviour
    {
        #region Variables
        //참조
        private WeaponController weaponController;

        public GameObject charingObject;
        public GameObject spiningFrame;

        //vfx 연출
        public MinMaxVector3 scale;

        public GameObject diskOrbitParticlePrefab;
        [SerializeField] private Vector3 offset;    //위치 조정값
        public Transform parentTransform;

        public MinMaxFloat orbitY;
        public MinMaxVector3 radius;

        //참조
        private ParticleSystem diskOrbitParticle;
        private ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule;

        //프레임
        public MinMaxFloat spiningSpeed;

        //sfx 연출
        private AudioSource audioSource;        //충전 효과음 플레이
        private AudioSource audioSourceLoop;    //회전 효과음 플레이

        public AudioClip chargeSfx;             //충전 효과음 소스
        public AudioClip loopChargeWeaponSfx;   //회전 효과음 소스

        [SerializeField] private float fadeLoopDuration = 0.5f;             //사운드 페이드 연출
        [SerializeField] private bool useProceduralPitchOnLoop = false;     //재생속도 효과/페이드 효과 선택
        [Range(1f, 5f)]
        [SerializeField] private float maxProceduralPitchValue = 2.0f;      //최대 재생 속도

        private float lastChargTriggerTimestemp;                //충전 시작하는 시간 체크
        private float endChargeTime;                            //충전 사운드 플레이가 끝나는 시간

        private float chargeRatio;              //무기의 충전값
        #endregion

        #region Property
        public GameObject ParticleInstance {  get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //충전 효과음 플레이
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = chargeSfx;
            audioSource.playOnAwake = false;

            //회전 효과음 플레이
            audioSourceLoop = gameObject.AddComponent<AudioSource>();
            audioSourceLoop.clip = loopChargeWeaponSfx;
            audioSourceLoop.playOnAwake = false;
            audioSourceLoop.loop = true;
        }

        private void Update()
        {
            if(ParticleInstance == null)
            {
                SpawnParticleSystem();
            }
                        
            diskOrbitParticle.gameObject.SetActive(weaponController.IsWeaponActive);
            chargeRatio = weaponController.CurrentCharge;

            //VFX
            //발사체 오브젝트
            charingObject.transform.localScale = scale.GetValueFromRatio(chargeRatio);
            if (spiningFrame != null)
            {
                spiningFrame.transform.localRotation *= Quaternion.Euler(0f,
                    spiningSpeed.GetValueFromRatio(chargeRatio) * Time.deltaTime, 0f);
            }

            //파티클 오브젝트
            velocityOverLifetimeModule.orbitalY = orbitY.GetValueFromRatio(chargeRatio);
            diskOrbitParticle.transform.localScale = radius.GetValueFromRatio(chargeRatio);

            //SFX
            if(chargeRatio > 0)
            {
                if(audioSourceLoop.isPlaying == false &&
                    weaponController.lastChargeTriggerTimestamp > lastChargTriggerTimestemp)
                {
                    lastChargTriggerTimestemp = weaponController.lastChargeTriggerTimestamp;
                    if (useProceduralPitchOnLoop == false) //사운드 페이드 효과 사용
                    {
                        endChargeTime = Time.time + chargeSfx.length; 
                        audioSource.Play();
                    }
                    audioSourceLoop.Play();
                }

                if(useProceduralPitchOnLoop)
                {
                    audioSourceLoop.pitch = Mathf.Lerp(1.0f, maxProceduralPitchValue, chargeRatio);
                }
                else //사운드 페이드 효과 사용
                {
                    float volumeRatio = Mathf.Clamp01((endChargeTime - Time.time - fadeLoopDuration) / fadeLoopDuration);
                    audioSource.volume = volumeRatio;
                    audioSourceLoop.volume = 1f - volumeRatio;
                }
                
            }
            else
            {
                audioSource.Stop();
                audioSourceLoop.Stop();
            }
        }
        #endregion

        #region Custom Method
        public void SpawnParticleSystem()
        {
            ParticleInstance = Instantiate(diskOrbitParticlePrefab,
                parentTransform != null ? parentTransform : transform);
            ParticleInstance.transform.localPosition += offset;

            //참조
            FindReference();
        }

        private void FindReference()
        {
            diskOrbitParticle = ParticleInstance.GetComponent<ParticleSystem>();
            velocityOverLifetimeModule = diskOrbitParticle.velocityOverLifetime;

            weaponController = GetComponent<WeaponController>();
        }
        #endregion
    }
}