using UnityEngine;
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

        private Collider doorCollider;
        private Mouse mouse;
        private bool isMouseOverDoor = false;
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
        }

        private void Update()
        {
            if (mouse == null) return;

            //플레이어의 캐스팅 거리가 체크
            if (PlayerCasting.DistanceFromTarget > 2f)
            {
                HideActionUI();
                isMouseOverDoor = false;
                return;
            }

            // 마우스 위치에서 raycast 수행
            Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
            bool hitDoor = Physics.Raycast(ray, out RaycastHit hit);

            // 이 오브젝트의 collider를 맞았는지 확인
            bool mouseOverDoor = hitDoor && hit.collider.gameObject == gameObject;

            // 상태 변화 감지            
            if (mouseOverDoor && !isMouseOverDoor)
            {
                // 마우스가 들어옴
                ShowActionUI();
                isMouseOverDoor = true;
            }
            else if (!mouseOverDoor && isMouseOverDoor)
            {
                // 마우스가 나감
                HideActionUI();
                isMouseOverDoor = false;
            }
        }
        #endregion

        #region Custom Method
        void ShowActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(true);
            }
        }

        void HideActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(false);
            }
        }
        #endregion
    }
}