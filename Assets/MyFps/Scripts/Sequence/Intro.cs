using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 인트로 시퀀스
    /// 페이드 인, 카메라 애니메이션, UI 활성화, 라이트 꺼짐, 페이드 아웃
    /// </summary>
    public class Intro : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "PlayScene01";

        public GameObject introUI;
        public GameObject lights;

        public CinemachineSplineCart cart;
        private bool isArrive = false;      //카트 도착 여부
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //페이드인
            fader.FadeStart();
            AudioManager.Instance.PlayBGM("IntroBgm");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && isArrive == false)
            {
                isArrive = true;
                StartCoroutine(ExitIntro());
                return;
            }

            //cart 포지션 체크
            if (isArrive == false)
            {
                if(cart.SplinePosition >= 1)
                {
                    isArrive = true;
                    StartCoroutine(ExitIntro());
                }
                else if (cart.SplinePosition >= 0.6f)
                {
                    if (introUI.activeSelf == true)
                        introUI.SetActive(false);
                }
                else if (cart.SplinePosition >= 0.4f)
                {
                    if(introUI.activeSelf == false)
                        introUI.SetActive(true);
                }
            }
        }
        #endregion

        #region Custom Method
        IEnumerator ExitIntro()
        {
            yield return new WaitForSeconds(2f);

            lights.SetActive(false);
            yield return new WaitForSeconds(1f);

            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
