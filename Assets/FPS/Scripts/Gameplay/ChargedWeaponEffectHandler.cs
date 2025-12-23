using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    /// <summary>
    /// 충전 무기의 충전 이펙트를 관리하는 클래스
    /// </summary>
    public class ChargedWeaponEffectHandler : MonoBehaviour
    {
        #region Variables
        public GameObject charingObject;
        public GameObject spiningFrame;
        public GameObject diskOrbitParticlePrefab;

        public MinMaxVector3 scale;
        #endregion
    }
}