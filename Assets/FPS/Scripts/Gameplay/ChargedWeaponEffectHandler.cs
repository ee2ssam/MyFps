using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    //충전용 무기 이펙트 관리 클래스
    public class ChargedWeaponEffectHandler : MonoBehaviour
    {
        #region Variables
        public GameObject charingObejct;
        public GameObject spiningFrame;
        public GameObject diskOrbitParticlePrefab;

        public MinMaxVector scale;          //충전 오브젝트의 크기

        [SerializeField] private Vector3 offset;    //파티클 프리팹 생성 위치 조정값
        public Transform parentTransform;           //파티클 프리팹 생성시 부모 오브젝트
        public MinMaxFloat orbitY;                  //파티클 프리팹 속성
        public MinMaxVector radius;                 //파티클의 크기

        public MinMaxFloat spiningSpeed;            //회전 프레임 회전 속도

        //사운드 효과
        public AudioClip chargeSound;               //충전 효과음
        public AudioClip loopChareWeaponSfx;        //회전 프레임 효과음

        public float fadeLoopDuration = 0.5f;       //사운드 페이드 효과 시간
        //효과음 처리 여부 체크 (true:효과음 재생속도 효과 처리, false:사운드 페이드 효과 처리)
        public bool useProceduralPitchOnLoop = false;   

        [Range(1.0f, 5.0f)]
        public float maxProceduralPitchValue = 2.0f;    //루프 효과음 재생 속도 Max값

        private AudioSource audioSource;            //충전 효과음 플레이 오디오
        private AudioSource audioSourceLoop;        //회전 프레임 효과음 플레이 오디오

        private ParticleSystem diskOrbitParticle;   //회전 파티클
        private ParticleSystem.VelocityOverLifetimeModule VelocityOverLifetimeModule;

        private float chargeRaito;                      //충전량
        private float lastChargeTriggerTimesTemp;      //충전 시간 저장
        private float endChargeTime;                   //충전 마지막 시간

        //참조
        private WeaponController weaponController;
        #endregion

        #region Property
        public GameObject particleInstance { get; private set; }    //파티클 생성 오브젝트 객체
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //충전 audioSource 컴포넌트 추가 및 설정
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = chargeSound;
            audioSource.playOnAwake = false;

            //회전 audioSourceLoop 컴포넌트 추가 및 설정
            audioSourceLoop = gameObject.AddComponent<AudioSource>();
            audioSourceLoop.clip = loopChareWeaponSfx;
            audioSourceLoop.playOnAwake = false;
            audioSourceLoop.loop = true;
        }

        private void Update()
        {
            if (particleInstance == null)
            {
                SpawnParticleSystem();
            }

            //파티클 오브젝트 활성화 여부 - 런처가 ActvieWeapon일때 활성화
            diskOrbitParticle.gameObject.SetActive(weaponController.IsWeaponActive);

            //충전량 가져오기
            chargeRaito = weaponController.CurrentCharge;

            //발사체 크기
            charingObejct.transform.localScale = scale.GetValueRatio(chargeRaito);

            //회전 프레임 회전속도
            if(spiningFrame)
            {
                spiningFrame.transform.localRotation *= Quaternion.Euler(
                    0f,
                    spiningSpeed.GetValueRatio(chargeRaito) * Time.deltaTime,
                    0f);
            }

            //파티클
            VelocityOverLifetimeModule.orbitalY = orbitY.GetValueRatio(chargeRaito);
            diskOrbitParticle.transform.localScale = radius.GetValueRatio(chargeRaito);

            //사운드 페이드 효과
            if(chargeRaito > 0f)
            {
                if(audioSourceLoop.isPlaying == false &&
                    weaponController.lastChargeTriggerTimesTemp > lastChargeTriggerTimesTemp )
                {
                    lastChargeTriggerTimesTemp = weaponController.lastChargeTriggerTimesTemp;
                    if(useProceduralPitchOnLoop == false)
                    {
                        endChargeTime = Time.time + chargeSound.length;

                        //충천 효과음 플레이
                        audioSource.Play();
                    }

                    //회전 효과음 플레이
                    audioSourceLoop.Play();
                }

                if(useProceduralPitchOnLoop == false)
                {
                    //볼륨 페이드 효과
                    float volumeRatio = Mathf.Clamp01((endChargeTime - Time.time - fadeLoopDuration) / fadeLoopDuration);
                    audioSource.volume = volumeRatio;
                    audioSourceLoop.volume = 1 - volumeRatio;
                }
                else
                {
                    //재생 속도 효과
                    audioSourceLoop.pitch = Mathf.Lerp(1.0f, maxProceduralPitchValue, chargeRaito);
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
        //파티클 오브젝트 생성
        private void SpawnParticleSystem()
        {
            particleInstance = Instantiate(diskOrbitParticlePrefab, parentTransform != null ? parentTransform : transform);
            particleInstance.transform.localPosition += offset; //생성위치 조정

            //참조
            FindReference();
        }

        private void FindReference()
        {
            weaponController = this.GetComponent<WeaponController>();
            diskOrbitParticle = particleInstance.GetComponent<ParticleSystem>();
            VelocityOverLifetimeModule = diskOrbitParticle.velocityOverLifetime;
        }
        #endregion

    }
}
