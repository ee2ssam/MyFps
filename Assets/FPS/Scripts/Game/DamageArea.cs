using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    //지정한 범위 안에 있는 Damageable 콜라이더 오브젝트에게 거리에 따라 데미지 주기
    public class DamageArea : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private float areaOfEffectDistance = 5f;    //폭발(데미지) 영향을 받는 거리
        [SerializeField]
        private AnimationCurve damageRatioOverDistance; //커브 곡선에 따라 데미지량 계산
        #endregion

        #region Custom Method
        public void InflictDamageArea(float damage, Vector3 center, LayerMask layers,
            QueryTriggerInteraction interaction, GameObject owner)
        {
            //범위 안에 있는 적(Health)들 수집
            Dictionary<Health, Damageable> uniqueDamagedHealth = new Dictionary<Health, Damageable>();

            //폭발 범위 안에 있는 모든 충돌체 가져오기
            Collider[] effectedColliders = Physics.OverlapSphere(center, areaOfEffectDistance, layers, interaction);
            foreach (var collider in effectedColliders)
            {
                //damageable 있는 콜라이더 찾아 health를 key로 uniqueDamagedHealth에 등록
                Damageable damageable = collider.GetComponent<Damageable>();
                if(damageable)
                {
                    Health health = damageable.GetComponentInParent<Health>();
                    //중복 체크
                    if(health && uniqueDamagedHealth.ContainsKey(health) == false)
                    {
                        uniqueDamagedHealth.Add(health, damageable);
                    }
                }

                //uniqueDamagedHealth에 있는 damageable에게만 데미지 주기
                foreach(var uniqueDamageable in uniqueDamagedHealth.Values)
                {
                    //폭발지점과의 거리 구하기
                    float distance = Vector3.Distance(uniqueDamageable.transform.position, center);
                    //거리에 따른 커브 곡선 데미지 구하기
                    float curveDamage = damage * damageRatioOverDistance.Evaluate(distance / areaOfEffectDistance);
                    //Debug.Log($"curveDamage: {curveDamage}");

                    uniqueDamageable.InflictDamage(curveDamage, true, owner);
                }

            }
        }
        #endregion
    }
}