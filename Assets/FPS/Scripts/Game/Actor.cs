using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 전투 참가자 정의 클래스
    /// </summary>
    public class Actor : MonoBehaviour
    {
        #region Variables        
        public int affiliation;         //소속            
        public Transform aimPoint;      //조준 지점

        private ActorManager actorManager;   //Actor를 관리하는 객체
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            actorManager = GameObject.FindFirstObjectByType<ActorManager>();
        }

        private void Start()
        {
            //ActorManager 등록
            if(actorManager.Actors.Contains(this) == false)
            {
                actorManager.Actors.Add(this);
            }
        }

        private void OnDestroy()
        {
            //ActorManager 등록 해제
            if(actorManager)
            {
                actorManager.Actors.Remove(this);        
            }
        }
        #endregion
    }
}