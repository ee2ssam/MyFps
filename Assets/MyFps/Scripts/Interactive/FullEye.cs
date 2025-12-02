using UnityEngine;
using TMPro;
using System.Collections;

namespace MyFps
{
    public class FullEye : Interactive
    {
        #region Variables
        public GameObject leftEye;      //액자 퍼즐 조각
        public GameObject rightEye;     //액자 퍼즐 조각
        public GameObject doorSwitch;   //숨겨진 도어 스위치

        //실패 메세지 UI
        public TextMeshProUGUI sequenceText;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            StartCoroutine(PutThePuzzlePieces());
        }

        //퍼즐 조각 맞추기
        IEnumerator PutThePuzzlePieces()
        {
            bool isLeft = PlayerStats.Instance.HavePuzzleItem(PuzzleItem.LeftEye);
            bool isRigth = PlayerStats.Instance.HavePuzzleItem(PuzzleItem.RightEye);

            //퍼즐 조각 맞추기
            if (isLeft)
            {
                leftEye.SetActive(true);
            }
            if (isRigth)
            {
                rightEye.SetActive(true);
            }

            //모든 퍼즐 조각을 다 맞추었는지 체크
            if(isLeft && isRigth)
            {
                doorSwitch.SetActive(true);
            }
            else // 실패
            {
                sequenceText.text = "Need more puzzle pieces";
                yield return new WaitForSeconds(2f);
                sequenceText.text = "";

                //충돌체 복구
                collider.enabled = true;
            }
        }
        #endregion
    }
}