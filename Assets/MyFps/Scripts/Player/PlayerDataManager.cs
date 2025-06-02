using UnityEngine;

namespace MyFps
{
    //장착 무기 타입 enum
    public enum WeaponType
    {
        None,
        Pistol,
    }

    //퍼즐 아이템 enum값 설정
    public enum PuzzleKey
    {
        ROOM01_KEY,
        LEFTEYE_KEY,
        RIGHTEYE_KEY,
        MAX_KEY              //퍼즐 아이템 갯수
    }

    //플레이어 데이터 관리 클래스 - 싱글톤(다음 씬에서 데이터 보존)
    public class PlayerDataManager : PersistanceSingleton<PlayerDataManager>
    {
        #region Variables
        private int ammoCount;

        private bool[] hasKeys;         //퍼즐 아이템 소지 여부 체크
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
            InitPlayerData();
        }

        //플레이 데이터 초기화 
        private void InitPlayerData()
        {
            ammoCount = 0;
            Weapon = WeaponType.None;

            //퍼즐 아이템 설정: 퍼즐 아이템 갯수만큼 불형 변수 요소수 생성
            hasKeys = new bool[(int)PuzzleKey.MAX_KEY];

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

        //퍼즐 아이템 획득 - 매개변수로 퍼즐키 타입 받는다
        public void GainPuzzleKey(PuzzleKey key)
        {
            hasKeys[(int)key] = true;
        }

        //퍼즐 아이템 소지 여부 체크 - 매개변수로 퍼즐키 타입 받는다
        public bool HasPuzzleKey(PuzzleKey key)
        {
            return hasKeys[(int)key];
        }
        #endregion

    }
}
