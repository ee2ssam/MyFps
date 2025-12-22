using UnityEngine;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using Unity.FPS.Game;

namespace Unity.FPS.UI
{
    /// <summary>
    /// 무기 HUD 를 관리하는 클래스 
    /// </summary>
    public class WeaponHUDManager : MonoBehaviour
    {
        #region Variables
        public RectTransform ammoPanel;         //ammo UI를 관리하는 부모 오브젝트
        public GameObject ammoCountPrefab;      //ammo UI 프리팹 오브젝트

        //부모 오브젝트에 부착된 ammoCouter List
        private List<AmmoCounter> ammoCounters = new List<AmmoCounter>();

        //무기 관리
        private PlayerWeaponManager playerWeaponManager;
        #endregion

        #region Unity Evetne Method
        private void Awake()
        {
            //참조
            playerWeaponManager = GameObject.FindFirstObjectByType<PlayerWeaponManager>();
        }

        private void Start()
        {
            
        }
        #endregion

        #region Custom Method
        //무기 추가시 UI 추가
        public void AddWeapon(WeaponController newWeapon, int weaponIndex)
        {
            GameObject ammoCounterInstance = Instantiate(ammoCountPrefab, ammoPanel);
            AmmoCounter ammoCounter = ammoCounterInstance.GetComponent<AmmoCounter>();

            ammoCounters.Add(ammoCounter);
        }

        //무기 제거시 UI 제거
        public void RemoveWeapon(WeaponController oldWeapon, int weaponIndex)
        {
            int findCounterIndex = -1;
            for (int i = 0; i < ammoCounters.Count; i++ )
            {
                if (ammoCounters[i].weaponCounterIndex == weaponIndex)
                {
                    findCounterIndex = i;
                    //하이라키창에 있는 게임오브젝트 제거
                    Destroy(ammoCounters[i].gameObject);
                }
            }

            if(findCounterIndex > -1)
            {
                //리스트에 있는 AmmoCouter 제거
                ammoCounters.RemoveAt(findCounterIndex);
            }
        }

        //액티브 무기 교체시 UI도 변경
        #endregion
    }
}