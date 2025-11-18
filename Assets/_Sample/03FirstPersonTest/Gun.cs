using UnityEngine;

namespace MySample
{
    /// <summary>
    /// F키를 누르면 탄환 발사 
    /// </summary>
    public class Gun : MonoBehaviour
    {
        #region Variables
        public GameObject bulletPrefab; //총알 프리팹
        public Transform firePoint;

        //연사방지,
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //F키를 누르면 탄환 발사
            if (Input.GetKeyDown(KeyCode.F))
            {
                Fire();
            }
        }
        #endregion

        #region Custom Method
        void Fire()
        {
            GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Bullet bullet = bulletGo.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.MoveForce();
            }

            //bullet kill 예약
            Destroy(bulletGo, 3f);

        }
        #endregion
    }
}
