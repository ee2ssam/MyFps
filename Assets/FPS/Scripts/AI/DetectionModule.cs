using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Game;
using System.Linq;

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
            //KnownDetectedTarget 체크
            if(KnownDetectedTarget && IsSeeingTarget == false 
                && (Time.time - timeLastSeenTarget) > knownTargetTimeout)
            {
                KnownDetectedTarget = null;
            }

            //피아 구분해서 최소 거리에 있고 DetectionRange 안에 있는 적 찾기
            float sqrDetectionRange = detectingRange * detectingRange;
            IsSeeingTarget = false;
            float closestSqrDistance = Mathf.Infinity;

            foreach (var otherActor in actorManager.Actors)
            {   
                if(otherActor.affiliation != actor.affiliation)
                {
                    //가장 가까운 적을 찾는다
                    float sqrDistance = (otherActor.aimPoint.position - detectingSourcePoint.position).sqrMagnitude;
                    if(sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                    {
                        RaycastHit[] hits = Physics.RaycastAll( detectingSourcePoint.position,
                            (otherActor.aimPoint.position - detectingSourcePoint.position).normalized,
                            detectingRange, -1, QueryTriggerInteraction.Ignore);

                        //가장 가까운 충돌체 찾기
                        RaycastHit closesHit = new RaycastHit();
                        closesHit.distance = Mathf.Infinity;
                        bool foundValidHit = false;             //적을 찾았는지 체크
                        foreach (var hit in hits)
                        {
                            if (hit.distance < closesHit.distance
                                && selfColliders.Contains(hit.collider) == false)
                            {
                                closesHit = hit;
                                foundValidHit = true;
                            }
                        }

                        //최소 거리에 있는 충돌체를 찾았으면 타겟 설정
                        if (foundValidHit)
                        {
                            Actor hitActor = closesHit.collider.GetComponentInParent<Actor>();
                            if(hitActor == otherActor)
                            {
                                IsSeeingTarget = true;
                                closestSqrDistance = sqrDistance;

                                timeLastSeenTarget = Time.time;
                                KnownDetectedTarget = otherActor.aimPoint.gameObject;
                            }
                        }
                    }
                }
            }

            //target을 모르고 있다가 타겟을 발견한 순간
            if(HadKnownTarget == false && KnownDetectedTarget != null)
            {
                OnDetectTarget();
            }

            //target을 알고 있다가 타겟을 잃어 버린 순간
            if(HadKnownTarget == true && KnownDetectedTarget == null)
            {
                OnLostTarget();
            }

            //
            HadKnownTarget = (KnownDetectedTarget != null);
        }

        private void OnDetectTarget()
        {
            onDetectedTarget?.Invoke();
        }

        private void OnLostTarget()
        {
            onLostTarget?.Invoke();
        }
        #endregion
    }
}