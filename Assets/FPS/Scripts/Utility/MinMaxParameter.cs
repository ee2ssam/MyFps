using System;
using UnityEngine;

namespace Unity.FPS.Utility
{
    //Lerp에서 사용하는 파라미터(최소값(A), 최대값(B))의 정의
    // A(0) -> B(1)

    [Serializable]
    public struct MinMaxFloat
    {
        public float Min;   //A
        public float Max;   //B

        //매개변수 ratio의 따른 Lerp값 반환
        public float GetValueFromRatio(float ratio)
        {
            return Mathf.Lerp(Min, Max, ratio);
        }
    }

    [Serializable]
    public struct MinMaxColor
    {
        [ColorUsage(true, true)] public Color Min;   //A
        [ColorUsage(true, true)] public Color Max;   //B

        //매개변수 ratio의 따른 Lerp값 반환
        public Color GetValueFromRatio(float ratio)
        {
            return Color.Lerp(Min, Max, ratio);
        }
    }

    [Serializable]
    public struct MinMaxVector3
    {
        public Vector3 Min;   //A
        public Vector3 Max;   //B

        //매개변수 ratio의 따른 Lerp값 반환
        public Vector3 GetValueFromRatio(float ratio)
        {
            return Vector3.Lerp(Min, Max, ratio);
        }
    }
}
