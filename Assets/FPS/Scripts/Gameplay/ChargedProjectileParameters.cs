using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    //충전량에 따라 발사체 속성값 결정
    public class ChargedProjectileParameters : MonoBehaviour
    {
        #region Variables
        public MinMaxFloat damage;
        public MinMaxFloat speed;
        public MinMaxFloat gravityDown;
        public MinMaxFloat radius;

        private ProjectileBase projectileBase;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            projectileBase = this.GetComponent<ProjectileBase>();
            projectileBase.OnShoot += OnShoot;
        }
        #endregion

        #region Custom Method
        private void OnShoot()
        {
            ProjectileStandard projectileStandard = this.GetComponent<ProjectileStandard>();

            projectileStandard.damage = damage.GetValueRatio(projectileBase.InitalCharge);
            projectileStandard.speed = speed.GetValueRatio(projectileBase.InitalCharge);
            projectileStandard.gravityDown = gravityDown.GetValueRatio(projectileBase.InitalCharge);
            projectileStandard.radius = radius.GetValueRatio(projectileBase.InitalCharge);
        }
        #endregion


    }
}
