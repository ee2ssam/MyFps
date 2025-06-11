using UnityEngine;
using System;

namespace MyFps
{
    //파일에 저장할 게임 플레이 데이터 목록
    [Serializable]
    public class PlayData
    {
        public int sceneNumber;
        public int ammoCount;
        public float playerHealth;

        //etc.....

        //생성자 : 멤버 변수 초기화
        public PlayData()
        {
            sceneNumber = PlayerDataManager.Instance.SceneNumber;
            ammoCount = PlayerDataManager.Instance.AmmoCount;
            playerHealth = PlayerDataManager.Instance.PlayerHealth;

            //etc...
        }
    }
}
