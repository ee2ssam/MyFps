using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 데미지를 입으면 깨지는 오브젝트
    /// 깨지는 연출 : Fake오브젝트가 없어지 break오브젝트 활성화
    /// </summary>
    public class BreakableObject : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
    }
}
