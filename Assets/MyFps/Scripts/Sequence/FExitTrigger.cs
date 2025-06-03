using UnityEngine;

namespace MyFps
{
    //트리거가 작동하면 메인 메뉴 보내기
    public class FExitTrigger : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter(Collider other)
        {
            //플레이어 체크
            if(other.tag == "Player")
            {
                SceneClear();
            }
        }

        private void SceneClear()
        {
            //씬 클리어 처리
            AudioManager.Instance.StopBgm();

            //커서제어
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //메인메뉴가기
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
