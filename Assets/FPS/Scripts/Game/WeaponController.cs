using UnityEngine;

namespace Unity.FPS.Game
{
    /// <summary>
    /// 총기류 무기를 관리하는 클래스
    /// </summary>
    [RequireComponent (typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        #region Variables
        //참조
        private AudioSource shootAudioSource;

        //무기 활성화, 비활성
        public GameObject weaponRoot;

        //무기 교체 효과음
        public AudioClip switchWeaponSfx;
        #endregion

        #region Property
        public GameObject Owner { get; set; }           //무기 주체(주인)
        public GameObject SoucePrefab { get; set; }     //무기 프리팹 오브젝트
        public bool IsWeaponActive { get; private set; }    //무기 활성화 여부, true 이면 현재 들고 있는 무기
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            shootAudioSource = GetComponent<AudioSource>();
        }
        #endregion

        #region Custom Method
        //무기 교체 - show: 활성화, 비활성
        public void ShowWeapon(bool show)
        {
            weaponRoot.SetActive(show);
            //무기 교체 효과음
            if (show == true && switchWeaponSfx != null)
            {
                shootAudioSource.PlayOneShot(switchWeaponSfx);
            }
            IsWeaponActive = show;
        }
        #endregion
    }
}