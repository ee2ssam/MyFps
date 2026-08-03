using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 게이지바 이미지의 컬러를 관리하는 클래스
    /// </summary>
    public class FillBarColorChange : MonoBehaviour
    {
        #region Variables
        public Image foregroundImage;               //게이지 이미지

        public Color defaultForegroundColor;    //게이지 이미지의 Origin 컬러
        public Color flashForegroundColorFull;      //게이지가 풀 찰때 번쩍이는 컬러

        public Image backgroundImage;               //배경 이미지

        public Color defaultBackgroundColor;    //베경 이미지의 Origin 컬러
        public Color flashBackgroundColorEmpty;     //게이지가 0로 일때 번쩍이는 컬러

        public float fullValue = 1f;                //게이지의 Full 값
        public float emptyValue = 0f;               //게이지의 Empty 값

        public float colorChangeSharpness = 5f;     //컬러 변경 연출 Lerp 속도 계수
        private float m_PriviousValue;              //was(last) 변수 - 리로드시 게이지가 풀로 채워지는 순간
        #endregion

        #region Custom Method
        //초기화
        public void Initialize(float fullValueRatio, float emptyValueRatio)
        {
            fullValue = fullValueRatio;
            emptyValue = emptyValueRatio;

            m_PriviousValue = fullValueRatio;
        }

        //update 함수
        public void UpdateVisual(float currentRatio)
        {
            //게이지가 풀로 차는 순간
            if(currentRatio == fullValue && currentRatio != m_PriviousValue)
            {
                foregroundImage.color = flashForegroundColorFull;
            }
            else if(currentRatio <= emptyValue) //게이지가 비어 있을때(특정 수치 이하로 떨어질때)
            {
                backgroundImage.color = flashBackgroundColorEmpty;
            }
            else
            {
                foregroundImage.color = Color.Lerp(foregroundImage.color, defaultForegroundColor,
                    colorChangeSharpness * Time.deltaTime);
                backgroundImage.color = Color.Lerp(backgroundImage.color, defaultBackgroundColor,
                    colorChangeSharpness * Time.deltaTime);
            }


            m_PriviousValue = currentRatio;
        }
        #endregion
    }
}