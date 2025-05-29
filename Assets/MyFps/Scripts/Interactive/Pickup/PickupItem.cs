using UnityEngine;

namespace MyFps
{
    //맵에 떨어진 아이템 부딪혀서 먹기
    public class PickupItem : MonoBehaviour
    {
        #region Variables
        //아이템 연출
        [SerializeField]
        private float rotateSpeed = 5f;             //회전

        [SerializeField]
        private float verticalBobFrequency = 1f;    //위 아래 이동 속도
        [SerializeField]
        private float bobingAmount = 1f;            //위 아래 진폭 크기

        private Vector3 startPosition;                //아이템 초기 위치값
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            startPosition = transform.position;
        }

        private void Update()
        {
            //위 아래 이동
            float bobingAnimationPhase = Mathf.Sin(Time.time * verticalBobFrequency) * bobingAmount;
            transform.position = startPosition + Vector3.up * bobingAnimationPhase;

            //아이템 회전
            transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
        }

        //충돌체크
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                if(OnPickup())
                {
                    //아이템 오브젝트 킬
                    Destroy(gameObject);
                }
            }
        }
        #endregion

        #region Custom Method
        //조건을 체크하여 아이템을 먹으면 true, 못 먹으면 false
        protected virtual bool OnPickup()
        {
            //아이템을 먹을수 있는지 체크
            //아이템 먹었을때의 효과 구현

            return true;
        }
        #endregion
    }
}

/*
2. 아이템 움직임 구현
- 아이템이 360도 회전
- 위아래로 왔다 갔다 흔들림 구현 (Mathf : Sine 곡선 활용)
*/