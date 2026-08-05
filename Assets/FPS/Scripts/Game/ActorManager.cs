using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    /// <summary>
    /// Actor를 관리하는 클래스
    /// </summary>
    public class ActorManager : MonoBehaviour
    {
        public List<Actor> Actors {  get; set; }
        public GameObject Player { get; private set; }


        private void Awake()
        {
            //액터 리스트 생성
            Actors = new List<Actor>();
        }

        public void SetPlayer(GameObject player)
        {
            Player = player;
        }

    }
}