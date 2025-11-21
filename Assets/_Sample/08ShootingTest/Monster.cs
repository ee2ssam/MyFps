using MyFps;
using UnityEngine;

namespace MySample2
{
    /// <summary>
    /// 몬스터의 기본(부모) 클래스
    /// </summary>
    public class Monster : MonoBehaviour
    {
        #region Variables
        //체력
        private float health;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;
        #endregion

        #region Unity Event Method
        protected virtual void Start()
        {
            //초기화
            health = maxHealth;
        }
        #endregion

        #region Custom Method
        //데미지 주기
        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log($"{transform.name} Health : {health}");

            //죽음 체크 - 두번 죽이지 마라
            if (health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽음 처리
        private void Die()
        {
            isDeath = true;
            
            //킬
            Destroy(gameObject);
        }
        #endregion
    }
}