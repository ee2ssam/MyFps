using UnityEngine;

namespace MySample
{
    public class RigidBodyTest : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;

        //힘
        [SerializeField]
        private float power = 100f;
        #endregion

        private void Start()
        {
            //참조
            rb = this.GetComponent<Rigidbody>();

            //일회성으로  월드 앞 방향으로 100의 힘으로 오브젝트를 이동시킨다
            //rb.AddForce(Vector3.forward * power, ForceMode.Impulse);

            //일회성으로  로컬 앞 방향으로 100의 힘으로 오브젝트를 이동시킨다
            rb.AddForce(transform.forward * power, ForceMode.Impulse);
            //rb.AddRelativeForce(Vector3.forward * power, ForceMode.Impulse);
        }

        private void FixedUpdate()
        {
            //rb.AddForce(Vector3.forward, ForceMode.Force);
        }
    }
}

/*
ForceMode.Force (연속, 질량 고려)
- 바람,자리력 처럼 연속적으로 주어지는 힘

ForceMode.Acceleration (연속, 질량 무시)
- 중력, 질량에 상관없이 일정한 가속을 구현할때

ForceMode.Impulse (불연속(1회성), 질량 고려)
- 타격, 폭발 등 순간적으로 적용하는 힘

ForceMode.VelocityChange (불연속(1회성), 질량 무시)
- 순간적으로 지정한 속도의 변화를 줄때
*/