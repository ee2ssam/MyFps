using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 아이템 줍기
    /// </summary>
    public class PickupItem : Interactive
    {
        #region Variables
        //인터랙티브 UI
        private InteractiveUI interactiveUI;
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            interactiveUI = FindFirstObjectByType<InteractiveUI>();
        }
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //Empty
        }

        protected override void ShowActionUI()
        {
            interactiveUI.ShowActionUI(action);
        }

        protected override void HideActionUI()
        {
            interactiveUI.HideActionUI();
        }
        #endregion
    }
}