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
        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {

        }
        #endregion
    }
}