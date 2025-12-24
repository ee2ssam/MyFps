using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전에 따른 발사체 효과 관리 클래스
    /// </summary>
    public class ChargedProjectileEffectHandler : MonoBehaviour
    {
        #region Variables
        public GameObject chargingObejct;       //발사체

        public MinMaxVector3 scale;             //발사체 크기 조절
        public MinMaxColor color;               //발사체 컬러 조절

        private ProjectileBase projectileBase;
        #endregion

        #region Unity Evnet Method
        private void OnEnable()
        {
            //참조
            projectileBase = GetComponent<ProjectileBase>();
            //이벤트 함수 등록
            projectileBase.onShoot += OnShoot;
        }
        #endregion

        #region Custom Method
        //발사 순간 이펙트 값 설정
        private void OnShoot()
        {
            chargingObejct.transform.localScale = scale.GetValueFromRatio(projectileBase.InitialCharge);
        }
        #endregion
    }
}