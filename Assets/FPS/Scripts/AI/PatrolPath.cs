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
        public List<Transform> PathNodes = new List<Transform>();

        //PatrolPath를 패트롤하는 enemy 리스트
        public List<EnemyController> enemiesToAssign = new List<EnemyController>();
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //패트롤 패스 등록
            foreach (var enemy in enemiesToAssign)
            {
                enemy.PatrolPath = this;
            }
        }

        //패트롤 경로 기즈모로 그리기
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < PathNodes.Count; i++)
            {
                int nextIndex = i + 1;
                if(nextIndex >= PathNodes.Count)
                {
                    nextIndex -= PathNodes.Count;
                }

                Gizmos.DrawLine(PathNodes[i].position, PathNodes[nextIndex].position);
                Gizmos.DrawSphere(PathNodes[i].position, 0.1f);
            }
        }
        #endregion

        #region Custom Method
        //지정된 위치부터 지정된 노드까지의 거리 구하기
        public float GetDistanceToNode(Vector3 originPos, int destinationNodeIndex)
        {
            //인덱스 체크
            if(destinationNodeIndex < 0 || destinationNodeIndex >= PathNodes.Count
                || PathNodes[destinationNodeIndex] == null)
            {
                return -1f;
            }

            return (PathNodes[destinationNodeIndex].position - originPos).magnitude;
        }

        //지정된 노드의 위치 반환
        public Vector3 GetPositionOfPathNode(int nodeIndex)
        {
            //인덱스 체크
            if (nodeIndex < 0 || nodeIndex >= PathNodes.Count
                || PathNodes[nodeIndex] == null)
            {
                return Vector3.zero;
            }

            return PathNodes[nodeIndex].position;
        }
        #endregion

    }
}