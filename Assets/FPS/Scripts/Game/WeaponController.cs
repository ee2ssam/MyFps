using UnityEngine;
using System;

namespace Unity.FPS.Game
{
    //크로스헤어 데이터 구조체
    [Serializable]
    public struct CrosshairData
    {
        public Sprite crosshairSprite;
        public float crosshairSize;
        public Color crosshairColor;
    }

    //무기를 제어하는 클래스, 모든 무기에 부착된다
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //무기 비쥬얼 활성화, 비활성
        public GameObject weaponRoot;

        private AudioSource shootAudioSource;
        public AudioClip swtichWeaponSfx;           //무기 교환시 효과음

        //크로스 헤어
        public CrosshairData defalutCrosshair;      //기본 크로스 헤어
        public CrosshairData targetInSightCrosshair;    //적이 타겟팅 되었을때의 크로스 헤어
        #endregion

        #region Property
        public GameObject Owner { get; set; }   //무기를 장착한 주인 오브젝트
        public GameObject SourcePrefab { get; set; }    //무기를 생성한 원본 프리팹
        public bool IsWeaponActive { get; set; }        //현재 이 무기가 액티브한지
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            shootAudioSource = this.GetComponent<AudioSource>();
        }
        #endregion

        #region Custom Method
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            IsWeaponActive = show;
            //무기교체
            if(show == true && swtichWeaponSfx)
            {
                shootAudioSource.PlayOneShot(swtichWeaponSfx);
            }
        }
        #endregion
    }
}
