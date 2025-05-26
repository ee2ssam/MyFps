using UnityEngine;
using UnityEngine.Events;

namespace MyFps
{
    //로봇의 체력을 관리하는 클래스
    public class RobotHealth : MonoBehaviour, IDamageable
    {
        #region Variables        
        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        //킬 딜레이
        [SerializeField]
        private float destroyDelay = 6f;

        private bool isDeath = false;

        //죽음시 호출되는 이벤트 함수 선언
        public UnityAction OnDie;
        #endregion

        #region Property
        public bool IsDeath => isDeath;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            currentHealth = maxHealth;
        }
        #endregion

        #region Custom Method
        //데미지 입기
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            //Debug.Log($"robot currentHealth: {currentHealth}");

            //데미지 연출 (Sfx, Vfx)

            if (currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽기
        private void Die()
        {
            isDeath = true;

            //죽음시 등록되는 함수 호출, 보상처리..
            OnDie?.Invoke();

            //킬
            Destroy(gameObject, destroyDelay);
        }
        #endregion
    }
}
