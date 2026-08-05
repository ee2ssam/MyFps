using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 전투 참가하는 모든 캐릭터에 부착되는 클래스
    /// </summary>
    public class Actor : MonoBehaviour
    {
        #region Variables
        //참조
        private ActorManager actorManager;

        //소속
        public int affiliation;
        //조준점
        public Transform aimPoint;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            actorManager = GameObject.FindAnyObjectByType<ActorManager>();
            //actor리스트 등록 - 중복 등록 체크
            if(actorManager && actorManager.Actors.Contains(this) == false)
            {
                actorManager.Actors.Add(this);
            }
        }

        //킬
        private void OnDestroy()
        {
            //actor리스트 삭제
            if (actorManager)
            {
                actorManager.Actors.Remove(this);
            }
        }
        #endregion
    }
}
