using UnityEngine;

namespace MySample
{
    public class PlayerMove : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;

        [SerializeField] private float fowardForce = 5f;
        [SerializeField] private float sideForce = 5f;

        private float inputX = 0f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            inputX = Input.GetAxis("Horizontal");

        }

        private void FixedUpdate()
        {
            //앞으로 이동
            rb.AddForce(0f, 0f, fowardForce, ForceMode.Acceleration);

            //좌우 이동
            if(inputX < 0f)
            {
                rb.AddForce(-sideForce, 0f, 0f, ForceMode.Acceleration);
            }
            else if (inputX > 0f)
            {
                rb.AddForce(sideForce, 0f, 0f, ForceMode.Acceleration);
            }

        }
        #endregion
    }
}