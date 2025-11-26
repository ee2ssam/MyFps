using UnityEngine;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// 인터랙티브 UI 관리 (show, hide) 하는 클래스
    /// </summary>
    public class InteractiveUI : MonoBehaviour
    {
        #region Variablse
        //인터랙티브 UI 오브젝트
        public GameObject extraCross;
        public GameObject actionUI;
        public TextMeshProUGUI actionText;
        #endregion

        #region Custom Method
        public void ShowActionUI(string action)
        {
            extraCross.SetActive(true);
            actionUI.SetActive(true);
            actionText.text = action;
        }

        public void HideActionUI()
        {
            extraCross.SetActive(false);
            actionUI.SetActive(false);
            actionText.text = "";
        }
        #endregion
    }
}