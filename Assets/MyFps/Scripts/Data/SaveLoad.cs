using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;   //이진화

namespace MyFps
{
    //게임 데이터를 파일에 저장/가져오기 기능 구현 - 이진화 포맷 적용
    public class SaveLoad
    {
        //저장하기
        public static void SaveData()
        {
            //파일 이름, 경로 지정
            string path = Application.persistentDataPath + "/pData.arr";

            //저장할 데이터를 이진화 준비
            BinaryFormatter formatter = new BinaryFormatter();

            //파일 접근 - FileMode.Create : 존재하면 파일 가져오기 없으면 새로 만들라
            FileStream fs = new FileStream(path, FileMode.Create);

            //저장할 데이터를 준비 : 생성자를 통해 세이브 데이터 셋팅
            PlayData playData = new PlayData();
            Debug.Log($"Save SceneNumber: {playData.sceneNumber}");

            //준비한 데이터를 이진화 저장
            formatter.Serialize(fs, playData);

            //파일에 접근하면 항상 파일을 닫아주어야 한다
            fs.Close();
        }

        //가져오기 - 저장된 데이터를 반환값으로 받아온다
        public static PlayData LoadData()
        {
            //가져오는 데이터
            PlayData playData = null;

            //파일 이름, 경로 지정
            string path = Application.persistentDataPath + "/pData.arr";

            //지정된 경로에 지정된 파일 있는지 없는지 체크
            if(File.Exists(path) == true)
            {
                //이진화 데이터를 가져올 준비
                BinaryFormatter formatter = new BinaryFormatter();

                //파일 접근
                FileStream fs = new FileStream(path, FileMode.Open);

                //이진화로 포맷으로 파일에 저장된 데이터를 역진화해서 가져온다
                playData = formatter.Deserialize(fs) as PlayData;
                Debug.Log($"Load SceneNumber: {playData.sceneNumber}");

                //파일에 접근하면 항상 파일을 닫아주어야 한다
                fs.Close();
            }
            else
            {
                Debug.Log("Not Found Load File");
            }

            return playData;
        }
    }
}
