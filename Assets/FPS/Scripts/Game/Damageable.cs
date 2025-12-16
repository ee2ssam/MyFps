using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 데미지를 입는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Health health;

        //데미지 계수
        [SerializeField]
        private float damageMultiplier = 1f;

        //자신에게 입히는 데미지 계수
        [SerializeField]
        private float sensibilityToSelfDamage = 0.5f;
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
        //damage: 데미지량, isExplosionDamage:폭발 데미지 여부, damageSource:데미지 주는 주체
        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            //health 체크
            if (health == null)
                return;

            var totalDamage = damage;

            //폭발 데미지 여부
            if (isExplosionDamage)
            {
                totalDamage *= 1f;
            }
            else
            {
                totalDamage *= damageMultiplier;
            }

            //셀프 데미지 여부
            if(health.gameObject == damageSource)
            {
                totalDamage *= sensibilityToSelfDamage;
            }

            //데미지 적용
            health.TakeDamage(totalDamage, damageSource);
        }
        #endregion
    }
}