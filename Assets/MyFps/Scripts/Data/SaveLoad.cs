using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyFps
{
    /// <summary>
    /// 게임 플레이 데이터 (파일 시스템으로) 저장, 로드하기 - 이진화 저장
    /// </summary>
    public static class SaveLoad
    {
        //파일 저장하기
        public static void SaveData()
        {
            //파일이름, 저장 경로 지정
            string path = Application.persistentDataPath + "/pData.dat";

            //저장할 데이터를 이진화 준비
            BinaryFormatter formatter = new BinaryFormatter();

            //파일 접근 : 존재하면 파일 가져오고 없으면 새로 만들고 => FileMode.Create
            FileStream fs = new FileStream(path, FileMode.Create);

            //저장할 데이터를 준비
            PlayData playData = new PlayData();
            Debug.Log($"Save sceneName: {playData.sceneName}");

            //준비한 데이터를 이진화 저장
            formatter.Serialize(fs, playData);

            //파일 클로즈
            fs.Close();

        }

        //파일 로드하기
        public static PlayData LoadData()
        {
            //파일에서 데이터를 로드해서 저장할 변수
            PlayData playData;

            //파일이름, 저장 경로 지정
            string path = Application.persistentDataPath + "/pData.dat";

            //지정된 경로에 파일이 있는지 없는지 체크
            if(File.Exists(path) == true)
            {
                //파일 있음
                //로드할 데이터를 역(이)진화 준비
                BinaryFormatter formatter = new BinaryFormatter();

                //파일 접근
                FileStream fs = new FileStream(path, FileMode.Open);

                //이진화해서 저장된 데이터를 역이진화해서 (PlayData)형식으로 읽어온다
                playData = formatter.Deserialize(fs) as PlayData;
                Debug.Log($"Load sceneName: {playData.sceneName}");

                //파일 클로즈
                fs.Close();
            }
            else
            {
                //파일 없음
                Debug.Log($"Cannot Found Load File");
                playData = null;
            }

            return playData;
        }
    }
}