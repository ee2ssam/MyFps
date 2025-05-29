using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace MyFps
{
    //메인 2번씬 오프닝 클래스
    public class DOpenning : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public GameObject thePlayer;
        //페이더 객체
        public SceneFader fader;

        //시나리오 대사 처리
        public TextMeshProUGUI sequenceText;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //커서 제어
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //시퀀스 플레이
            StartCoroutine(SequencePlay());
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //0.플레이 캐릭터 비 활성화
            //thePlayer.SetActive(false);
            PlayerInput input = thePlayer.GetComponent<PlayerInput>();
            input.enabled = false;

            //1. 페이드인 연출 (1초동안 페인드인 효과)
            fader.FadeStart();

            //2.화면 하단에 시나리오 텍스트 빈값 초기화
            sequenceText.text = "";

            yield return new WaitForSeconds(1f);

            //TODO : Cheating
            //메인 2번 씬 설정
            PlayerDataManager.Instance.Weapon = WeaponType.Pistol;
            PlayerDataManager.Instance.AddAmmo(5);

            //배경음 플레이 시작
            AudioManager.Instance.PlayBgm("Bgm02");

            //0.플레이 캐릭터 활성화
            input.enabled = true;
        }
        #endregion
    }
}
