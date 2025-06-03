using UnityEngine;
using TMPro;
using System.Collections;

namespace MyFps
{
    //퍼즐 조각을 모두 모우면 비밀의 문이 열린다
    public class FullEyeExit : Interactive
    {
        #region Variables
        //액자
        public GameObject fakePicture;  //빈 액자
        public GameObject realPicture;  //눈 그림이 있는 액자

        //숨겨진 벽
        public Animator animator;
        [SerializeField]
        private string openTrigger = "ExitOpen";

        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequence = "You need more Eye Pictures";
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //퍼즐조각 2개를 모았는지 체크
            if(PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.LEFTEYE_KEY)
                && PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.RIGHTEYE_KEY))
            {
                OpenHiddenWall();
            }
            else //조각이 모자르면
            {
                StartCoroutine(LockHiddenWall());
            }
        }

        private void OpenHiddenWall()
        {
            //액자
            fakePicture.SetActive(false);
            realPicture.SetActive(true);

            //숨겨진 벽
            animator.SetTrigger(openTrigger);
        }

        IEnumerator LockHiddenWall()
        {
            unInteractive = true;
            sequenceText.text = sequence;

            yield return new WaitForSeconds(2f);

            unInteractive = false;
            sequenceText.text = "";
        }
        #endregion
    }
}