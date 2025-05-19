using UnityEngine;

namespace MySample
{
    public class MoveObejct : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody rb;

        //오브젝트에 주는 힘
        [SerializeField]
        private float movePower = 3f;

        //오브젝트 컬러 바꾸기
        private Renderer render;
        private Color originColor;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            rb = this.GetComponent<Rigidbody>();
            render = this.GetComponent<Renderer>();

            //초기화
            originColor = render.material.color;

            //시작할때 오른쪽 힘 준다: 3f  - 일회성 힘
            MoveRight();
        }

        #endregion

        #region Custom Method
        //플레이어를 왼쪽으로 3만큼 힘을 준다
        public void MoveLeft()
        {
            rb.AddForce(Vector3.left * movePower, ForceMode.Impulse);
        }

        //플레이어를 오른쪽으로 3만큼 힘을 준다
        public void MoveRight()
        {
            rb.AddForce(Vector3.right * movePower, ForceMode.Impulse);
        }

        //빨간색으로 바꾼다
        public void ChangeColorRed()
        {
            render.material.color = Color.red;
        }

        //오리지널 컬러로 바꾼다
        public void ChangeColorOrigin()
        {
            render.material.color = originColor;
        }
        #endregion
    }
}