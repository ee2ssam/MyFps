using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

namespace MyFps
{
    //시네머신 카메라 흔들림 효과 구현 : 싱글톤
    public class CinemachineCameraShake : Singleton<CinemachineCameraShake>
    {
        #region Variables
        //참조
        private CinemachineBasicMultiChannelPerlin channelPerlin;

        //흔들림 체크
        private bool isShake = false;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            channelPerlin = this.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            //TODO: Cheating Test
            /*if(Input.GetKeyDown(KeyCode.Q))
            {
                Shake(1f, 1f, 1f);
            }*/
        }
        #endregion

        #region Custom Method
        //카메라 흔들기
        //amplitudeGain : 흔들림 크기,  frequenceGain:흔들림 속도, shakeTime: 흔들리는 시간
        public void Shake(float amplitudeGain, float frequencyGain, float shakeTime)
        {
            //흔들림 체크
            if (isShake)
                return;

            StartCoroutine(StartShake(amplitudeGain, frequencyGain, shakeTime));
        }

        //
        IEnumerator StartShake(float amplitudeGain, float frequencyGain, float shakeTime)
        {
            isShake = true;

            //흔들림 셋팅
            channelPerlin.AmplitudeGain = amplitudeGain;
            channelPerlin.FrequencyGain = frequencyGain;

            //흔들림 지속 시간
            yield return new WaitForSeconds(shakeTime);

            //흔들림 초기화, 고정
            channelPerlin.AmplitudeGain = 0f;
            channelPerlin.FrequencyGain = 0f;

            isShake = false;
        }
        #endregion
    }
}
