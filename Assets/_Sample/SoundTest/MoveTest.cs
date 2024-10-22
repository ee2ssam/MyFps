using UnityEngine;

namespace MySample
{
    public class MoveTest : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;

        [SerializeField] private float forwardForce = 5f;   //앞으로 가는 힘
        [SerializeField] private float sideForce = 5f;      //좌우로 가는 힘

        private float dx;   //좌우 입력값
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //참조
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            dx = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            //앞으로 이동
            rb.AddForce(0f, 0f, forwardForce, ForceMode.Acceleration);

            //좌우 이동
            if(dx < 0f)
            {
                rb.AddForce(-sideForce, 0f, 0f, ForceMode.Acceleration);
            }
            if(dx > 0f)
            {
                rb.AddForce(sideForce, 0f, 0f, ForceMode.Acceleration);
            }
        }
    }
}
