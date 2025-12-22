using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 오디오 플레이 관련 정적(static) 기능 구현
    /// </summary>
    public class AudioUtility : MonoBehaviour
    {
        #region Custom Static Method
        //지정된 위치에서 3D사운드 플레이
        public static void CreatSFX(AudioClip clip, Vector3 point, float spatialBlend,
            float rolloffDsitanceMin = 1f)
        {
            //지정된 위치에 빈오브젝트 만들기
            GameObject sfxInstance = new GameObject();
            sfxInstance.transform.position = point;

            //빈 오브젝트에 오디오소스 컴포넌트 추가 및 셋팅
            AudioSource audioSource = sfxInstance.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = spatialBlend;
            audioSource.minDistance = rolloffDsitanceMin;
            audioSource.Play();

            //빈 오브젝트에 TimeSelfDestruct 컴포넌트 추가 킬 예약
            TimeSelfDestruct timeSelfDestruct = sfxInstance.AddComponent<TimeSelfDestruct>();
            timeSelfDestruct.lifeTime = clip.length;

        }
        #endregion
    }
}