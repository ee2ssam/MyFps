using UnityEngine;

namespace MyFps
{
    //플레이어의 속성, 데이터값을 관리하는 (싱글톤, DontDestory)클래스.. ammoCount
    public class PlayerStats : PersistentSingleton<PlayerStats>
    {
        #region Variables
        //탄환 갯수
        [SerializeField] private int ammoCount;
        public int AmmoCount
        { 
            get { return ammoCount; }
            private set { ammoCount = value; }
        }
        #endregion

        private void Start()
        {
            //속성값/Data 초기화
            AmmoCount = 0;
        }

        public void AddAmmo(int amount)
        {
            AmmoCount += amount;
        }

        public bool UseAmmo(int amount)
        {
            //소지 갯수 체크
            if(AmmoCount < amount)
            {
                Debug.Log("You need to reload!!!!");
                return false;   //사용량보다 부족하다
            }

            AmmoCount -= amount;
            return true;
        }


    }
}