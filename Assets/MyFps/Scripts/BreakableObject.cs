using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 총을 맞으면(데미지를 입으면) 깨지는 오브젝트
    /// </summary>
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
    }
}