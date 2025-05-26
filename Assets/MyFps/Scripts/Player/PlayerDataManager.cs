using UnityEngine;

namespace MyFps
{
    //장착 무기 타입 enum
    public enum WeaponType
    {
        None,
        Pistol,
    }

    //플레이어 데이터 관리 클래스 - 싱글톤(다음 씬에서 데이터 보존)
    public class PlayerDataManager : PersistanceSingleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;
        #endregion

        #region Property
        //무기 타입
        public WeaponType Weapon { get; set; }

        //탄환 갯수 리턴하는 읽기 전용 프로퍼티
        public int AmmoCount => ammoCount;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //플레이 데이터 초기화 
            ammoCount = 0;
            Weapon = WeaponType.None;
        }
        #endregion

        #region Custom Method
        //ammo 저축 함수
        public void AddAmmo(int amount)
        {
            ammoCount += amount;
        }

        //ammo 사용 함수
        public bool UseAmmo(int amount)
        {
            //소지 ammo 체크
            if (ammoCount < amount)
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
