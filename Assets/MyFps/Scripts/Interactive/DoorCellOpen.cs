using UnityEngine;
using TMPro;

namespace MyFps
{
    /// <summary>
    /// 플레이어와 인터랙션 기능 오브젝트
    /// 인터랙티브 : 마우스를 가져가면 UI활성화 빼면 UI 비활성화
    /// 인터랙션 기능 : 도어 오픈
    /// </summary>
    public class DoorCellOpen : Interactive
    {
        #region Varibles                
        //액션
        [Header("Interactive Action")]
        public Animator animator;

        //문여는 소리
        public AudioSource audioSource;

        //애니메이터 파라미터
        private const string Open = "Open";
        #endregion

        #region Custom Method
        //Interactive Action
        protected override void DoAction()
        {
            //액션 UI 감추기
            HideActionUI();

            //애니메이션
            animator.SetTrigger(Open);

            //사운드 플레이
            if(audioSource) //오디오소스가 널이 아니면
            {
                audioSource.Play();
            }
        }
        #endregion
    }
}