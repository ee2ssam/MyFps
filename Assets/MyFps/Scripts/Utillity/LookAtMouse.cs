using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 오브젝트가 마우스 포인터를 바라본다
    /// </summary>
    public class LookAtMouse : MonoBehaviour
    {
        #region Variables
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //Vector3 worldPosition = RayToWorld();
            Vector3 worldPosition = ScreenToWorld();

            transform.LookAt(worldPosition);
        }
        #endregion

        #region Custom Method
        private Vector3 ScreenToWorld()
        {
            float depth = 2.7f;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            return worldPos;
        }

        //마우스 포인터 위치에서 레이를 쏘아 히트한 지점의 위치를 반환한다
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