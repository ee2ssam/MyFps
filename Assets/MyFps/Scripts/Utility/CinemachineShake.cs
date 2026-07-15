using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 화면 흔들림 효과 구현 싱글톤 클래스
    /// 흔들림 계수: 흔들림 속도, 흔들림 크기, 흔들림 시간
    /// </summary>
    public class CinemachineShake : Singleton<CinemachineShake>
    {
        #region Variables
        //참조
        private CinemachineBasicMultiChannelPerlin multiChannelPerlin;

        //흔들림 크기, 흔들림 속도, 흔들림 시간
        [SerializeField] private float amplitude = 0f;
        [SerializeField] private float frequency = 0f;
        [SerializeField] private float shakeTimer = 1f;
        #endregion

        #region Unity Event Method
        #endregion

        #region Custom Method
        #endregion
    }
}