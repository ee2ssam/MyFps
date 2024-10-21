using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFps
{
    public class Title : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField] private string loadToScene = "MainMenu";

        private bool isAnyKey = false;
        public GameObject anykeyUI;
        #endregion

        private void Start()
        {
            //페이드인 효과
            fader.FromFade();

            //초기화
            isAnyKey = false;

            StartCoroutine(TitleProcess());
        }

        private void Update()
        {
            if(Input.anyKey && isAnyKey)
            {
                GotoMenu();
            }
        }

        //3초뒤에 anykey Show, 10초 뒤에 자동 넘김
        IEnumerator TitleProcess()
        {
            yield return new WaitForSeconds(4f);
            isAnyKey = true;
            anykeyUI.SetActive(true);

            yield return new WaitForSeconds(10f);
            GotoMenu();
        }


        private void GotoMenu()
        {
            StopAllCoroutines();

            fader.FadeTo(loadToScene);
        }
    }
}