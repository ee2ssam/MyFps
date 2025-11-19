using UnityEngine;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// 플레이어와 인터랙션 기능 오브젝트
    /// 인터랙티브 : 마우스를 가져가면 UI활성화 빼면 UI 비활성화
    /// 인터랙션 기능 : 도어 오픈
    /// </summary>
    public class DoorCellOpen : MonoBehaviour
    {
        #region Varibles
        //크로스헤어
        public GameObject extraCross;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        [SerializeField]
        private string action = "Open The Door";

        //액션
        public Animator animator;
        private BoxCollider collider;

        //애니메이터 파라미터
        const string Open = "Open";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            collider = GetComponent<BoxCollider>();
        }

        private void OnMouseOver()
        {
            if(PlayerCasting.distanceFromTarget > 2f)
            {
                HideActionUI();
                return;
            }

            ShowActionUI();

            //만약 Action 버튼을 누르면
            if (Input.GetButtonDown("Action"))
            {
                OpenDoor();
            }
        }

        private void OnMouseExit()
        {
            HideActionUI();
        }
        #endregion

        #region Custom Method
        private void ShowActionUI()
        {
            extraCross.SetActive(true);
            actionUI.SetActive(true);
            actionText.text = action;
        }

        private void HideActionUI()
        {
            extraCross.SetActive(false);
            actionUI.SetActive(false);
            actionText.text = "";
        }

        void OpenDoor()
        {
            //액션 UI 감추기
            HideActionUI();

            //애니메이션
            animator.SetTrigger(Open);

            //충돌체 기능 제거
            collider.enabled = false;
        }
        #endregion
    }
}