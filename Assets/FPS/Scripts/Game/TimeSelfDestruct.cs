using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// TimeSelfDestruct 컴포넌트가 붙어 있으면 생성후 LiftTime이 지나면 킬
    /// </summary>
    public class TimeSelfDestruct : MonoBehaviour
    {
        #region Variables
        public float lifeTime = 1f;

        private float spawnTime;        //생성될때의 시간
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //생성 시간 저장
            spawnTime = Time.time;
        }

        private void Update()
        {
            //생성시간에서 라이프 타임이 지나가면 킬
            if(Time.time >= (spawnTime + lifeTime))
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
}