using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 일정 범위 안에 있는 모든 충돌체(damageable) 오브젝트에게 데미지 주기
    /// 하나의 Heath에게는 한번만 데미지 주기, 거리에 반비례해서 데미지량 주기
    /// </summary>
    public class DamageArea : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float areaOfEffectDistance = 5f;        //폭발지점으로부터 데미지 입는 반경
        [SerializeField]
        private AnimationCurve damageRatioOverDistance; //거리에 따른 데미지량 계산 커브
        #endregion

        #region Custom Method
        //폭발(범위 공격) 데미지 계산
        public void InflictDamageArea(float damage, Vector3 center, LayerMask layer, 
            QueryTriggerInteraction interaction, GameObject owner)
        {
            Dictionary<Health, Damageable> uniqueDamagedHealth = new Dictionary<Health, Damageable>();

            //범위 안에 있는 모든 콜라이더 가져오기
            Collider[] affectedColliders = Physics.OverlapSphere(center, areaOfEffectDistance,
                layer, interaction);
            foreach (Collider collider in affectedColliders)
            {
                Damageable damageable = collider.GetComponent<Damageable>();
                if(damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    if(health && uniqueDamagedHealth.ContainsKey(health) == false)
                    {
                        uniqueDamagedHealth.Add(health, damageable);
                    }
                }
            }

            //등록한 damageable만 데미지 주기
            foreach(var uniqueDamageable in uniqueDamagedHealth.Values)
            {
                //거리 계산에 따른 데미지 계산
                float distance = Vector3.Distance(center, uniqueDamageable.transform.position);
                float curveDamage = 
                    damage * damageRatioOverDistance.Evaluate(distance/ areaOfEffectDistance);
                uniqueDamageable.InflictDamage(curveDamage, true, owner);
            }
        }
        #endregion
    }
}