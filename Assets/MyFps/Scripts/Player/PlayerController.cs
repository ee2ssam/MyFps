using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        #region Variables
        public SceneFader fader;
        [SerializeField] private string loadToScene = "GameOver";

        //체력
        [SerializeField] private float maxHealth = 20;
        private float currentHealth;

        private bool isDeath = false;

        //데미지 효과
        public GameObject damageFlash;      //데미지 플래쉬 효과
        public AudioSource hurt01;          //데미지 사운드1
        public AudioSource hurt02;          //데미지 사운드2
        public AudioSource hurt03;          //데미지 사운드2

        //무기
        public GameObject realPistol;
        #endregion

        private void Start()
        {
            //초기화
            currentHealth = maxHealth;

            //무기획득
            if(PlayerStats.Instance.HasGun)
            {
                realPistol.SetActive(true);
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"Player Health: {currentHealth}");

            //데미지 효과
            StartCoroutine(DamageEffect());

            if (currentHealth <= 0 && !isDeath)
            {
                Die();
            }
        }

        void Die()
        {
            //Debug.Log("GameOver!!!");
            fader.FadeTo(loadToScene);
        }

        IEnumerator DamageEffect()
        {
            damageFlash.SetActive(true);
            CinemachineShake.Instance.ShakeCamera(1f, 1f);

            int randNumber = Random.Range(1, 4);
            if(randNumber == 1)
            {
                hurt01.Play();
            }
            else if (randNumber == 2)
            {
                hurt02.Play();
            }
            else
            {
                hurt03.Play();
            }

            yield return new WaitForSeconds(1f);

            damageFlash.SetActive(false);                
        }

    }
}