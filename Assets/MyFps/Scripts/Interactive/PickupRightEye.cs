using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    public class PickupRightEye : PickupPuzzleItem
    {
        #region Variables
        public GameObject fakeWall;
        public GameObject exitWall;
        #endregion

        protected override void DoAction()
        {
            StartCoroutine(GainPuzzleItem());

            ShowExitWall();
        }

        private void ShowExitWall()
        {
            //출구 보이기            
            if(PlayerStats.Instance.HasPuzzleItem(PuzzleKey.LEFTEYE_KEY)
                && PlayerStats.Instance.HasPuzzleItem(PuzzleKey.RIGHTEYE_KEY))
            {
                fakeWall.SetActive(false);
                exitWall.SetActive(true);
            }
        }
    }
}
