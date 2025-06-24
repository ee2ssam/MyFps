using UnityEngine;
using System.Collections.Generic;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

namespace Unity.FPS.UI
{
    //Weapon ammo UI 관리 클래스
    public class WeaponHUDManager : MonoBehaviour
    {
        #region Variables
        //참조
        private PlayerWeaponManager playerWeaponManager;

        public RectTransform ammoPanel;     //ammoCounter프리팹 오브젝트의 부모 오브젝트
        public AmmoCounter ammoCounterPrefab;   //ammoCounter프리팹

        //ammoCounter UI 리스트
        private List<AmmoCounter> ammoCounters = new List<AmmoCounter>();
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            playerWeaponManager = FindFirstObjectByType<PlayerWeaponManager>();
        }

        private void Start()
        {
            //초기화
            playerWeaponManager.OnAddedWeapon += AddWeapon;
            playerWeaponManager.OnRemovedWeapon += RemoveWeapon;
        }
        #endregion

        #region Custom Method
        //무기 추가시 UI 프리팹 추가
        private void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            AmmoCounter ammoCounter = Instantiate(ammoCounterPrefab, ammoPanel);

            ammoCounters.Add(ammoCounter);
        }

        //무기 제거시 UI 오브젝트 제거
        private void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {

        }
        #endregion
    }
}
