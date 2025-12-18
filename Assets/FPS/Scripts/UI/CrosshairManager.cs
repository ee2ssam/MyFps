using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Gameplay;
using Unity.FPS.Game;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 크로스 헤어를 관리하는 클래스, 교체, 상태변환
    /// </summary>
    public class CrosshairManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager weaponManager;

        public Image crosshairImage;    //크로스헤어 UI

        public Sprite nullCrosshairSprite;  //액티브 무기가 없을 경우 보이는 크로스헤어

        //무기 교체 연출
        [SerializeField]
        private float crosshairUpdateSharpness = 5f;    //Lerp 계수 빠르기 설정값

        private RectTransform crosshairRectTransform;   //크로스헤어 트랜스폼

        [SerializeField]
        private CrosshairData defaultCrosshair;         //평상시의 크로스헤어
        [SerializeField]
        private CrosshairData targetInCrosshair;        //타겟이 잡혔을때의 크로스헤어

        [SerializeField]
        private CrosshairData currentCrosshair;         //현재 화면에 보이는 크로스헤어

        private bool wasPointingAtEnemy;                //적 포착 상태 체크 변수
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            weaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();
            crosshairRectTransform = crosshairImage.GetComponent<RectTransform>();
        }

        private void Start()
        {
            //현재 액티브무기의 크로스헤어로 교체
            OnWeaponChanged(weaponManager.GetAcitveWeapon());

            //이벤트 함수 등록
            weaponManager.onSwitchToWeapon += OnWeaponChanged;
        }

        private void Update()
        {
            //크로스헤어 업데이트
            UpdateCrosshairPointAtEnemy(false);

            //적 포착 상태 변화 체크
            wasPointingAtEnemy = weaponManager.IsPointigAtEnemy;
        }
        #endregion

        #region Custom Method
        //크로스헤어 업데이트
        private void UpdateCrosshairPointAtEnemy(bool force)
        {
            //크로스헤어 데이터 체크
            if (defaultCrosshair.CrossHairSprite == null)
                return;

            //적 포착 상태 변경 시점 체크
            if((force || wasPointingAtEnemy == false) && weaponManager.IsPointigAtEnemy == true)
            {
                //적 포착을 시작하는 시점
                currentCrosshair = targetInCrosshair;
                crosshairImage.sprite = currentCrosshair.CrossHairSprite;
                if (force)
                {
                    crosshairRectTransform.sizeDelta = currentCrosshair.CorssHairSize * Vector2.one;
                }   
            }
            else if((force || wasPointingAtEnemy == true) && weaponManager.IsPointigAtEnemy == false)
            {
                //적 포착을 놓치는 시점
                currentCrosshair = defaultCrosshair;
                crosshairImage.sprite = currentCrosshair.CrossHairSprite;
                if(force)
                {
                    crosshairRectTransform.sizeDelta = currentCrosshair.CorssHairSize * Vector2.one;
                }   
            }

            crosshairImage.color = Color.Lerp(crosshairImage.color, currentCrosshair.CrossHairColor,
                crosshairUpdateSharpness * Time.deltaTime);
            crosshairRectTransform.sizeDelta = Mathf.Lerp(crosshairRectTransform.sizeDelta.x,
                currentCrosshair.CorssHairSize, crosshairUpdateSharpness * Time.deltaTime) * Vector2.one;
        }

        //무기 교체시 크로스헤어 데이터 바꾸기
        private void OnWeaponChanged(WeaponController newWeapon)
        {
            if(newWeapon != null)
            {
                crosshairImage.enabled = true;
                defaultCrosshair = newWeapon.defalutCrossHair;
                targetInCrosshair = newWeapon.targetInSightCrossHair;
            }
            else
            {
                if(nullCrosshairSprite)
                {
                    crosshairImage.sprite = nullCrosshairSprite;
                }
                else
                {
                    crosshairImage.enabled = false;
                }
            }

            //강제로 크로스헤어 데이터적용
            UpdateCrosshairPointAtEnemy(true);
        }
        #endregion
    }
}
