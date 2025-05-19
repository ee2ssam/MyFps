using UnityEngine;
using TMPro;

namespace MyFps
{
    //문열기 인터렉티브 액션 구현
    public class DoorCellOpen : MonoBehaviour
    {
        #region Variables
        //문과 플레이어와의 거리
        private float theDistance;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        //크로스헤어
        public GameObject extraCross;

        [SerializeField]
        private string action = "Open The Door";

        //애니메이션
        public Animator animator;

        //애니 파라미터 스트링
        private string  paramIsOpen = "IsOpen";
        #endregion

        #region Unity Event Method

        private void Update()
        {
            //문과 플레이어와의 거리 가져오기
            theDistance = PlayerCasting.distanceFromTarget;
        }

        private void OnMouseOver()
        {
            extraCross.SetActive(true);

            if (theDistance <= 2f)
            {
                ShowActionUI();

                //TODO : New Input System 대체 구현
                //키입력 체크
                if(Input.GetKeyDown(KeyCode.E))
                {
                    //UI 숨기기, 문열고, 충돌체 제거
                    HideActionUI();
                    animator.SetBool(paramIsOpen, true);        //문 여는 애니메이션 연출
                    this.GetComponent<BoxCollider>().enabled = false; //문 충돌체 제거
                }
            }
            else
            {
                HideActionUI();
            }
        }

        private void OnMouseExit()
        {
            extraCross.SetActive(false);

            HideActionUI();
        }
        #endregion

        #region Custom Method
        //Action UI 보여주기
        private void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;
        }

        //Action UI 숨기기
        private void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }
        #endregion
    }
}
