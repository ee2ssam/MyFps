using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace MyFps
{
    //레프트 Eye 퍼즐아이템 획득
    public class PickupLeftEye : Interactive
    {
        #region Variables
        //퍼즐 아이템
        [SerializeField]
        private PuzzleKey puzzleKey = PuzzleKey.LEFTEYE_KEY;

        //퍼즐 UI
        public GameObject puzzleUI;
        public Image puzzleImage;

        public Sprite puzzleSprite;

        //퍼즐 대사
        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequence = "You have obtained a puzzle item";

        //숨겨진 벽
        public GameObject fakeWall;
        public GameObject hiddenWall;
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            StartCoroutine(ShowPuzzleUI());
        }

        IEnumerator ShowPuzzleUI()
        {
            //Debug.Log("레프트 Eye 퍼즐아이템 획득");
            PlayerDataManager.Instance.GainPuzzleKey(puzzleKey);

            //숨겨진 벽 체크
            if(PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.LEFTEYE_KEY)
                && PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.RIGHTEYE_KEY))
            {
                fakeWall.SetActive(false);
                hiddenWall.SetActive(true);
            }

            //인터랙티브 기능 제거
            unInteractive = true;

            //UI 연출            
            if (puzzleUI && puzzleSprite)
            {
                sequenceText.text = "";

                puzzleUI.SetActive(true);
                puzzleImage.sprite = puzzleSprite;

                yield return new WaitForSeconds(0.3f);
            }

            sequenceText.text = sequence;
            yield return new WaitForSeconds(1.7f);

            //값 초기화
            sequenceText.text = "";
            puzzleUI.SetActive(false);

            //트리거 오브젝트 킬
            Destroy(gameObject);
        }
        #endregion
    }
}
