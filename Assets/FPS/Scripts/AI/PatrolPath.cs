using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 패트롤 웨이포인트를 관리하는 클래스
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        #region Variables
        //웨이포인트 리스트
        public List<Transform> pathNodes = new List<Transform>();
        #endregion

        #region Unity Event Method
        //경로 그리기 - 기즈모 (0-1, 1-2, 2-0)
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < pathNodes.Count; i++)
            {
                int nextIndex = i + 1;
                if(nextIndex >= pathNodes.Count)
                {
                    nextIndex = 0;
                }
                Gizmos.DrawLine(pathNodes[i].position, pathNodes[nextIndex].position);
            }
        }
        #endregion
    }
}