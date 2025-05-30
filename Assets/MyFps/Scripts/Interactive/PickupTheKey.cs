using UnityEngine;

namespace MyFps
{
    public class PickupTheKey : Interactive
    {
        #region Variables
        [SerializeField]
        private PuzzleKey puzzleKey = PuzzleKey.ROOM01_KEY;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //퍼즐 아이템(key) 획득
            PlayerDataManager.Instance.GainPuzzleKey(puzzleKey);

            //아이템 제거 : 트리거 비활성화, 킬
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        #endregion
    }
}