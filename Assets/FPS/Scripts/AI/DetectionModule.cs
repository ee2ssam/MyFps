using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Game;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 적이 일정거리안에 들어왔는지 체크
    /// </summary>
    public class DetectionModule : MonoBehaviour
    {
        #region Variables
        //참조
        private ActorManager actorManager;

        public Transform detectingSourcePoint;                      //디텍션 오브젝트
        [SerializeField] private float detectingRange = 20f;        //디텍션 거리

        [SerializeField] private float knownTargetTimeout = 4f;     //시야에서 잃어버린 후 타이머 관리
        protected float timeLastSeenTarget = Mathf.NegativeInfinity;    //타겟을 본 마지막 시간

        public UnityAction onDetectedTarget;    //적을 찾았을때 등록된 함수 호출
        public UnityAction onLostTarget;        //적을 잃어버리면 등록된 함수 호출
        #endregion

        #region Property
        public GameObject KnownDetectedTarget { get; private set; } //디텍팅되어 있는 타겟 오브젝트
        public bool HadKnownTarget { get; private set; }    //특정 시점을 찾기위한 was변수
        public bool IsSeeingTarget { get; private set; }    //타겟이 시야에 있는지 체크
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            actorManager = GameObject.FindFirstObjectByType<ActorManager>();
        }
        #endregion

        #region Custom Method
        //디텍팅
        public virtual void HandleTargetDetection(Actor actor, Collider[] selfColliders)
        {

        }
        #endregion
    }
}