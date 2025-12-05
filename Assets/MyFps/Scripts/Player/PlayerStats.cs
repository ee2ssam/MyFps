using UnityEngine;

namespace MyFps
{
    //손에 든 무기 타입 enum 정의
    public enum WeaponType
    {
        None = 0,   //무기가 없을때
        Pistol,
        Healmatic,
    }

    //퍼즐 아이템 enum 정의
    public enum PuzzleItem
    {
        None = 0,
        Key01,
        LeftEye,
        RightEye,

        MaxPuzzleItem
    }


    /// <summary>
    /// 플레이어의 데이터를 관리하는 싱글톤 클래스
    /// 모든 씬에서 계속 데이터를 유지 관리
    /// </summary>
    public class PlayerStats : PersistantSingleton<PlayerStats>
    {
        #region Variables
        //씬 번호
        private int sceneNumber;
        private string sceneName;

        //탄환 갯수
        [SerializeField]
        private int ammoCont;

        //소지 무기 타입
        [SerializeField]
        private WeaponType weaponType;

        //플레이어 체력
        private float health;
        [SerializeField]
        private float maxHealth = 20f;

        //퍼즐 아이템 획득 여부
        [SerializeField]
        private bool[] puzzleItems;
        #endregion

        #region Property
        public int SceneNumber { get { return sceneNumber; } }
        public string SceneName { get { return sceneName; } }
        public int AmmonCount { get { return ammoCont; } }
        public WeaponType WeaponType { get { return weaponType; } }
        public float Health { get { return health; } }
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //TO DO : cheating
            //PlayerStatsInitialize(null);
            //weaponType = WeaponType.Pistol;
        }
        #endregion

        #region Custom Method
        //플레이어 데이터 초기화
        public void PlayerStatsInitialize(PlayData playData)
        {
            if (playData != null)
            {
                sceneNumber = playData.sceneNumber;
                sceneName = playData.sceneName;
                ammoCont = playData.ammoCount;
                weaponType = GetWeaponType(playData.weaponType);
                health = playData.health;
            }
            else
            {
                //처음 다운로드 받고 처음 실행
                sceneNumber = -1;
                sceneName = string.Empty;
                ammoCont = 0;
                weaponType = WeaponType.None;
                health = maxHealth;
            }

            puzzleItems = new bool[(int)PuzzleItem.MaxPuzzleItem];
        }

        //1번씬 데이터 초기값 설정
        public void PlayerStatsInit()
        {
            ammoCont = 0;
            weaponType = WeaponType.None;
            health = maxHealth;
        }

        //씬 번호 대입하기
        public void SetSceneNumnber(int number)
        {
            sceneNumber = number;
        }

        public void SetSceneName(string name)
        {
            sceneName = name;
        }

        // ammo 추가하기
        public void AddAmmo(int amount)
        {
            ammoCont += amount;
            //Debug.Log($"ammoCont: {ammoCont}");
        }

        // ammo 사용하기
        public bool UseAmmo(int amount = 1)
        {
            if(ammoCont < amount)
            {
                Debug.Log("You need to reload");
                return false;
            }

            ammoCont -= amount;
            //Debug.Log($"ammoCont: {ammoCont}");
            return true;
        }

        //무기 획득(교체)
        public void SetWeaponType(WeaponType type)
        {
            weaponType = type;
        }

        //
        private WeaponType GetWeaponType(int type)
        {
            switch (type)
            {
                case 0: return WeaponType.None;
                case 1: return WeaponType.Pistol;
                case 2: return WeaponType.Healmatic;
            }

            return WeaponType.None;
        }

        //매개변수로 입력 받은 퍼즐 아이템 획득 여부
        public bool HavePuzzleItem(PuzzleItem puzzleItem)
        {
            //아이템이 없다
            if (puzzleItem == PuzzleItem.None || puzzleItem == PuzzleItem.MaxPuzzleItem)
            {
                Debug.Log("Out of range");
                return false;
            }   

            return puzzleItems[(int)puzzleItem];
        }


        //매개변수로 입력 받은 퍼즐 아이템 획득, 성공/실패 처리
        public bool GainPuzzleItem(PuzzleItem puzzleItem)
        {
            //획득 실패
            if(puzzleItem == PuzzleItem.None || puzzleItem == PuzzleItem.MaxPuzzleItem)
            {
                Debug.Log("Out of range");
                return false;
            }

            puzzleItems[(int)puzzleItem] = true;
            return true;
        }

        //체력 저장
        public void SetHealth(float value)
        {
            health = value;
        }
        #endregion

    }
}
