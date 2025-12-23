using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 게이지바의 컬러를 관리하는 클래스
    /// </summary>
    public class FillBarColorChange : MonoBehaviour
    {
        #region Variables
        //게이지바 이미지
        public Image foregroundImage;
        public Color defaultForegroundColor;    //게이지바 기본 컬러
        public Color flashForgroundColorFull;   //게이지바가 풀 찰때 플래시 컬러

        //게이지 백그라운드 이미지
        public Image backgroundIamge;
        public Color defaultBackgroundColor;    //게이지 백그라운드 기본 컬러
        public Color flashBackgroundColorEmpty; //게이지바가 비었을때 백그라운드 플래시 컬러

        //연출
        public float fullValue = 1f;
        public float emptyValue = 0f;
        public float colorChangeSharpness = 5f; //컬러 변경 Lerp 계수 값

        private float m_WasValue;               //특정 시점을 찾기 위한 변수
        #endregion

        #region Custom Method
        //UI 생성시 게이지바 컬러값 초기화
        public void Initialize(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            m_WasValue = fullValue;
        }

        //CurrentAmmoRatio에 의한 게이지바 컬러 연출
        public void UpdateVisual(float currentRaito)
        {
            //평상시, 풀찰때, emptyValue이하로 떨어질때





            m_WasValue = currentRaito;
        }
        #endregion
    }
}