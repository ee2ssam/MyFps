using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 전투 참가자(Actor)를 관리하는 클래스
    /// </summary>
    public class ActorManager : MonoBehaviour
    {
        #region Property
        public List<Actor> Actors { get; private set; }     //전투 참가자 리스트
        public GameObject Player { get; private set; }      //플레이어 객채
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            Actors = new List<Actor>();
        }
        #endregion

        #region Custom Method
        //플레이어 셋팅
        public void SetPlayer(GameObject player)
        {
            Player = player;
        }
        #endregion
    }
}