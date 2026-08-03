using UnityEngine;
using Unity.FPS.Utility;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전 발사체의 충전량에 따른 속성값 설정하기
    /// </summary>
    public class ProjectileChargeParameters : MonoBehaviour
    {
        #region Variables
        //충전량에 따른 설정할 속성값
        public MinMaxFloat damage;          //데미지
        public MinMaxFloat speed;           //이동 속도
        public MinMaxFloat gravityDown;     //중력 값
        public MinMaxFloat radius;          //반경

        //참조
        private ProjectileBase projectileBase;
        #endregion

        private void OnEnable()
        {
            //참조
            projectileBase = GetComponent<ProjectileBase>();
            //이벤트 함수 등록
            projectileBase.onShoot += OnShoot;
        }

        private void OnShoot()
        {
            ProjectileStandard projectileStandard = GetComponent<ProjectileStandard>();

            projectileStandard.damage = damage.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.speed = speed.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.gravityDown = gravityDown.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.radius = radius.GetValueFromRatio(projectileBase.InitialCharge);
        }
    }
}