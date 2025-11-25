using UnityEngine;

namespace MyFps
{
    /// <summary>
    /// 무기 발사 구현
    /// </summary>
    public class PlayerShoot : MonoBehaviour
    {
        #region Variables
        //참조
        public Animator animator;       //무기 애니메이터
        public Transform firePoint;     //파이어 포인트

        //무기 사거리
        [SerializeField]
        private float attackRange = 200f;
        //연사 방지(딜레이 시간: 0.7초)

        //무기 공격력
        [SerializeField]
        private float attackDamage = 5f;

        //이펙트 효과(VFX, SFX)
        public GameObject hitImpactPrefab;
        public AudioSource pistolShoot;

        //애니메이션 파라미터
        private const string IsShoot = "IsShoot";
        #endregion

        #region Property
        public bool IsFire
        {
            get
            {
                return animator.GetBool(IsShoot);
            }
        }
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //발사 버튼 입력 처리, 연사 방지
            if(Input.GetButtonDown("Fire") && IsFire == false)
            {
                //무기 소지 여부
                if(PlayerStats.Instance.WeaponType != WeaponType.None)
                {
                    //탄환 체크
                    if (PlayerStats.Instance.UseAmmo(1))
                    {
                        Shoot();
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            /*RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackRange);

            if(isHit)
            {
                //충돌체까지 레이저 그리기
                Gizmos.DrawRay(firePoint.position, firePoint.forward * hit.distance);
            }
            else
            {
                //attackRange까지 레이저 그리기
                Gizmos.DrawRay(firePoint.position, firePoint.forward * attackRange);
            }*/

            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, attackRange);

            if (isHit)
            {
                //충돌체까지 레이저 그리기
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            }
            else
            {
                //attackRange까지 레이저 그리기
                Gizmos.DrawRay(transform.position, transform.forward * attackRange);
            }
        }
        #endregion

        #region Custom Method
        //발사 처리
        private void Shoot()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                //Debug.Log($"hit object: {hit.transform.name}");
                //이펙트 효과(VFX)
                if (hitImpactPrefab)
                {
                    GameObject effectGo = Instantiate(hitImpactPrefab, hit.point, 
                        Quaternion.LookRotation(hit.normal));
                    //이펙트 킬 예약
                    Destroy(effectGo, 2f);
                }

                /*//적에게 데미지 주기                
                Robot robot = hit.transform.GetComponent<Robot>();
                if(robot)
                {
                    robot.TakeDamage(attackDamage);
                }*/
                //hit 오브젝트에 IDamageable을 상속받은 컴포넌트가 있으면 데미지 준다
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                }
            }

            //애니메이션
            animator.SetBool(IsShoot, true);

            //이펙트 효과(SFX)
            if(pistolShoot)
            {
                pistolShoot.Play();
            }
        }
        #endregion
    }
}