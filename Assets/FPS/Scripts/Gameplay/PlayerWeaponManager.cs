using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 플레이어가 가지고 다니는 무기<WeaponController>들을 관리하는 클래스
    /// </summary>
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        //참조 - 인풋 처리
        private PlayerInputHandler inputHandler;

        //무기 장착
        //유저에게 처음 지급되는 무기<WeaponController> 리스트
        public List<WeaponController> startingWeapons = new List<WeaponController>();

        //무기가 장착될 부모 오브젝트
        public Transform weaponParentSocket;

        //플레이어가 게임중에 들고 다닐수 있는 무기 리스트
        private WeaponController[] weaponSlots = new WeaponController[9];
        //무기리스트(슬롯)을 관리하는 인덱스 - 현재 사용하고 있는 무기의 인덱스
        public int ActiveWeaponIndex { get; private set; }

        //무기 교체
        //무기 교체 상태
        public enum WeaponSwitchState
        {
            Up,                 //무기 들고 있는 상태
            Down,               //무기가 내려가 있는 상태
            PutDownPrevious,    //무기를 교체하기 위해 내릴려는 상태
            PutUpNew,           //다운상태에서 무기 교체후 올리려는 상태
        }

        //무기 교체시 등록된 함수 호출하는 이벤트 함수
        public UnityAction<WeaponController> OnSwitchToWeapon;

        //무기 교체 상태 변수
        private WeaponSwitchState weaponSwitchState;

        //연산되는 무기의 최종 위치
        private Vector3 weaponMainLocalPosition;

        public Transform defaultWeaponPosition;     //무기 up 위치
        public Transform downWeaponPosition;        //무기 down 위치

        //교체 연출에 필요한 변수
        private int weaponSwitchNewWeaponIndex;
        private float weaponSwitchTimeStarted = 0f;
        [SerializeField] private float weaponSwitchDelay = 1f;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            inputHandler = GetComponent<PlayerInputHandler>();

            //초기화
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            //지급 받은 무기 장착하기
            foreach (var w in startingWeapons)
            {
                AddWeapon(w);
            }
        }
        #endregion

        #region Custom Method
        //지급 받은 무기<WeaponController>를 무기 슬롯에 추가하기
        public bool AddWeapon(WeaponController weaponPrefab)
        {
            //추가하는 무기 소지 여부 체크 - 중복 검사
            if(HasWeapon(weaponPrefab) != null)
            {
                Debug.Log("Have Same Weapon");
                return false;
            }

            //빈슬롯에 무기<WeaponController> 추가하기
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                //빈슬롯 찾기
                if (weaponSlots[i] == null)
                {
                    //무기 생성 후 슬롯에 추가
                    WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                    weaponInstance.transform.localPosition = Vector3.zero;
                    weaponInstance.transform.localRotation = Quaternion.identity;

                    //무기 초기화
                    weaponInstance.Owner = gameObject;
                    weaponInstance.SourcePrefab = weaponPrefab.gameObject;
                    weaponInstance.ShowWeapon(false);

                    //슬롯에 추가
                    weaponSlots[i] = weaponInstance;
                    return true;
                }
            }

            Debug.Log("Weapon Slots Full");
            return false;
        }

        //매개변수로 들어온 프리팹으로 생성된 무기가 있으면 생성된 무기 반환
        public WeaponController HasWeapon(WeaponController weaponPrefab)
        {
            //슬롯에서 무기 체크
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                var w = weaponSlots[i];
                if(w != null && w.SourcePrefab == weaponPrefab.gameObject)
                {
                    return w;
                }
            }

            return null;
        }

        //지정한 인덱스의 슬롯 무기 반환
        public WeaponController GetWeaponAtSlotIndex(int index)
        {
            //index의 범위 체크
            if (index < 0 || index >= weaponSlots.Length)
                return null;

            return weaponSlots[index];
        }

        //현재 활성화된 무기 가져오기
        public WeaponController GetActiveWeapon()
        {
            return GetWeaponAtSlotIndex(ActiveWeaponIndex);
        }

        //무기 교체하기, ascendingOrder: 오름차순, 내림차순으로 무기 교체하기
        public void SwitchWeapon(bool ascendingOrder)
        {

        }
        #endregion
    }
}