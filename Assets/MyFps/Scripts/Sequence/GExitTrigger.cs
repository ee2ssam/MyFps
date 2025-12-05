using UnityEngine;
using System.Collections;

namespace MyFps
{
    public class GExitTrigger : MonoBehaviour
    {
        #region Variables
        private BoxCollider collider;

        //씬 이동
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(SequencePlay());

            //충돌체 비활성화(또는 킬)
            collider.enabled = false;
        }
        #endregion

        #region Custom Method
        IEnumerator SequencePlay()
        {
            //배경음 종료
            AudioManager.Instance.StopBGM();

            //씬 종료시 구현 내용
            //플레이 데이터 저장
            SavePlayData();

            yield return new WaitForSeconds(0.1f);

            fader.FadeTo(loadToScene);
        }

        private void SavePlayData()
        {
            //저장할 데이터 세팅
            PlayerStats.Instance.SetSceneName(loadToScene);
            SaveLoad.SaveData();
        }
        #endregion
    }
}
