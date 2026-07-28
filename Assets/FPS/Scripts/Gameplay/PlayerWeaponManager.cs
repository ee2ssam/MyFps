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
        public Transform aimingWeaponPosition;      //무기 조준 위치

        //교체 연출에 필요한 변수
        private int weaponSwitchNewWeaponIndex;
        private float weaponSwitchTimeStarted = 0f;
        [SerializeField] private float weaponSwitchDelay = 1f;
                
        //적 타겟팅
        public bool IsPointingAtEnemy { get; private set; }     //적 타겟팅 여부
        public Camera weaponCamera;                              //무기 전용 카메라

        //카메라
        private PlayerCharacterController playerCharacterController;
        public float defaultFov = 60f;                          //FOV 기본값
        public float weaponFovMultiplier = 1f;                  //무기 fov 계수값

        //조준
        public bool IsAiming { get; private set; }          //조준 여부
        public float aimingAnimationSpeed = 10f;            //연출 속도
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            inputHandler = GetComponent<PlayerInputHandler>();
            playerCharacterController = GetComponent<PlayerCharacterController>();
        }

        private void Start()
        {
            //초기화
            ActiveWeaponIndex = -1;
            weaponSwitchState = WeaponSwitchState.Down;
            SetFov(defaultFov);

            //무기 교체 이벤트 등록
            OnSwitchToWeapon += OnWeaponSwitched;

            //지급 받은 무기 장착하기
            foreach (var w in startingWeapons)
            {
                AddWeapon(w);
            }
            SwitchWeapon(true);
        }

        private void Update()
        {
            //현재 손에 들고 있는 무기(액티브 무기) 가져오기
            WeaponController activeWeapon = GetActiveWeapon();

            //인풋 처리
            IsAiming = inputHandler.GetAimInputHeld();

            //조준 안하고 있을때만 무기 교체 가능
            if (IsAiming == false)
            {
                if (weaponSwitchState == WeaponSwitchState.Up
                    || weaponSwitchState == WeaponSwitchState.Down)
                {
                    int switchWeaponInput = inputHandler.GetSwitchWeaponInput();
                    if (switchWeaponInput != 0)
                    {
                        bool switchUp = switchWeaponInput > 0f;
                        SwitchWeapon(switchUp);
                    }
                }
            }

            //적 타겟팅
            IsPointingAtEnemy = false;
            if(activeWeapon)
            {
                if(Physics.Raycast(weaponCamera.transform.position,
                    weaponCamera.transform.forward, out RaycastHit hit, 100f))
                {
                    //Debug.Log($"hit {hit.collider.gameObject.name}");
                    Health enemyHealth = hit.collider.GetComponentInParent<Health>();
                    if(enemyHealth)
                    {                        
                        IsPointingAtEnemy = true;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            UpdateWeaponAiming();
            UpdateWeaponSwitching();

            //무기의 최종 위치
            weaponParentSocket.localPosition = weaponMainLocalPosition;
        }
        #endregion

        #region Custom Method
        //FOV 조정하기
        public void SetFov(float fov)
        {
            playerCharacterController.PlayerCamera.fieldOfView = fov;
            weaponCamera.fieldOfView = fov * weaponFovMultiplier;
        }

        //무기 조준 연출 : 디폴트위치 <-> 조준위치
        private void UpdateWeaponAiming()
        {
            //상태 체크
            if (weaponSwitchState != WeaponSwitchState.Up)
                return;
            
            WeaponController activeWeapon = GetActiveWeapon();
            if(IsAiming && activeWeapon)
            {
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition,
                   aimingWeaponPosition.localPosition + activeWeapon.aimOffset,
                   aimingAnimationSpeed * Time.deltaTime);
                SetFov(Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView, 
                    activeWeapon.aimZoomratio * defaultFov, aimingAnimationSpeed * Time.deltaTime));
            }
            else
            {
                weaponMainLocalPosition = Vector3.Lerp(weaponMainLocalPosition,
                   defaultWeaponPosition.localPosition, aimingAnimationSpeed * Time.deltaTime);
                SetFov(Mathf.Lerp(playerCharacterController.PlayerCamera.fieldOfView,
                    defaultFov, aimingAnimationSpeed * Time.deltaTime));
            }

        }

        //무기 상태 변화로 무기 교체 연출 : 디폴트위치 <-> 아래위치
        private void UpdateWeaponSwitching()
        {
            //Lerp 계수
            float switchingTimeFactor = 0f;
            if(weaponSwitchDelay == 0f)
            {
                switchingTimeFactor = 1f;
            }
            else
            {
                switchingTimeFactor = Mathf.Clamp01((Time.time - weaponSwitchTimeStarted) / weaponSwitchDelay);
            }

            //타이머 완료 - 
            if(switchingTimeFactor >= 1)
            {
                //내리는 연출 완료
                if (weaponSwitchState == WeaponSwitchState.PutDownPrevious)
                {
                    //현재 무기를 false, 새로운 무기를 true
                    WeaponController oldWeapon = GetActiveWeapon();
                    if(oldWeapon != null)
                    {
                        oldWeapon.ShowWeapon(false);
                    }

                    ActiveWeaponIndex = weaponSwitchNewWeaponIndex;
                    WeaponController newWeapon = GetWeaponAtSlotIndex(weaponSwitchNewWeaponIndex);
                    OnSwitchToWeapon?.Invoke(newWeapon);

                    //연출 초기화
                    switchingTimeFactor = 0f;
                    if(newWeapon != null)
                    {
                        //올라가는 연출 시작
                        weaponSwitchTimeStarted = Time.time;
                        weaponSwitchState = WeaponSwitchState.PutUpNew;
                    }
                    else
                    {
                        weaponSwitchState = WeaponSwitchState.Down;
                    }
                }
                else if (weaponSwitchState == WeaponSwitchState.PutUpNew) //올리는 연출 완료
                {
                    weaponSwitchState = WeaponSwitchState.Up;
                }
            }

            //무기 스위치 이동 연출
            if(weaponSwitchState == WeaponSwitchState.PutDownPrevious)
            {
                weaponMainLocalPosition = Vector3.Lerp(defaultWeaponPosition.localPosition, 
                    downWeaponPosition.localPosition, switchingTimeFactor);
            }
            else if (weaponSwitchState == WeaponSwitchState.PutUpNew)
            {
                weaponMainLocalPosition = Vector3.Lerp(downWeaponPosition.localPosition,
                    defaultWeaponPosition.localPosition, switchingTimeFactor);
            }
        }


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
        //현재 들고 있는 무기 false => 새로운 무기 true
        public void SwitchWeapon(bool ascendingOrder)
        {
            //새로운 무기 인덱스
            int newWeponIndex = -1;
            int closestSlotDistance = weaponSlots.Length;
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if(i != ActiveWeaponIndex && GetWeaponAtSlotIndex(i) != null)
                {
                    //액티브 무기와의 거리 구하기
                    int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(
                        ActiveWeaponIndex, i, ascendingOrder);
                    if (distanceToActiveIndex < closestSlotDistance)
                    {
                        closestSlotDistance = distanceToActiveIndex;
                        newWeponIndex = i;
                    }
                }
            }

            //새로운 무기의 인덱스로 무기 교체
            SwitchWeaponIndex(newWeponIndex);
        }

        //새로운 무기의 인덱스로 무기 교체
        private void SwitchWeaponIndex(int newWeaponIndex)
        {
            //인덱스 체크
            if (newWeaponIndex < 0 || newWeaponIndex >= weaponSlots.Length
                || newWeaponIndex == ActiveWeaponIndex)
                return;

            //무기 교체 연출 초기화
            weaponSwitchNewWeaponIndex = newWeaponIndex;
            weaponSwitchTimeStarted = Time.time;

            //액티브 무기 체크
            if(GetActiveWeapon() == null)
            {
                weaponMainLocalPosition = downWeaponPosition.localPosition;
                weaponSwitchState = WeaponSwitchState.PutUpNew;

                ActiveWeaponIndex = weaponSwitchNewWeaponIndex;
                WeaponController newWeapon = GetWeaponAtSlotIndex(weaponSwitchNewWeaponIndex);
                OnSwitchToWeapon?.Invoke(newWeapon);
            }
            else
            {
                //내리는 연출 시작
                weaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }

        //슬롯간 거리 구하기
        private int GetDistanceBetweenWeaponSlots(int fromIndex, int toIndex, bool ascendingOrder)
        {
            int distance = 0;

            if(ascendingOrder)
            {
                distance = toIndex - fromIndex;
            }
            else
            {
                distance = fromIndex - toIndex;
            }

            if(distance < 0)
            {
                distance = distance + weaponSlots.Length;
            }

            return distance;
        }

        private void OnWeaponSwitched(WeaponController newWeapon)
        {
            if(newWeapon != null)
            {
                newWeapon.ShowWeapon(true);
            }
        }
        #endregion
    }
}