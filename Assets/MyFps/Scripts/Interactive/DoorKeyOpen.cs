using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    //퍼즐 키로 문열기
    public class DoorKeyOpen : Interactive
    {
        #region Variables
        //참조
        public Animator animator;

        //시나리오 텍스트
        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequence = "You need to The Key";
        #endregion

        #region Unity Event Method
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //조건에 따라 문이 안열린다
            if(PlayerDataManager.Instance.HasPuzzleKey(PuzzleKey.ROOM01_KEY))
            {
                OpenedDoor();
            }
            else
            {
                //문이 안열린다
                StartCoroutine(LockedDoor());
            }
        }

        private void OpenedDoor()
        {
            //충돌체 제거
            this.GetComponent<BoxCollider>().enabled = false;

            //문열기 연출, SFX
            animator.SetBool("IsOpen", true);
            AudioManager.Instance.Play("DoorBang");
        }

        IEnumerator LockedDoor()
        {
            //인터랙티브 기능 끄기
            unInteractive = true;

            sequenceText.text = "";
            AudioManager.Instance.Play("DoorLocked");

            yield return new WaitForSeconds(1f);
            sequenceText.text = sequence;

            yield return new WaitForSeconds(2f);
            sequenceText.text = "";

            //인터랙티브 기능 켜기
            unInteractive = false;
        }
        #endregion
    }
}