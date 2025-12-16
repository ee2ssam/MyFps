using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 체력을 관리하는 클래스
    /// </summary>
    public class Health : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float maxHealth = 100f;
        private bool isDeath = false;

        //체력 위험 경계 비율
        [SerializeField]
        private float criticalHealthRatio = 0.3f;

        //이벤트 함수
        public UnityAction<float> onHeal;   //힐 성공시 등록된 함수 실행
        public UnityAction<float, GameObject> onDamaged;    //데미지 입었을때 등록된 함수 실행
        public UnityAction onDeath;         //죽었을때 등록된 함수 실행
        #endregion

        #region Property
        public float CurrentHealth { get; private set; }

        //무적
        public bool Invincible { get; set; }

        //힐 아이템을 먹을수 있는지 체크
        public bool CanPickup => CurrentHealth < maxHealth;
        //UI : health 게이지 량
        public float HealthRatio => CurrentHealth / maxHealth;
        //위험 경고
        public bool IsCritical => HealthRatio <= criticalHealthRatio;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            CurrentHealth = maxHealth;
        }
        #endregion

        #region Custom Method
        public bool Heal(float amount)
        {
            float beforeHealth = CurrentHealth; //힐 계산전 체력 값
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            //realHeal 구하기
            float realHeal = CurrentHealth - beforeHealth;
            if(realHeal > 0f)
            {
                //힐 효과 구현
                onHeal?.Invoke(amount);

                return true;
            }

            return false;
        }

        //damage: 데미지 량, damageSource: 데미지를 주는 주체
        public void TakeDamage(float damage, GameObject damageSource)
        {
            //무적 체크
            if(Invincible)
                return;

            float beforeHeath = CurrentHealth;  //데미지 계산전의 체력 값
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

            //real Damage 구하기
            float realDamage = beforeHeath - CurrentHealth;
            if(realDamage > 0f)
            {
                //데미지 효과 구현
                onDamaged?.Invoke(damage, damageSource);
            }

            //죽음 처리
            HandleDeath();
        }

        //죽음 처리
        private void HandleDeath()
        {
            //죽음 체크
            if (isDeath == true)
                return;

            if (CurrentHealth <= 0f)
            {
                isDeath = true;

                //죽음 효과 구현
                onDeath?.Invoke();
            }
        }
        #endregion
    }
}