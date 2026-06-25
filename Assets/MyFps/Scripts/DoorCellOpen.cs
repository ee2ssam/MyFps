using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace MyFps
{
    /// <summary>
    /// 플레이어와 인터랙티브 기능 구현
    /// 가까이 가서 마우스 가져가면 액션 UI 보여준다
    /// 액션 : 문을 연다
    /// </summary>
    public class DoorCellOpen : MonoBehaviour
    {
        #region Variables
        //UI 오브젝트
        public GameObject actionUI;
        public GameObject extraCross;
        public TextMeshProUGUI actionText;

        private Collider doorCollider;
        private Mouse mouse;

        private bool mouseOverDoor = false;         //현재 상태
        private bool wasMouseOverDoor = false;      //이전 상태

        //인터랙브 액션
        public InputActionReference interactAction;
        public string action = "action Text";       //인터랙티브 액션 내용

        public Animator animator;
        private string isOpen = "IsOpen";

        public AudioSource audioSource;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            doorCollider = GetComponent<Collider>();
            if (doorCollider == null)
            {
                Debug.LogError("DoorCellOpen: Collider component not found!");
            }
        }

        private void OnEnable()
        {
            mouse = Mouse.current;
            interactAction.action.Enable();
        }

        private void OnDisable()
        {
            interactAction.action.Disable();
        }

        private void Update()
        {
            if (mouse == null) return;

            //플레이어의 캐스팅 거리가 체크
            if (PlayerCasting.DistanceFromTarget > 2f)
            {
                HideActionUI();
                wasMouseOverDoor = false;
                return;
            }

            // 마우스 위치에서 raycast 수행
            Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
            bool hitDoor = Physics.Raycast(ray, out RaycastHit hit);

            // 이 오브젝트의 collider를 맞았는지 확인
            mouseOverDoor = hitDoor && hit.collider.gameObject == gameObject;

            // 상태 변화 감지            
            if (mouseOverDoor != wasMouseOverDoor)
            {
                if(mouseOverDoor)
                {
                    // 마우스가 들어옴
                    ShowActionUI();
                }
                else
                {
                    // 마우스가 나감
                    HideActionUI();
                }
            }

            if(mouseOverDoor && interactAction.action.WasPressedThisFrame())
            {
                DoAction();
            }

            //was 상태 저장
            wasMouseOverDoor = mouseOverDoor;
        }
        #endregion

        #region Custom Method
        void DoAction()
        {
            //인터랙티브 액션 - open the door
            animator.SetBool(isOpen, true);

            //사운드 플레이, AudioSource null 체크
            if (audioSource)
            {
                audioSource.Play();
            }

            //초기화
            HideActionUI();
            doorCollider.enabled = false;
        }

        void ShowActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(true);
                extraCross.SetActive(true);
                actionText.text = action;
            }
        }

        void HideActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(false);
                extraCross.SetActive(false);
                actionText.text = "";
            }
        }
        #endregion
    }
}