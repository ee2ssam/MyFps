using UnityEngine;
using UnityEngine.Events;

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

        //데미지 입을때 등록된 함수 호출
        public UnityAction onDamage;
        //죽었을때 호출되는 함수 호출
        public UnityAction onDie;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            health = PlayerStats.Instance.Health;
        }
        #endregion


        #region Custom Method
        public void TakeDamage(float damage)
        {
            health -= damage;
            //Debug.Log($"player Health : {health}");

            PlayerStats.Instance.SetHealth(health);

            //데미지 이벤트 함수에 등록된 함수 호출
            onDamage?.Invoke();

            if (health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        private void Die()
        {
            //죽음 체크
            isDeath = true;

            //죽음 이벤트 함수에 등록된 함수 호출
            onDie?.Invoke();
        }
        #endregion
    }
}