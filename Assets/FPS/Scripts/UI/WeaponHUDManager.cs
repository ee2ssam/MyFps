using Unity.FPS.Game;
using Unity.FPS.Gameplay;
using UnityEngine;
using System.Collections.Generic;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 무기(Ammo) UI들을 관리하는 클래스
    /// </summary>
    public class WeaponHUDManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager playerWeaponManager;

        //UI 관리
        public RectTransform ammoPannel;            //무기(Ammo) UI를 관리하는 부모 오브젝트
        public AmmoCounter ammoCountPrefab;          //무기(Ammo) UI 프리팹
        private List<AmmoCounter> ammoCounters = new List<AmmoCounter>();   //무기(Ammo) UI 리스트
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            playerWeaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();
        }

        private void OnEnable()
        {
            //이벤트 함수 등록
            playerWeaponManager.OnAddedWeapon += AddWeapon;
            playerWeaponManager.OnRemovedWeapon += RemoveWeapon;
            playerWeaponManager.OnSwitchToWeapon += SwitchWeapon;
        }

        private void OnDisable()
        {
            //이벤트 함수 제거
            playerWeaponManager.OnAddedWeapon -= AddWeapon;
            playerWeaponManager.OnRemovedWeapon -= RemoveWeapon;
            playerWeaponManager.OnSwitchToWeapon -= SwitchWeapon;
        }
        #endregion

        #region Custom Method
        //무기 추가시 호출되는 함수
        private void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            AmmoCounter ammoCounter = Instantiate(ammoCountPrefab, ammoPannel);
            //UI 초기화
            ammoCounter.Initialize(newWeapon, weaponIndex);
            ammoCounters.Add(ammoCounter);
        }

        //무기 제거시 호출되는 함수
        private void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {

        }

        //무기 교체시 호출되는 함수
        private void SwitchWeapon(WeaponController weapon)
        {

        }
        #endregion
    }
}