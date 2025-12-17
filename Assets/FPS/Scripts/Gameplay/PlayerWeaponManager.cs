using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Game;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 무기 교체 상태 정의
    /// </summary>
    public enum WeaponSwitchState
    {
        Up,                 //현재 무기를 들고 있는 상태
        Down,               //교체하기 위해 무기를 내린 상태
        PutDownPrevious,    //교체하기 위해 업에서 다운으로 가는 상태
        PutUpNew,           //교체 후 다운에서 업으로 가는 상태    
    }

    /// <summary>
    /// 플레이어가 가지고 무기<WeaponController>들을 관리하는 클래스
    /// </summary>
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerInputHandler inputHandler;

        //무기 장착
        //처음 지급되는 무기(WeaponController가 붙어 있는 프리팹) 리스트
        public List<WeaponController> startingWeapons = new List<WeaponController>();

        //무기가 장착될 오브젝트
        public Transform weaponParentSocrket;

        //플레이어가 게임중에 들고다니는 무기 슬롯 리스트
        private WeaponController[] weaponSlots = new WeaponController[9];

        //무기 교체        
        public UnityAction<WeaponController> onSwitchToWeapon;  //무기 교체시 등록된 함수를 호출

        private WeaponSwitchState weaponSwitchState;    //교체 상태

        private Vector3 weaponMainLocalPosition;        //액티브 무기의 위치 계산 값

        public Transform defaultWeaponPosition;           //무기를 들고 있을때의 위치
        public Transform downWeaponPosition;              //무기를 교체할때의 위치

        private int weaponSwitchNewWeaponIndex;           //무기 교체할때 새로운 무기의 인덱스

        //교체 연출
        private float weaponSwitchTimeStarted = 0f;        //무기 교체 시작 시간
        [SerializeField]
        private float weaponSwitchDelay = 1f;               //무기 교체 딜레이
        #endregion

        #region Property
        //무기 슬롯(weaponSlots)을 관리하는 인덱스
        public int ActiveWeaponIndex { get; private set; }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            //초기화
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;

            //무기 교체 이벤트 함수 등록
            onSwitchToWeapon += OnWeaponSwitched;

            //지급받은 무기를 무기 장착 (startingWeapons(프리팹) -> weaponSlots)
            foreach (var w in startingWeapons)
            {
                AddWeapon(w);
            }
            //지급 받은 무기중에 맨 앞에 있는 무기 활성화
            SwitchWeapon(true); 
        }

        private void Update()
        {
            //Weapon Input 처리
            if(weaponSwitchState == WeaponSwitchState.Up || weaponSwitchState == WeaponSwitchState.Down)
            {
                //무기 교체 인풋
                int swithWeaponIndex = inputHandler.GetSwitchWeaponInput();
                if(swithWeaponIndex != 0)
                {
                    bool isSwitchUp = swithWeaponIndex > 0f;
                    SwitchWeapon(isSwitchUp);
                }
            }
        }

        private void LateUpdate()
        {
            UpdateWeaponSwitching();

            //무기 위치 최종 연산값을 적용
            weaponParentSocrket.localPosition = weaponMainLocalPosition;
        }
        #endregion

        #region Custom Method
        //무기 교체 상태에 따른 무기 연출 및 상태 전환
        private void UpdateWeaponSwitching()
        {
            //Lero 변수
            float switchingTimeFactor = 0f;
            if(weaponSwitchDelay == 0)
            {
                switchingTimeFactor = 1;
            }
            else
            {
                switchingTimeFactor = Mathf.Clamp01((Time.time - weaponSwitchTimeStarted) / weaponSwitchDelay);
            }

            //타이머 완료: 연출 완료
            if(switchingTimeFactor >= 1f)
            {
                if (weaponSwitchState == WeaponSwitchState.PutDownPrevious)
                {
                    //무기 교체 : 현재 무기 false, 새로운 무기 true
                    WeaponController oldWeapon = GetWeaponAtSlotIndex(ActiveWeaponIndex);
                    if (oldWeapon != null)
                    {
                        oldWeapon.ShowWeapon(false);
                    }

                    ActiveWeaponIndex = weaponSwitchNewWeaponIndex;
                    WeaponController newWeaponController = GetWeaponAtSlotIndex(weaponSwitchNewWeaponIndex);
                    onSwitchToWeapon?.Invoke(newWeaponController);

                    //무기 연출
                    switchingTimeFactor = 0f;   //left 변수 초기화
                    if (newWeaponController != null)
                    {
                        weaponSwitchTimeStarted = Time.time;
                        weaponSwitchState = WeaponSwitchState.PutUpNew;
                    }
                    else
                    {
                        weaponSwitchState = WeaponSwitchState.Down;
                    }
                }
                else if (weaponSwitchState == WeaponSwitchState.PutUpNew)
                {
                    weaponSwitchState = WeaponSwitchState.Up;
                }
            }

            //무기 위치 이동 연출
            if(weaponSwitchState == WeaponSwitchState.PutDownPrevious)
            {
                weaponMainLocalPosition = Vector3.Lerp(defaultWeaponPosition.localPosition,
                    downWeaponPosition.localPosition, switchingTimeFactor);
            }
            else if(weaponSwitchState == WeaponSwitchState.PutUpNew)
            {
                weaponMainLocalPosition = Vector3.Lerp(downWeaponPosition.localPosition,
                    defaultWeaponPosition.localPosition, switchingTimeFactor);
            }
        }

        //무기 장착(WeaponController를 가지고 있는 프리팹 -> weaponSlots)
        private bool AddWeapon(WeaponController weaponPrefab)
        {
            //추가하는 무기를 소지 여부 체크 - 중복 검사
            if(HasWeapon(weaponPrefab) != null)
            {
                Debug.Log("Have Same Weapon");
                return false;
            }

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                //빈슬롯 체크
                if (weaponSlots[i] == null)
                {
                    //무기 장착
                    WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocrket);
                    //무기 셋팅
                    weaponInstance.transform.localPosition = Vector3.zero;  //부모 오브젝트와 동일한 위치
                    weaponInstance.transform.localRotation = Quaternion.identity;

                    weaponInstance.Owner = gameObject;
                    weaponInstance.SoucePrefab = weaponPrefab.gameObject;
                    weaponInstance.ShowWeapon(false);

                    //셋팅한 무기를 슬롯에 추가
                    weaponSlots[i] = weaponInstance;

                    return true;
                }
            }

            //슬롯 풀(장착 실패)
            Debug.Log("weaponSlots full");
            return false;

        }

        //매개변수로 들어온 weaponPrefab으로 생성된 무기가 있으면 생성된 무기 반환
        private WeaponController HasWeapon(WeaponController weaponPrefab)
        {
            foreach (var w in weaponSlots)
            {
                if(w != null && w.SoucePrefab == weaponPrefab.gameObject)
                {
                    return w;
                }
            }
            
            //생성된 무기가 없으면 
            return null;
        }

        //현재 액티브 무기 가져오기
        public WeaponController GetAcitveWeapon()
        {
            return GetWeaponAtSlotIndex(ActiveWeaponIndex);
        }    

        //매개변수로 받은 슬롯 인덱스의 무기를 반환
        private WeaponController GetWeaponAtSlotIndex(int index)
        {
            if(index >= 0 && index < weaponSlots.Length)
            {
                return weaponSlots[index];
            }

            return null;
        }

        //무기 교체 : 현재들고 있는 무기 false, 새로운 무기 true
        private void SwitchWeapon(bool ascendingOrder)
        {
            //최소 거리에 있는 무기 찾기
            int newWeponIndex = -1;                         //새로운 무기 인덱스
            int closestSlotDistance = weaponSlots.Length;   //빈 슬롯 중 가장 가까운 거리 찾기
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                //현재 들고 있는 무기가 아니고, 빈슬롯 아니면
                if(i != ActiveWeaponIndex && GetWeaponAtSlotIndex(i) != null)
                {
                    //현재 들고 있는 무기와 슬롯간의 거리 구하기
                    int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(ActiveWeaponIndex, i, ascendingOrder);
                    if (distanceToActiveIndex < closestSlotDistance)
                    {
                        closestSlotDistance = distanceToActiveIndex;
                        newWeponIndex = i;
                    }
                }
            }

            //최소 거리에 있는 무기로 교체
            SwitchToWeaponIndex(newWeponIndex);
        }

        //매개변수로 들어온 인덱스의 무기로 교체
        private void SwitchToWeaponIndex(int newWeaponIndex)
        {
            //현재 들고 있는 무기 체크
            if (newWeaponIndex == ActiveWeaponIndex)
                return;
            //슬롯 인덱스 체크
            if (newWeaponIndex < 0 || newWeaponIndex >= weaponSlots.Length)
                return;

            weaponSwitchNewWeaponIndex = newWeaponIndex;        //교체할 무기 인덱스 저장
            weaponSwitchTimeStarted = Time.time;                //교체 시작하는 시간을 저장

            if(GetAcitveWeapon() == null)
            {
                weaponSwitchState = WeaponSwitchState.PutUpNew;
                weaponMainLocalPosition = downWeaponPosition.localPosition;

                ActiveWeaponIndex = newWeaponIndex;
                WeaponController newWeaponController = GetWeaponAtSlotIndex(newWeaponIndex);
                onSwitchToWeapon?.Invoke(newWeaponController);
            }
            else
            {
                weaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }

        //슬롯간의 거리 구하기
        private int GetDistanceBetweenWeaponSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
        {
            int distance = 0;

            if (ascendingOrder)
            {
                distance = toSlotIndex - fromSlotIndex;
            }
            else
            {
                distance = fromSlotIndex - toSlotIndex;
            }

            if (distance < 0)
            {
                distance += weaponSlots.Length;
            }

            return distance;
        }
        
        //매개변수로 들어온 무기를 보여준다
        private void OnWeaponSwitched(WeaponController newWeapon)
        {
            if(newWeapon == null)
                return;

            newWeapon.ShowWeapon(true);
        }
        #endregion
    }
}