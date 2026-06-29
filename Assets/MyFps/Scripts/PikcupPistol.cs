using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFps
{
    /// <summary>
    /// 인터랙티브 구현
    /// 가까이 가서 crosshair 캐스팅하면 액션 UI 보여준다
    /// 액션 : 권총 아이템 획득
    /// </summary>
    public class PikcupPistol : MonoBehaviour
    {
        #region Variables
        [Header("Interative")]     //헤더 특성
        //UI 오브젝트
        public GameObject actionUI;
        public GameObject extraCross;
        public TextMeshProUGUI actionText;
        
        public InputActionReference interactAction;
        public string action = "action Text";       //인터랙티브 액션 내용

        private Collider castCollider;
        private bool currentCasting = false;        //현재 캐스팅 상태
        private bool wasCasting = false;            //이전 캐스팅 상태

        [Header("Action")]        
        public GameObject realPistol;
        public GameObject arrow;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            castCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            interactAction.action.Enable();
        }

        private void OnDisable()
        {
            interactAction.action.Disable();
        }

        private void Update()
        {
            //플레이어의 캐스팅 거리가 체크
            if (PlayerCasting.DistanceFromTarget > 2f)
            {
                HideActionUI();
                wasCasting = false;
                return;
            }

            // 이 오브젝트의 캐스팅한 오브젝트인 체크
            currentCasting = PlayerCasting.CastGameObject != null && PlayerCasting.CastGameObject == this.gameObject;

            // 상태 변화 감지: 경계
            if (currentCasting != wasCasting && currentCasting == true)
            {
                //캐스팅하고 있지 않다가 캐스팅을 시작할때
                ShowActionUI();
            }
            else if (currentCasting != wasCasting && currentCasting == false)
            {
                //캐스팅 하고 있다가 캐스팅을 놓치는것을 시작할때
                HideActionUI();
            }

            if (currentCasting && interactAction.action.WasPressedThisFrame())
            {
                DoAction();
            }

            //was 상태 저장
            wasCasting = currentCasting;
        }
        #endregion


        #region Custom Method
        void DoAction()
        {
            //- 오른손 쪽의 총은 화면 출력 -활성화
            //- 책상위의 가이드 화살표는 없어진다
            //-테이블 위의 총은 없어지고 - 비활성화
            //- 다시 캐스팅해도 트리거가 작동이 안되어야 한다

            realPistol.SetActive(true);

            //arrow.SetActive(false);
            //this.gameObject.SetActive(false); //fakePistol
            //castCollider.enabled = false;

            Destroy(arrow);
            Destroy(this.gameObject);
        }

        void ShowActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(true);
                extraCross.SetActive(true);
                actionText.text = action;
            }
        }

        void HideActionUI()
        {
            if (actionUI != null)
            {
                actionUI.SetActive(false);
                extraCross.SetActive(false);
                actionText.text = "";
            }
        }
        #endregion
    }
}