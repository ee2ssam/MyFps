using UnityEngine;
using System.Collections;

namespace MyFps
{
    //�÷��̾��� ü���� �����ϴ� Ŭ����
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        #region Variables
        //ü��
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //������ ȿ��
        public GameObject damageFlash;

        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

        //���� ó��
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "GameOver";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //�ʱ�ȭ
            currentHealth = maxHealth;
        }
        #endregion

        #region Custom Method
        //�÷��̾� ������
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"player currentHealth: {currentHealth}");

            //������ ���� Sfx, Vfx
            StartCoroutine(DamageEffect());

            if (currentHealth <= 0 && isDeath == false)
            {
                Die();
            }
        }

        //- ȭ����ü ������ �÷��� ȿ��
        //- ������ ���� 3���� 1 ���� �߻�
        IEnumerator DamageEffect()
        {
            //vfx
            damageFlash.SetActive(true);

            //sfx
            int randNum = Random.Range(1, 4);
            if (randNum == 1)
            {
                hurt01.Play();
            }
            else if (randNum == 2)
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

        private void Die()
        {
            isDeath = true;

            //����ó��
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
