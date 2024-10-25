using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyFps
{
    public class PickupLeftEye : Interactive
    {
        #region Variables
        //퍼즐UI
        public GameObject puzzleUI;
        public Image itemImage;
        public TextMeshProUGUI puzzleText;

        public GameObject puzzleItemGp;

        public Sprite itemSprite;                                   //획득한 아이템 아이콘
        [SerializeField] private string puzzleStr = "Puzzle Text";  //아이템 획득 안내 텍스트
        #endregion

        protected override void DoAction()
        {
            StartCoroutine(GainPuzzleItem());
        }

        IEnumerator GainPuzzleItem()
        {
            //LEFTEYE 퍼즐 아이템 획득
            PlayerStats.Instance.AcquirePuzzleItem(PuzzleKey.LEFTEYE_KEY);

            //UI연출
            if(puzzleUI != null)
            {
                //아이템 트리거 비활성
                this.GetComponent<BoxCollider>().enabled = false;
                puzzleItemGp.SetActive(false);

                puzzleUI.SetActive(true);
                itemImage.sprite = itemSprite;
                puzzleText.text = puzzleStr;

                yield return new WaitForSeconds(2f);
                puzzleUI.SetActive(false);
            }

            //아이템 트리거 킬
            Destroy(gameObject);
        }
    }
}