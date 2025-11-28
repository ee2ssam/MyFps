using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어가 충돌체에 (트리거 체크)부딪히면 아이템 지급하는 부모 추상 클래스    
    /// </summary>
    public abstract class Pickup : MonoBehaviour
    {
        #region abstract Method
        protected abstract bool OnPickup();
        #endregion

        #region Variables
        //흔들림 - 위아래
        [SerializeField] private float bobingAmount = 1f;    //흔들림 량
        [SerializeField] private float verticalBobFrequency = 1f;    //흔들림 속도

        private Vector3 startPosition;  //아이템의 처음 위치

        //회전
        [SerializeField] private float rotateSpeed = 360f;
        #endregion

        #region Unity Event Method
        protected void Start()
        {
            //초기화
            startPosition = transform.position;
        }

        protected void Update()
        {
            //흔들림 량
            float bobingAnimationPhase = (Mathf.Sin(Time.time * verticalBobFrequency)) * bobingAmount;
            transform.position = startPosition + Vector3.up * bobingAnimationPhase;

            //회전
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
        }

        protected void OnTriggerEnter(Collider other)
        {
            //충돌체(플레이어) 체크
            if(other.tag == "Player")
            {
                //아이템 지급
                if(OnPickup() == true)
                {
                    //아이템 킬
                    Destroy(gameObject);
                }
            }
        }
        #endregion

        #region Custom Method        
        #endregion
    }
}
