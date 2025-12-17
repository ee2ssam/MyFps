using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Gameplay;

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
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            weaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();
        }
        #endregion
    }
}
