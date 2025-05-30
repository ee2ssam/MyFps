using UnityEngine;

namespace MyFps
{
    //공격을 받으면 (데미지 입으면) 오브젝트가 부서진다
    //오브젝트 부서지는 연출, 두번 다시 공격을 받지 않아야 된다
    //부서질때 그릇 깨지는 사운드 플레이
    //아이템(key) 숨기기
    public class BreakableObejct : MonoBehaviour, IDamageable
    {
        #region Variables
        public GameObject fakeObejct;
        public GameObject realObject;

        private bool isDeath = false;   //두번 죽는것 체크
        private float health = 1f;        
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {

        }
        #endregion
    }
}
