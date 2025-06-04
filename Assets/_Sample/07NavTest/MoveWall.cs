using UnityEngine;

namespace MySample
{
    //이동벽 : 좌우로 패트롤 이동, 
    public class MoveWall : MonoBehaviour
    {
        #region Variable
        [SerializeField]
        private float moveSpeed = 1f;

        [SerializeField]
        private float moveTime = 1f;
        [SerializeField]
        private float countdown = 0f;

        //이동방향: 1이면 오르쪽, -1이면 왼쪽
        [SerializeField]
        private float dir = 1f;
        #endregion

        #region Unity Event Method 
        private void Update()
        {
            //타이머 시간마다 방향을 바꿔서 이동
            countdown += Time.deltaTime;
            if(countdown >= moveTime)
            {
                //방향을 바꾼다
                dir *= -1;

                //타이머 초기화
                countdown = 0f;
            }

            transform.Translate((Vector3.right * dir) * Time.deltaTime * moveSpeed, Space.World);
        }
        #endregion
    }
}
