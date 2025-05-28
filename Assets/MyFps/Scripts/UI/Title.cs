using UnityEngine;
using System.Collections;

namespace MyFps
{
    //타이틀 씬을 관리하는 클래스 : 3초후에 애니키 보이고 10초 후에 메인메뉴 가기
    public class Title : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";

        private bool isShow = false;    //애니키가 보이냐?
        public GameObject anyKey;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //페이드인 효과
            fader.FadeStart();

            //배경음 플레이
            AudioManager.Instance.PlayBgm("TitleBgm");

            //코루틴 함수 실행
            StartCoroutine(TitleProcess());
        }

        private void Update()
        {
            //애니키가 보인후에 아무키나 누르면 메인메뉴 가기 - old Input
            if(Input.anyKeyDown && isShow)
            {
                StopAllCoroutines();

                AudioManager.Instance.Stop("TitleBgm");
                fader.FadeTo(loadToScene);
            }
        }
        #endregion

        #region Custom Method
        //코루틴 함수 : 3초후에 애니키 보이고 10초 후에 메인메뉴 가기
        IEnumerator TitleProcess()
        {
            yield return new WaitForSeconds(3f);

            isShow = true;
            anyKey.SetActive(true);

            yield return new WaitForSeconds(10f);

            AudioManager.Instance.Stop("TitleBgm");
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
