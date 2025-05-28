using UnityEngine;

namespace MySample
{
    //플레이어 이동 클래스
    public class PlayerMove : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody rb;

        //앞으로 이동하는 힘
        [SerializeField]
        private float forwardForce = 10f;
        //좌우로 이동하는 힘
        [SerializeField]
        private float sideForce = 5f;

        //인풋 값
        private float dx = 0f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            rb = this.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            dx = Input.GetAxis("Horizontal"); //-1 ~ 1
        }

        private void FixedUpdate()
        {
            rb.AddForce(0f, 0f, forwardForce, ForceMode.Acceleration);

            //좌우 이동
            if(dx < 0)
            {
                rb.AddForce(-sideForce, 0f, 0f, ForceMode.Acceleration);
            }
            if(dx > 0)
            {
                rb.AddForce(sideForce, 0f, 0f, ForceMode.Acceleration);
            }
        }
        #endregion
    }
}

/*
이동은 Rigidbody Force를 이용하여 이동
앞으로 자동으로 이동한다
좌우키를 입력받아 좌우 이동한다
*/