using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전값에 따라 발사체 속성값 설정하는 클래스
    /// </summary>
    public class ProjectileChargeParameters : MonoBehaviour
    {
        #region Variables
        //참조
        ProjectileBase projectileBase;

        //설정값
        public MinMaxFloat speed;
        public MinMaxFloat gravityDown;
        public MinMaxFloat radius;
        public MinMaxFloat damage;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = GetComponent<ProjectileBase>();
            projectileBase.onShoot += OnShoot;
        }
        #endregion

        #region Custom Method
        //발사 순간 발사체의 속성갑 설정
        private void OnShoot()
        {
            ProjectileStandard projectileStandard = GetComponent<ProjectileStandard>();

            projectileStandard.speed = speed.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.gravityDown = gravityDown.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.radius = radius.GetValueFromRatio(projectileBase.InitialCharge);
            projectileStandard.damage = damage.GetValueFromRatio(projectileBase.InitialCharge);
        }
        #endregion
    }
}