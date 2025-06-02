using UnityEngine;

namespace MyFps
{
    //날아가다 그라운드에 떨어지면 부딪히는 사운드 플레이
    public class FlyingObject : MonoBehaviour
    {
        #region Unity Event Method
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == "Ground" && collision.relativeVelocity.magnitude > 1.0f)
            {
                AudioManager.Instance.Play("DoorBang2");
            }
        }
        #endregion
    }
}
