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
            WeaponController activeWeapon = playerWeaponManager.GetActiveWeapon();
            if(activeWeapon)
            {
                AddWeapon(activeWeapon, playerWeaponManager.ActiveWeaponIndex);
                SwitchWeapon(activeWeapon);
            }

            //초기화
            playerWeaponManager.OnAddedWeapon += AddWeapon;
            playerWeaponManager.OnRemovedWeapon += RemoveWeapon;
            playerWeaponManager.OnSwitchToWeapon += SwitchWeapon;
        }
        #endregion

        #region Custom Method
        //무기 추가시 UI 프리팹 추가
        private void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            //ammoCounter 오브젝트 생성
            AmmoCounter ammoCounter = Instantiate(ammoCounterPrefab, ammoPanel);
            //UI 초기화
            ammoCounter.Initialize(newWeapon, weaponIndex);

            //UI리스트에 추가
            ammoCounters.Add(ammoCounter);
        }

        //무기 제거시 UI 오브젝트 제거
        private void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {
            //삭제될 AmmoCounter UI 찾기
            int findCounterIndex = -1;
            for (int i = 0; i < ammoCounters.Count; i++)
            {
                if (ammoCounters[i].WeaponCounterIndex == weaponIndex)
                {
                    findCounterIndex = i;
                    //UI 오브젝트 킬
                    Destroy(ammoCounters[i].gameObject);
                    break;
                }
            }

            //UI를 찾았다면 리스트에서도 제거
            if(findCounterIndex >= 0)
            {
                ammoCounters.RemoveAt(findCounterIndex);
            }
        }


        //무기 교체시 (ammoPanel) UI 리빌딩(갱신)
        private void SwitchWeapon(WeaponController weapon)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(ammoPanel);
        }
        #endregion
    }
}
