using MyFps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    public class EnemyTest : MonoBehaviour, IDamageable
    {
        #region Variables
        //체력
        [SerializeField] private float maxHealth = 20;
        private float currentHealth;

        private bool isDeath = false;
        #endregion

        private void Start()
        {
            //초기화
            currentHealth = maxHealth;
            isDeath = false;            
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"currentHealth: {currentHealth}");

            //데미지 효과

            if (currentHealth <= 0 && !isDeath)
            {
                Die();
            }
        }

        void Die()
        {
            isDeath = true;

            //죽음처리
            //보상 - 경험치, 돈

            //효과

            Destroy(gameObject, 3f);
        }
    }
}