using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// 첫번째 트리거 시퀀스 실행
    /// </summary>
    public class BFirstTrigger : MonoBehaviour
    {
        #region Variables
        //참조: 충돌체
        private BoxCollider collider;

        //시퀀스
        //플레이어 오브젝트
        public GameObject thePlayer;

        //시퀀스 UI
        public TextMeshProUGUI sequenceText;
        [SerializeField]
        private string sequecne = "Looks like a weapon on that table";

        //오디오
        public AudioSource line03;

        //화살표
        public GameObject theMarker;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //트리거 시퀀스 플레이
            StartCoroutine(SequencePlay());

            //충돌체 비활성화(또는 킬)
            collider.enabled = false;
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //-플레이 캐릭터 비활성화(플레이 멈춤)
            thePlayer.SetActive(false);

            //-대사 출력: "Looks like a weapon on that table."
            sequenceText.text = sequecne;
            // 오디오 출력
            line03.Play();

            yield return new WaitForSeconds(2f); //- 2초 딜레이

            //-화살표 활성화
            theMarker.SetActive(true);
            yield return new WaitForSeconds(1f); //- 1초 딜레이

            //-초기화
            sequenceText.text = ""; //텍스트 초기화            

            //-플레이 캐릭터 활성화(다시 플레이)
            thePlayer.SetActive(true);
        }
        #endregion
    }
}