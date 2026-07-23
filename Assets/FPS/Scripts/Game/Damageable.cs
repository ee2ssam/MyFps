using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 충돌체에 부착되어 데미지를 처리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Health health;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            health = GetComponent<Health>();
            if(health == null)
            {
                health = GetComponentInParent<Health>();
            }
        }
        #endregion

        #region Custom Method
        //데미지 처리, damage:데미지량, isExplosionDamage:폭발 데미지 여부, damageSource: 데미지를 주는 주체
        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {

        }
        #endregion
    }
}