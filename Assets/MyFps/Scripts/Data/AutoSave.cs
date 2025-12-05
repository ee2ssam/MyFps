using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyFps
{
    /// <summary>
    /// 플레이씬 시작하면 시작하자마자 현재 씬번호를 파일 저장한다
    /// </summary>
    public class AutoSave : MonoBehaviour
    {
        //PlayerPrefs 파라미터
        private const string SceneNumber = "SceneNumber";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //씬 시작하자마자 데이터 저장
            SaveData();
        }

        //데이터 저장하기
        private void SaveData()
        {
            //저장된 번호 가져오기
            //int saveNumber = PlayerPrefs.GetInt(SceneNumber, -1);
            int saveNumber = PlayerStats.Instance.SceneNumber;

            //씬 번호 저장
            int sceneNumber = SceneManager.GetActiveScene().buildIndex;

            if (saveNumber < sceneNumber)
            {
                //저장
                //PlayerPrefs.SetInt(SceneNumber, sceneNumber);
                //Debug.Log($"Save Scene Number: {sceneNumber}");

                PlayerStats.Instance.SetSceneNumnber(sceneNumber);
                SaveLoad.SaveData();
            }
        }
    }
}