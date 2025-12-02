using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 충돌시 충돌 상대속도가 1이상이면 충돌 사운드 플레이
    /// </summary>
    public class FlyObejct : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            //상대속도가 1이상이면
            if(collision.relativeVelocity.magnitude > 1.0f)
            {
                AudioManager.Instance.Play("DoorBang");
            }
        }
    }
}