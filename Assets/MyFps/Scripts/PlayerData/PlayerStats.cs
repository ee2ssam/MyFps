using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 플레이어의 속성값들을 관리하는 싱글톤 클래스
    /// </summary>
    public class PlayerStats : PersistanctSingleton<PlayerStats>
    {
        #region Varibles
        private int ammoCount;
        #endregion

        #region Property
        public int AmmoCount => ammoCount;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            ammoCount = 0;
        }
        #endregion

        #region Custom Method
        //탄환 추가
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
        }

        //탄환 사용하기
        public bool UseAmmo(int amount = 1)
        {
            if(ammoCount < amount)
            {
                Debug.Log("You need to reload");
                return false;
            }

            ammoCount -= amount;
            return true;
        }
        #endregion

    }
}