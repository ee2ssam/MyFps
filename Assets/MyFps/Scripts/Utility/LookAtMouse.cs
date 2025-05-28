using System.Drawing;
using UnityEngine;

namespace MyFps
{
    //오브젝트가 마우스 포인터를 바라본다
    public class LookAtMouse : MonoBehaviour
    {
        #region Variables
        //마우스 포인터가 가리키는 월드 포지션값
        private Vector3 worldPosition;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //월드 포지션값 얻어오기 - Ray 이용
            worldPosition = ScreenToWorld();

            //월드 포지션값 바라보기
            transform.LookAt(worldPosition);
        }
        #endregion

        #region Custom Method
        //월드 포지션값 얻어오기 - 마우스의 위치값을 이용하여
        private Vector3 ScreenToWorld()
        {
            Vector3 worldPos = Vector3.zero;
            Vector3 mousePosition = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 3.8f));

            return worldPos;
        }

        //월드 포지션값 얻어오기 - 마우스의 위치에 쏘는 Ray를 이용
        private Vector3 RayToWorld()
        {
            Vector3 worldPos = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                worldPos = hit.point;
            }

            return worldPos;
        }
        #endregion

    }
}
