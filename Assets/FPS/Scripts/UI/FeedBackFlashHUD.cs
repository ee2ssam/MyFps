using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

namespace Unity.FPS.UI
{
    //데미지 입을때, 힐 할때 전체 화면 플래시 이펙트 효과
    //체력이 크리티컬 이하로 떨어지면 화면에 경고 이펙트 효과
    public class FeedBackFlashHUD : MonoBehaviour
    {
        #region Variables
        //참조
        private Health playerHealth;

        //플래시 효과
        public Image flashImage;                //플래시 효과 이미지
        public CanvasGroup flashCanvasGroup;    //이미지 알파값 조절

        public Color damageFlashColor;          //데미지 효과 컬러
        public Color healFlashColor;            //힐 효과 컬러

        [SerializeField]
        private float flashDuration = 1f;       //플래시 효과 지속 시간
        [SerializeField]
        private float flashMaxAlpha = 1f;       //플래시 효과시 알파 최대값

        private bool flashActive = false;       //플래시 효과 실행 여부
        private float lastTimeFlashStarted = Mathf.NegativeInfinity; //플래시 효과를 시작한 시간

        //체력 위험 경계 효과
        public Image vignetteImage;                 //vignette 효과 이미지
        public CanvasGroup vignetteCanvasGroup;     //알파값 조절

        public Color vignettColor;                  //vignette 효과 컬러

        [SerializeField]
        private float vignetteMaxAlpha = 1f;        //vignette 효과시 알파 최대값
        [SerializeField]
        private float vignetteFrequency = 5f;       //vignette 효과시 알파 변경 속도

        private bool vignetteActive = false;        //vignette 효과 실행 여부
        private bool wasCritical = false;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            PlayerCharacterController playerCharacterController
                = FindFirstObjectByType<PlayerCharacterController>();

            playerHealth = playerCharacterController.GetComponent<Health>();
        }

        private void Start()
        {
            //playerHealth 이벤트 함수 등록
            playerHealth.OnDamaged += OnDamaged;
            playerHealth.OnHeal += OnHealed;
        }

        private void Update()
        {
            //플래시 효과
            if(flashActive)
            {
                //0->1
                float normalizedTimeSinceDamaged = (Time.time - lastTimeFlashStarted) / flashDuration;

                if(normalizedTimeSinceDamaged < 1f)
                {
                    //플래시 효과 지속
                    //1 -> 0
                    float flashAmount = flashMaxAlpha * (1f - normalizedTimeSinceDamaged);
                    flashCanvasGroup.alpha = flashAmount;
                }
                else
                {
                    //플래시 효과 끝
                    flashActive = false;
                    //플래시 효과 이미지 오브젝트 비활성화
                    flashCanvasGroup.gameObject.SetActive(false);
                }
            }

            //vignette 효과
            if(vignetteActive)
            {
                float vignetteAlpha = vignetteMaxAlpha 
                    * (1 - (playerHealth.GetRatio() / playerHealth.CriticalHealthRatio));
                vignetteCanvasGroup.alpha = vignetteAlpha
                    * (Mathf.Sin(Time.time * vignetteFrequency)/2 + 0.5f);
            }

            if(wasCritical == false && playerHealth.IsCritical() == true)
            {
                //vignette 효과 시작
                vignetteActive = true;
                vignetteCanvasGroup.gameObject.SetActive(true);
                vignetteImage.color = vignettColor;
            }
            else if(wasCritical == true && playerHealth.IsCritical() == false)
            {
                //vignette 효과 끝
                vignetteActive = false;
                vignetteCanvasGroup.gameObject.SetActive(false);
            }

            //
            wasCritical = playerHealth.IsCritical();
        }
        #endregion

        #region Custom Method
        //플래시 효과 시작
        private void ResetFlash()
        {
            //플래시 효과 관련 변수 초기화
            flashActive = true;
            lastTimeFlashStarted = Time.time;
            flashCanvasGroup.alpha = 0f;
            //플래시 효과 이미지 오브젝트 활성화
            flashCanvasGroup.gameObject.SetActive(true);
        }

        //데미지 이벤트 함수에 등록할 함수
        private void OnDamaged(float damage, GameObject damageSource)
        {
            ResetFlash();
            flashImage.color = damageFlashColor;
        }

        //힐 이벤트 함수에 등록할 함수
        private void OnHealed(float healAmount)
        {
            ResetFlash();
            flashImage.color = healFlashColor;
        }
        #endregion
    }
}
