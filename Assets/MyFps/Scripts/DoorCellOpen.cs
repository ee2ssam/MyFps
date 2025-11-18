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
        //액션 UI
        public GameObject actionUI;
        public TextMeshProUGUI actionText;

        [SerializeField]
        private string action = "Open The Door";
        #endregion

        #region Unity Event Method
        private void OnMouseOver()
        {
            actionUI.SetActive(true);
            actionText.text = action;

            //만약 Action 버튼을 누르면

        }

        private void OnMouseExit()
        {
            actionUI.SetActive(false);
            actionText.text = "";
        }
        #endregion
    }
}