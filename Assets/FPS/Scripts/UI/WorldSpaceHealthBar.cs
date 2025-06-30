using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    //캐릭터의 머리위에 있는 HealthBar를 관리하는 클래스
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        #region Variables
        public Health health;

        public Image helthBarImage;         //게이지바 이미지
        public Transform healthBarPivot;    //healthBar UI를 관리하는 오브젝트

        //HP가 full이면 게이지바가 보이지 않도록 한다
        [SerializeField]
        private bool hideFullHealthBar = true;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //게이지 구현
            helthBarImage.fillAmount = health.GetRatio();

            //게이지바가 항상 플레이어를 바라보도록 한다
            healthBarPivot.LookAt(Camera.main.transform.position);

            //HP가 full이면 게이지바가 보이지 않도록 한다 full이 아니면 보인다
            if(hideFullHealthBar)
            {
                healthBarPivot.gameObject.SetActive(helthBarImage.fillAmount != 1f);
            }
        }
        #endregion
    }
}
