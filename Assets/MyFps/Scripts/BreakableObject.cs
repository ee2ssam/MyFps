using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 데미지를 입으면 깨지는 오브젝트
    /// 깨지는 연출 : Fake오브젝트가 없어지 break오브젝트 활성화
    /// </summary>
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        #region Variables
        //깨지지 않는 오브젝트 체크
        [SerializeField]
        private bool unBreakable = false;

        private float health;
        [SerializeField]
        private float maxHealth = 1f;
        //죽음 체크, 깨짐 체크
        private bool isDeath = false;

        public GameObject fakeObject;       //온전한 오브젝트   
        public GameObject breakObject;      //조각 모음 오브젝트
        public GameObject activateObject;   //부서지는 연출 오브젝트

        private BoxCollider collider;       //데미지 입는 충돌체

        //리워드
        public GameObject rewardItemPrefab;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            collider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            //초기화
            health = maxHealth;
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {
            //깨지지 않는 오브젝트 체크
            if (unBreakable)
                return;

            health -= damage;
            
            if(health <= 0f && isDeath == false)
            {
                Die();
            }
        }

        //
        private void Die()
        {   
            isDeath = true;

            StartCoroutine(Break());

        }

        IEnumerator Break()
        {
            //깨짐 처리
            collider.enabled = false;

            fakeObject.SetActive(false);
            breakObject.SetActive(true);

            yield return new WaitForSeconds(0.2f);
            if (activateObject != null)
            {
                activateObject.SetActive(false);
            }
            //사운드
            AudioManager.Instance.Play("PotterySmash");

            yield return new WaitForSeconds(0.5f);
            //리워드 처리
            //필드에 아이템 떨구기
            if (rewardItemPrefab != null)
            {
                Instantiate(rewardItemPrefab, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
            
        }
        #endregion
    }
}
