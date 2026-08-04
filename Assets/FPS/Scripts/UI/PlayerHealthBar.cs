using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    /// HUD에 있는 플레이어 HealthBar를 관리하는 클래스
    /// HealthBar 게이지 관리
    /// 플레이어의 health는 Find Object (PlayerCharacterController)로 가져와서 참조    /// 
    /// </summary>
    public class PlayerHealthBar : MonoBehaviour
    {
        #region Variables
        //참조
        private Health playerHealth;
        public Image healthFillImage;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            PlayerCharacterController playerCharacterController 
                = GameObject.FindAnyObjectByType<PlayerCharacterController>();
            playerHealth = playerCharacterController.GetComponent<Health>();
        }

        private void Update()
        {
            healthFillImage.fillAmount = playerHealth.HealthRatio;
        }
        #endregion

    }
}
