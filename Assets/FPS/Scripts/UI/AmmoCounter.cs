using UnityEngine;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine.UI;
using TMPro;

namespace Unity.FPS.UI
{
    //무기의 Ammo Counter UI를 관리하는 클래스
    public class AmmoCounter : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager weaponManager;
        private WeaponController weaponController;

        private int weaponCounterIndex;             //Ammo Counter UI 인덱스 번호

        //UI
        public TextMeshProUGUI weaponIndexText;
        public Image ammoFillIamge;

        public CanvasGroup canvasGroup;             //UI 투명도
        [SerializeField]
        [Range(0, 1)]
        private float unSelectedOpacity = 0.5f;     //선택되지 않은 UI 투명값
        private Vector3 unSelectedScale = Vector3.one * 0.8f; //선택되지 않은 UI 크기 (80%)

        [SerializeField]
        private float ammoFillSharpness = 10f;      //ammo UI 게이지바 충전 속도(Lerp 계수)
        [SerializeField]
        private float weaponSwitchSharpness = 10f;  //무기 변경시 UI 투명도, 크기 변경 속도(Lerp 계수)
        #endregion

        #region Property
        public int WeaponCounterIndex => weaponCounterIndex;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            float currentFillRate = weaponController.CurrentAmmoRate;

            //게이지바
            ammoFillIamge.fillAmount = Mathf.Lerp(ammoFillIamge.fillAmount, currentFillRate,
                Time.deltaTime * ammoFillSharpness);

            //액티브 무기와 아닌 무기 구분
            bool isActiveWeapon = (weaponController == weaponManager.GetActiveWeapon());
            //UI 투명도 - 무기 교체시 연출 구현
            float currentOparcity = isActiveWeapon ? 1f : unSelectedOpacity;            
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, currentOparcity,
                Time.deltaTime * weaponSwitchSharpness);
            //UI 크기 - 무기 교체시 연출 구현
            Vector3 currentScale = isActiveWeapon ? Vector3.one : unSelectedScale;
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale,
                Time.deltaTime * weaponSwitchSharpness);
        }
        #endregion

        #region Custom Method
        //Ammo Counter UI 초기화
        public void Initialize(WeaponController weapon, int weaponIndex)
        {
            weaponController = weapon;
            weaponCounterIndex = weaponIndex;

            //weaponManager 가져오기
            weaponManager = FindFirstObjectByType<PlayerWeaponManager>();

            //UI 초기화
            weaponIndexText.text = (weaponIndex + 1).ToString();
        }
        #endregion
    }
}
