using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    //UI 게이지 Bar 이미지 컬러 변경 효과
    public class ForBackColorChange : MonoBehaviour
    {
        #region Variables
        public Image forgroundImage;
        public Color defaultForGroundColor;     //게이지바 기본 컬러
        public Color fullFlashForGroundColor;   //100% 충전시 플래시 효과 컬러

        public Image backgroundImage;
        public Color defaultBackGroundColor;    //게이지바 백그라운드 이미지 기본 컬러
        public Color emptyFlashBackGroundColor; // 0%시 플래시 효과 컬러

        [SerializeField] private float fullValue = 1f;  //게이지바 rate Max Value
        [SerializeField] private float emptyValue = 0f; //게이지바 rate Min Value

        [SerializeField] private float colorChangeSharpness = 5f; //컬러 변경 속도 (Lerp 계수)

        private float m_PreviousValue;              //연출을 위한 was 변수
        #endregion

        #region Custom Method
        //UI 게이지 Bar 효과 초기화
        public void Initialized(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            m_PreviousValue = fullValue;
        }

        //게이지 Bar 효과 Update
        public void UpdateVusual(float currentRatio)
        {
            //100% 충전되는 순간 체크
            if(currentRatio == fullValue && currentRatio != m_PreviousValue)
            {
                forgroundImage.color = fullFlashForGroundColor;
            }
            else if(currentRatio < emptyValue)
            {
                backgroundImage.color = emptyFlashBackGroundColor;
            }
            else
            {
                forgroundImage.color = Color.Lerp(forgroundImage.color, defaultForGroundColor,
                    Time.deltaTime * colorChangeSharpness);
                backgroundImage.color = Color.Lerp(backgroundImage.color, defaultBackGroundColor,
                    Time.deltaTime * colorChangeSharpness);
            }

            //
            m_PreviousValue = currentRatio;
        }
        #endregion
    }
}
