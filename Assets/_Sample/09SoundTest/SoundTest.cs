using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

namespace MySample
{
    /// <summary>
    /// 사운드 플레이 클래스
    /// </summary>
    public class SoundTest : MonoBehaviour
    {
        #region Variables
        //재생할 오디오(사운드) 소스
        public AudioClip clip;

        [SerializeField] private float volume = 1f;
        [SerializeField] private float pitch = 1f;
        [SerializeField] private bool isLoop = false;
        [SerializeField] private bool playOnAwake = false;

        //참조
        private AudioSource audioSource;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            //사운드 플레이
            SoundPlay();
        }
        #endregion

        #region Custom Method
        private void SoundPlay()
        {
            audioSource.clip = clip;

            audioSource.volume = volume;            
            audioSource.pitch = pitch;
            audioSource.loop = isLoop;
            audioSource.playOnAwake = playOnAwake;

            audioSource.Play();
        }
        #endregion
    }
}