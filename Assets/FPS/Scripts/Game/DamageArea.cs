using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 일정 범위 안에 있는 모든 충돌체(damageable) 오브젝트에게 데미지 주기
    /// 폭발위치와의 거리에 반비례해서 데미지량 주기
    /// 하나의 Health에게는 한번만 데미지 주기
    /// </summary>
    public class DamageArea : MonoBehaviour
    {
        #region Variables
        //일정 범위, 폭발지점으로 부터 데미지를 입는 반경
        [SerializeField] private float areaOfEffectDistance = 5f;
        //거리에 따른 데미지량 계산하는 커브
        [SerializeField] private AnimationCurve damageRatioOverDistance;
        #endregion

        #region Custom Method
        //폭발(범위 공격) 데미지 계산
        public void InflictDamageArea(float damage, Vector3 center, LayerMask layer,
            QueryTriggerInteraction interaction, GameObject owner)
        {
            //하나의 Health에 Damageable 하나만 등록
            Dictionary<Health, Damageable> uniqueDamagedHealth = new Dictionary<Health, Damageable>();

            //범위 안에 있는 모든 충돌체(Damageable) 가져오기
            Collider[] affectedColliers = Physics.OverlapSphere(center, areaOfEffectDistance,
                layer, interaction);
            foreach (Collider collider in affectedColliers)
            {
                Damageable damageable  = collider.GetComponent<Damageable>();
                if(damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    //health 중복체크
                    if (health != null && uniqueDamagedHealth.ContainsKey(health) == false)
                    {
                        uniqueDamagedHealth.Add(health, damageable);
                    }
                }
            }

            //uniqueDamagedHealth에 등록되어 있는 damageable에게만 데미지 주기
            foreach (var uniqueDamageable in uniqueDamagedHealth.Values)
            {
                //거리에 따른 데미지 계산
                float distance = Vector3.Distance(center, uniqueDamageable.transform.position);
                float curveDamage = damage 
                    * damageRatioOverDistance.Evaluate(distance/areaOfEffectDistance);
                //Debug.Log($"CurveDamage: {curveDamage}");

                //damageable에게 데미지 주기
                uniqueDamageable.InflictDamage(curveDamage, true, owner);
            }

        }
        #endregion


    }
}