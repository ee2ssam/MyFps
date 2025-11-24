using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 플레이어 Health 관리 클래스
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        //체력
        private float health;   //현재 체력
        [SerializeField]
        private float maxHealth = 20;   //최대 체력

        private bool isDeath = false;   //죽음 체크

        //데미지
        public GameObject damageFlash;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            health = maxHealth;
        }
        #endregion


        #region Custom Method
        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log($"player Health : {health}");

            //데미지 이펙트
            StartCoroutine(DamageEffect());

            if (health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Goto GameOver");
        }

        IEnumerator DamageEffect()
        {
            //화면전체 빨간색 플래쉬 효과
            damageFlash.SetActive(true);

            //데미지 사운드 3개중 1 랜덤 발생

            yield return new WaitForSeconds(1.0f);

            damageFlash.SetActive(false);
        }
        #endregion
    }
}