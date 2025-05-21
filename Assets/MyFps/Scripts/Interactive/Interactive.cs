using TMPro;
using UnityEngine;

namespace MyFps
{
    //인터랙티브 액션의 부모 클래스
    public class Interactive : MonoBehaviour
    {
        #region Variables
        //문과 플레이어와의 거리
        protected float theDistance;

        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        //크로스헤어
        public GameObject extraCross;

        [SerializeField]
        protected string action = "Do Interactive Action";
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //
                    extraCross.SetActive(false);

                    //UI 숨기기
                    HideActionUI();

                    //액션
                    DoAction();
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
        protected void ShowActionUI()
        {
            actionUI.SetActive(true);
            actionText.text = action;
        }

        //Action UI 숨기기
        protected void HideActionUI()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }

        //액션 함수
        protected virtual void DoAction()
        {

        }
        #endregion
    }
}
