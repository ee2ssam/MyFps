using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어를 관리(제어하는) 클래스
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerHealth playerHealth;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            playerHealth = GetComponent<PlayerHealth>();
        }
        #endregion


    }
}