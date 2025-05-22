using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    //피스톨 제어 클래스
    public class PistolShoot : MonoBehaviour
    {
        #region Variable
        #endregion

        #region Unity Event Method
        #endregion

        #region Custom Method
        public void OnFire(InputAction.CallbackContext context)
        {
            if(context.started) //keydown, bottondown
            {
                Debug.Log("Fire!!!!");
            }
        }
        #endregion
    }
}