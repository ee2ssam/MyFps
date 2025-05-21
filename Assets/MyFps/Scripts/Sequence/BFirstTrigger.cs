using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    //첫번째 트리거 연출
    public class BFirstTrigger : MonoBehaviour
    {
        #region Variables
        public GameObject thePlayer;

        //화살표
        public GameObject theArrow;
        
        //시나리오 대사 처리
        public TextMeshProUGUI sequenceText;

        [SerializeField]
        private string sequence = "Looks like a weapon on that table";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //플레이어 체크
            if (other.tag == "Player")
            {
                //트리거 해제
                this.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(SequencePlayer());
            }
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlayer()
        {
            //플레이 캐릭터 비활성화  (플레이 멈춤)
            thePlayer.SetActive(false);

            //대사 출력 :  "Looks like a weapon on that table."
            sequenceText.text = sequence;

            //1초 딜레이
            yield return new WaitForSeconds(1f);

            //화살표 활성화
            theArrow.SetActive(true);

            //2초 딜레이
            yield return new WaitForSeconds(2f);

            sequenceText.text = "";
            //플레이 캐릭터 활성화
            thePlayer.SetActive(true);

        }
        #endregion
    }
}
