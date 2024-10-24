using UnityEngine;

namespace MyFps
{
    public class PickupKey : Interactive
    {
        #region Variables
        #endregion

        protected override void DoAction()
        {
            //key Item 저장
            PlayerStats.Instance.AcquirePuzzleItem(PuzzleKey.ROOM01_KEY);

            //킬
            Destroy(gameObject);
        }
    }
}
