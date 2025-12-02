using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 1초마다 좌우로 방향을 바꾸어 이동한다
    /// </summary>
    public class MoveWall : MonoBehaviour
    {
        #region Variables
        public float moveSpeed = 3f;
        public float moveTimer = 1f;
        public float countdown = 0f;

        public float dir = 1f;      //이동방향 1:오른쪽, -1:왼쪽
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //타이머
            countdown += Time.deltaTime;
            if (countdown >= moveTimer)
            {
                //타이머 기능, 방향전환
                dir *= -1;

                //타이머 초기화
                countdown = 0f;
            }

            transform.Translate(Vector3.right * dir * Time.deltaTime * moveSpeed, Space.World);
        }
        #endregion
    }
}