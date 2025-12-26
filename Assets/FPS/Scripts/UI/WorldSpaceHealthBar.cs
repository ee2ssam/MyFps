using UnityEngine;
using UnityEngine.UI;
using Unity.FPS.Game;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 캐릭터의 머리위에 있는 Healthbar 관리
    /// </summary>
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Health health;

        //UI
        public Image healthBarImage;        //fill 이미지
        public Transform healthBarPivot;    //플레이어(메인카메라)를 바라보도록 한다

        private bool hideFullHeathBar = true;   //health Full이면 healthBar를 보이지 않게 한다
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Haeath gauge bar
            healthBarImage.fillAmount = health.HealthRatio;

            //플레이어(메인카메라)를 바라보도록 한다
            healthBarPivot.LookAt(Camera.main.transform.position);

            //health Full이면 healthBar를 보이지 않게 한다
            if (hideFullHeathBar)
            {
                healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1f);
            }
        }
        #endregion
    }
}