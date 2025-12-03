using UnityEngine;
using UnityEngine.Events;

namespace MyFps
{
    /// <summary>
    /// 문(door) 열기/닫기
    /// </summary>
    public class Door : MonoBehaviour, ISwitchable
    {
        #region Variables
        //참조
        protected Animator animator;

        // true:문 열려 있는 상태, false : 문이 닫혀 있는 상태
        [SerializeField]
        protected bool isActive;

        public UnityAction OnActivate;
        public UnityAction OnDeactivate;

        //적 등록
        public GunMan[] enemies;

        //사운드

        //애니메이터 파라미터
        const string IsOpen = "IsOpen";
        #endregion

        #region Property
        public bool IsActive
        {
            get { return isActive; }
            set
            { 
                isActive = value;
                animator.SetBool(IsOpen, value);

                //사운드 플레이
            }
        }
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            //문 상태 열림/닫힘 설정
            if (isActive)
            {
                Activate();
            }
        }
        #endregion

        #region Custom Method
        public void Activate()
        {
            IsActive = true;

            //활성화시 등록된 함수 호출, 문열기
            OnActivate?.Invoke();

            foreach (var enemy in enemies)
            {
                enemy.OnActive();
            }
        }

        public void Deactivate()
        {
            IsActive = false;

            //비 활성화시 등록된 함수 호출, 문닫기
            OnDeactivate?.Invoke();

            foreach (var enemy in enemies)
            {
                enemy.GoBack();
            }
        }
        #endregion
    }
}