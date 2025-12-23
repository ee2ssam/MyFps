using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using TMPro;

namespace Unity.FPS.UI
{
    /// <summary>
    /// Ammo UI를 관리하는 클래스
    /// </summary>
    public class AmmoCounter : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager weaponManager;
        public FillBarColorChange fillBarColorChange;

        private WeaponController weapon;        //해당 무기
        public int weaponCounterIndex;          //해당 무기 인덱스

        //UI
        public TextMeshProUGUI weaponIndexText;
        public Image ammoFillImage;

        //연출
        [SerializeField] private float ammoFillSharpness = 10f; //fillAmount Lerp 속도 계수

        public CanvasGroup canvasGroup;
        [Range(0f, 1f)]
        [SerializeField] private float unSelectedOpacity = 0.5f;
        private Vector3 unSelectedScale = Vector3.one * 0.8f;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Ammo 게이지바 연출
            float currentFillRate = weapon.CurrentAmmoRatio;
            ammoFillImage.fillAmount = Mathf.Lerp(ammoFillImage.fillAmount, currentFillRate,
                Time.deltaTime * ammoFillSharpness);

            //액티브 무기와 다른 무기 구분: 투명도, 크기로 구분
            bool isActvieWeapon = (weapon == weaponManager.GetAcitveWeapon());
            float currentOpacity = isActvieWeapon ? 1.0f : unSelectedOpacity;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOpacity,
                Time.deltaTime * ammoFillSharpness);
            Vector3 currentScale = isActvieWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale,
                Time.deltaTime * ammoFillSharpness);

            //CurrentAmmoRatio에 의한 게이지바 컬러 연출
            fillBarColorChange.UpdateVisual(currentFillRate);
        }
        #endregion

        #region Custom Method
        //무기 추가하여 UI 생성할때 호출되는 함수, UI 초기화
        public void Initialize(WeaponController _weapon, int weaponIndex)
        {
            //참조
            weaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();

            weapon = _weapon;
            weaponCounterIndex = weaponIndex;

            weaponIndexText.text = (weaponIndex + 1).ToString();

            //게이지바 컬러 초기화
            fillBarColorChange.Initialize(1f, 0.1f);
        }
        #endregion
    }
}