using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

namespace MyFps
{
    //인트로 연출 구현
    public class Intro : MonoBehaviour
    {
        #region Variables
        //참조
        public SceneFader fader;        
        [SerializeField] private string loadToScene = "MainScene01";

        //이동
        public CinemachineSplineCart cart;  //돌리 카트
        private SplineAutoDolly.FixedSpeed dolly;

        private bool[] isArrive;            //이동 포인트 지점에 도착했는지 여부 체크
        [SerializeField] private int wayPointIndex;          //다음 이동 목표 지점

        //연출
        public Animator animator;
        public GameObject introUI;
        public GameObject theShedLight;

        [SerializeField] private string aroundTrigger = "Around";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            isArrive = new bool[5];
            wayPointIndex = 0;

            dolly = cart.AutomaticDolly.Method as SplineAutoDolly.FixedSpeed;
            dolly.Speed = 0f;

            //시퀀스
            StartCoroutine(PlayStartSequence());
        }

        private void Update()
        {
            //도착 판정, 1회 판정
            if(cart.SplinePosition >= wayPointIndex && isArrive[wayPointIndex] == false)
            {
                Arrive();
            }

            //스킵
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GotoPlayScene();
            }
        }
        #endregion

        #region Custom Method
        //목표 지점 도착
        private void Arrive()
        {
            //마지막 엔드 포인트 지점 도착 체크
            if(wayPointIndex == isArrive.Length - 1)
            {
                StartCoroutine(PlayEndSequence());
            }
            else
            {
                StartCoroutine(PlayStaySequence());
            }   
        }

        //플레이씬 가기
        private void GotoPlayScene()
        {
            //코루틴 종료
            StopAllCoroutines();

            //배경음 종료
            AudioManager.Instance.StopBgm();

            //다음씬 가기
            fader.FadeTo(loadToScene);
        }

        //시작 시퀀스 : 페이드인 효과, 두리번 거리기, 카메라 이동 시작
        IEnumerator PlayStartSequence()
        {
            isArrive[0] = true;

            //페이드인 효과
            fader.FadeStart();
            //배경음 시작
            AudioManager.Instance.PlayBgm("IntroBgm");
            yield return new WaitForSeconds(1f);
            
            //둘러보기
            animator.SetTrigger(aroundTrigger);
            yield return new WaitForSeconds(4f);

            //이동 시작
            wayPointIndex = 1;  //다음 목표지점 설정
            dolly.Speed = 0.05f;
        }

        //이동 포인트 지점 도착 시퀀스
        IEnumerator PlayStaySequence()
        {
            //-1번 포인트: 두리번 거리기, 인트로 UI 보이기
            //-2번 포인트: 두리번 거리기, 인트로 UI 숨기기
            //-3번 포인트 : 두리번 거리기,  오두막 앞에서 오두막 라이트 켜기

            //도착 체크
            isArrive[wayPointIndex] = true;

            //이동 멈춤
            dolly.Speed = 0f;

            //둘러보기
            animator.SetTrigger(aroundTrigger);
            yield return new WaitForSeconds(4f);

            switch(wayPointIndex)
            {
                case 1:
                    introUI.SetActive(true);
                    dolly.Speed = 0.05f;
                    break;
                case 2:
                    introUI.SetActive(false);
                    dolly.Speed = 0.05f;
                    break;
                case 3:
                    theShedLight.SetActive(true);
                    yield return new WaitForSeconds(1f);

                    dolly.Speed = 0.15f;
                    break;
            }

            //이동 시작
            wayPointIndex++;  //다음 목표지점 설정
                              
        }

        //최종지점 도착 시퀀스 : 오두막 라이트 끄고 다음 씬으로 이동, 배경음 종료
        IEnumerator PlayEndSequence()
        {
            //도착 체크
            isArrive[wayPointIndex] = true;

            //이동 멈춤
            dolly.Speed = 0f;

            yield return new WaitForSeconds(2f);

            //오두막 라이트 끄고
            theShedLight.SetActive(false);

            yield return new WaitForSeconds(1f);

            //배경음 종료
            AudioManager.Instance.StopBgm();

            //다음씬 가기
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}