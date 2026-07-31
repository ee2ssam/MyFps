using UnityEngine;

namespace Unity.FPS.Utility
{
    /// <summary>
    /// 생성후 lifeTime이 지나면 오브젝트를 자동 킬 한다
    /// </summary>
    public class TimeSelfDestruct : MonoBehaviour
    {
        public float lifeTime = 3f;
        private float spawnTime;        //생성 시간

        private void Awake()
        {
            spawnTime = Time.time;
        }

        private void Update()
        {
            if(Time.time > spawnTime + lifeTime)
            {
                Destroy(gameObject);
            }
        }

    }
}