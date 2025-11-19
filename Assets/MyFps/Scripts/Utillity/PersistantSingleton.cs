using UnityEngine;

namespace MyFps
{
    public class PersistantSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
