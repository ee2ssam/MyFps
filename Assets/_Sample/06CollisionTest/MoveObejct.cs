using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 오브젝트의 이동 구현: 리디지바디의 힘을 이용
    /// </summary>
    public class MoveObejct : MonoBehaviour
    {
        #region Variables
        private Rigidbody rb;

        //입력 값(좌, 우)
        private float moveX;

        //이동 힘
        [SerializeField]
        private float movePower = 10f;

        public Material redMaterial;

        private Renderer renderer;
        private Material originMaterial;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb = GetComponent<Rigidbody>();
            renderer = GetComponent<Renderer>();

            //초기화
            originMaterial = renderer.material;
        }

        private void Update()
        {
            //좌우 입력
            moveX = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            //힘에 의한 오브젝트 좌우 이동
            rb.AddForce(Vector3.right * moveX * movePower, ForceMode.Force);
        }
        #endregion

        #region Custom Method
        //왼쪽으로 힘(200f)
        public void MoveLeft()
        {
            rb.AddForce(Vector3.left * movePower, ForceMode.Impulse);
        }

        //오른쪽으로 힘(200f)
        public void MoveRigth()
        {
            rb.AddForce(Vector3.right * movePower, ForceMode.Impulse);
        }

        //이동 메터리얼 변경
        public void ChangeMoveColor()
        {
            renderer.material = redMaterial;
        }

        //컬러 초기화
        public void ResetMoveColor()
        {
            renderer.material = originMaterial;
        }
        #endregion
    }
}