using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어의 체력을 관리하는 클래스
    /// IDamageable 상속 받는다
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        [SerializeField] private float maxHealth = 20f;
        private float currentHealth;

        private bool isDeath = false;
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
            Debug.Log($"{gameObject.name} currentHealth: {currentHealth}");

            //데미지 효과 처리(VFX, SFX)

            //죽음 체크
            if (currentHealth <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //죽기
        void Die()
        {
            isDeath = true;

            //죽음 처리 (VFX, SFX, 보상처리)

            //게임오버
            Debug.Log("GameOver!!!");
        }
        #endregion
    }
}