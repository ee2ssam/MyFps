using UnityEngine;
using System;

namespace MyFps
{
    /// <summary>
    /// 사운드 데이터 속성 정의 클래스
    /// </summary>
    [Serializable]
    public class Sound
    {
        public string name;     //관리되는 사운드의 이름

        public AudioClip clip;  //재생할 사운드(오디오) 리소스 - 음원

        [Range(0f, 1f)]
        public float vloume;    //재생 소리 크기

        [Range(0.1f, 3f)]
        public float pitch;     //재생 속도

        public bool loop;       //반복 재생 여우
        public bool playOnAwake; //처음 플레이 여부

        [HideInInspector]   //직렬화된 속성이어도 인스펙터 창에서 보이지 않는다
        public AudioSource source;  //오디오 데이터가 설정될 오디오소스
    }
}