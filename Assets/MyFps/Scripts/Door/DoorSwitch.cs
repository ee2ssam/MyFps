using UnityEngine;
using System.Collections;


namespace MyFps
{
    /// <summary>
    /// 등록된 문의 열기, 닫기 구현
    /// 인터랙티브 액션으로 이벤트 구현, 인터랙티브 상속 받는다
    /// </summary>
    public class DoorSwitch : Interactive
    {
        #region Variables
        public Door door;       //문닫기, 열기할 문 게임오브젝트

        public Renderer renderer;           //스위치를 그리는 랜더러
        public Material closeMaterial;      //닫을때 스위치 컬러
        private Material originMaterial;    //열을때 스위치 컬러
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            door.OnActivate += DoorOpen;
            door.OnDeactivate += DoorClose;
        }

        private void OnDisable()
        {
            door.OnActivate -= DoorOpen;
            door.OnDeactivate -= DoorClose;
        }

        protected void Start()
        {
            //초기화
            originMaterial = renderer.material;
        }
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            StartCoroutine(Toggle());
        }

        IEnumerator Toggle()
        {
            //문 열고 닫기
            if(door.IsActive)
            {
                door.Deactivate();
            }
            else
            {
                door.Activate();
            }

            yield return new WaitForSeconds(1f);

            //충돌체 복구
            collider.enabled = true;
        }

        void DoorOpen()
        {
            action = "Close The Door";
            renderer.material = closeMaterial;
        }

        void DoorClose()
        {
            action = "Open The Door";
            renderer.material = originMaterial;
        }
        #endregion

    }
}
