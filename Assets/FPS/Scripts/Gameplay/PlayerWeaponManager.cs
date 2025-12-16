using UnityEngine;
using Unity.FPS.Game;
using System.Collections.Generic;

namespace Unity.FPS.Gameplay
{
    public class PlayerWeaponManager : MonoBehaviour
    {
        #region Variables
        //무기 장착
        //처음 지급되는 무기(WeaponController가 붙어 있는 프리팹) 리스트
        public List<WeaponController> startingWeapons = new List<WeaponController>();

        //무기가 장착될 오브젝트
        public Transform weaponParentSocrket;

        //플레이어가 게임중에 들고다니는 무기 슬롯 리스트
        private WeaponController[] weaponSlots = new WeaponController[9];
        #endregion

        #region Property
        //무기 슬롯(weaponSlots)을 관리하는 인덱스
        public int ActiveWeaponIndex { get; private set; }
        #endregion
    }
}