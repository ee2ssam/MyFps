using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 무기(Ammo) UI를 관리하는 클래스
    /// </summary>
    public class AmmoCounter : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager weaponManager;
        
        private WeaponController weapon;                //UI와 매칭되는 무기
        private int weaponCounterIndex;                 //무기(Ammo) UI의 인덱스

        //UI
        public TextMeshProUGUI weaponIndexText;
        public Image ammoFillImage;

        public float ammoFillSharpness = 10f;           //게이지 채울때 UI 연출 Lerp 계수
        public float weaponSwitchSharpness = 10f;       //무기 교체시 UI 연출 Lerp 계수

        public CanvasGroup canvasGroup;
        [Range(0,1)] public float unSelectedOpacity = 0.5f; //액티브 무기가 아니면 투명 처리
        private Vector3 unSelectedScale = Vector3.one * 0.8f; //액티브 무기가 아니면 크기 80%

        //게이지바 이미지 컬러 변경 연출
        public FillBarColorChange fillBarColorChange;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //ammo 게이지 그리기
            float currentFillRate = weapon.CurrentAmmoRatio;
            ammoFillImage.fillAmount = Mathf.Lerp(ammoFillImage.fillAmount, currentFillRate,
                ammoFillSharpness * Time.deltaTime);

            //액티브 무기 판정
            bool isActiveWeapon = (weapon == weaponManager.GetActiveWeapon());
            float currentOpacity = isActiveWeapon ? 1f : unSelectedOpacity;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOpacity,
                weaponSwitchSharpness * Time.deltaTime);
            Vector3 currentScale = isActiveWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale,
                weaponSwitchSharpness * Time.deltaTime);

            //게이지 컬러 변경 업데이트
            fillBarColorChange.UpdateVisual(currentFillRate);
        }
        #endregion

        #region Custom Method
        //초기화
        public void Initialize(WeaponController _weapon, int weaponIndex)
        {
            //참조
            weaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();

            weapon = _weapon;
            weaponCounterIndex = weaponIndex;

            //UI
            weaponIndexText.text = (weaponCounterIndex + 1).ToString();

            //게이지 컬러 변경 초기화
            fillBarColorChange.Initialize(1.0f, 0.1f);
        }
        #endregion
    }
}