using UnityEngine;

namespace Unity.FPS.AI
{
    /// <summary>
    /// 이동하는 Enemy를 관리하는 클래스
    /// </summary>
    public class EnemyMobile : MonoBehaviour
    {
        #region Variables
        //참조
        private EnemyController enemyController;

        //데미지 VFX
        public ParticleSystem[] randomHitSparks;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            enemyController = GetComponent<EnemyController>();
        }

        private void Start()
        {
            //enemyController 이벤트 함수 등록
            enemyController.onDamaged += OnDamaged;
        }
        #endregion

        #region Custom Method
        private void OnDamaged()
        {
            //데이지 효과 구현
            //스파크 파티클 랜덤 플레이
            if(randomHitSparks.Length > 0)
            {
                int randNumber = Random.Range(0, randomHitSparks.Length);
                randomHitSparks[randNumber].Play();
            }


        }
        #endregion
    }
}