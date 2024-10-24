using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyFps
{
    public class DoorKeyOpen : Interactive
    {
        #region Variables
        public TextMeshProUGUI textBox;
        [SerializeField]
        private string sequence = "You need the Key";
        #endregion

        protected override void DoAction()
        {
            //문열기
            if (PlayerStats.Instance.HasPuzzleItem(PuzzleKey.ROOM01_KEY))
            {
                OpenDoor();
            }
            else
            {
                StartCoroutine(LockedDoor());
            }
        }

        void OpenDoor()
        {
            this.GetComponent<BoxCollider>().enabled = false;

            this.GetComponent<Animator>().SetBool("IsOpen", true);
            AudioManager.Instance.Play("DoorOpen");
        }

        IEnumerator LockedDoor()
        {
            //문 잠긴 소리
            unInteractive = true;   //인터랙티브 기능 정지
            AudioManager.Instance.Play("DoorLocked");

            yield return new WaitForSeconds(1f);

            textBox.gameObject.SetActive(true);
            textBox.text = sequence;

            yield return new WaitForSeconds(2f);
            
            textBox.gameObject.SetActive(false);
            textBox.text = "";

            unInteractive = false;   //인터랙티브 기능 복원
        }
    }
}
