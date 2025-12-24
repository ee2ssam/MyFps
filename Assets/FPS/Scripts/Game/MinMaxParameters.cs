using System;
using UnityEngine;

namespace Unity.FPS.Game
{
    //최대값, 최소값 설정후 Lerp값을 구하는 구조체 정의
        
    /// <summary>
    /// float Lerp값 구하기
    /// </summary>
    [Serializable]
    public struct MinMaxFloat
    {
        public float Min;           //a: 0
        public float Max;           //b: 1

        public float GetValueFromRatio(float ratio)
        {
            return Mathf.Lerp(Min, Max, ratio);
        }
    }

    /// <summary>
    /// Color Lerp값 구하기
    /// </summary>
    [Serializable]
    public struct MinMaxColor
    {
        [ColorUsage(true, true)]
        public Color Min;
        [ColorUsage(true, true)]
        public Color Max;

        public Color GetValueFromRatio(float ratio)
        {
            return Color.Lerp(Min, Max, ratio);
        }
    }

    /// <summary>
    /// Vector3 Lerp값 구하기
    /// </summary>
    [Serializable]
    public struct MinMaxVector3
    {
        public Vector3 Min;
        public Vector3 Max;

        public Vector3 GetValueFromRatio(float ratio)
        {
            return Vector3.Lerp(Min, Max, ratio);
        }
    }
}
