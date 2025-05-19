using UnityEngine;

namespace MyFps
{
    //플레이어와 정면에 있는 오브젝트(콜라이더) 와의 거리를 구하는 클래스
    //RayCast를 이용한다
    public class PlayerCasting : MonoBehaviour
    {
        #region Variables
        //타겟까지의 거리
        public static float distanceFromTarget;
        public float toTarget;      //인스펙터창 디버깅용
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            distanceFromTarget = Mathf.Infinity;
            toTarget = distanceFromTarget;
        }

        private void Update()
        {
            //레이를 쏘아 거리 구하기
            RaycastHit hit; //레이hit 정보를 저장하는 변수
            
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                distanceFromTarget = hit.distance;
                toTarget = distanceFromTarget;
            }

        }

        //레이 기즈모 그리기, 길이 100인 레이 기즈모를 그리기
        private void OnDrawGizmosSelected()
        {
            RaycastHit hit;
            float maxDistance = 100f;
            bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance);

            Gizmos.color = Color.red;
            if(isHit)
            {
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            }
            else
            {
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            }
        }
        #endregion
    }
}