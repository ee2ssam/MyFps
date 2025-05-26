using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyFps
{
    //피스톨 제어 클래스
    public class PistolShoot : MonoBehaviour
    {
        #region Variable
        //참조
        private Animator animator;
        public Transform firePoint;

        public GameObject muzzleEffect;
        public AudioSource pistolShot;

        //총구 발사 이펙트
        public ParticleSystem muzzleFlash;
        //피격 이펙트 - 뷸렛이 피격되는 지점에서 이펙트 효과 발생
        public GameObject hitImpactPrefab;
        //hit 충격강도
        [SerializeField]
        private float impactForce = 10f;

        //연사방지
        private bool isFire = false;

        //공격력
        [SerializeField]
        private float attackDamage = 5f;

        //공격 최대 사거리
        private float maxAttackDistance = 200f;

        //애니메이션 파라미터
        private string fire = "Fire";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            animator = this.GetComponent<Animator>();
        }

        private void OnDrawGizmosSelected()
        {
            //FirePoint에서 DrawRay(Red) 최대 200으로
            //레이를 쏴서 200 안에 충돌체가 있으면 충돌체 까지 레이를 그리고
            //충돌체가 없으면 레이를 200까지 그린다
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDistance);

            Gizmos.color = Color.red;
            if(isHit)
            {
                Gizmos.DrawRay(firePoint.position, firePoint.forward * hit.distance);
            }
            else
            {
                Gizmos.DrawRay(firePoint.position, firePoint.forward * maxAttackDistance);
            }
        }
        #endregion

        #region Custom Method
        public void Fire()
        {
            //연사 방지 체크
            if (isFire)
                return;

            if (PlayerDataManager.Instance.UseAmmo(1))
            {
                StartCoroutine(Shoot());
            }   
        }

        //슛
        IEnumerator Shoot()
        {
            //연사 방지 발사후(1초동안)  발사가 안되도록 한다
            isFire = true;

            //레이를 쏴서 200 안에 적(로봇)이 있으면 적(로봇)에게 데미지를 준다
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDistance);
            if(isHit)
            {
                //Debug.Log($"{hit.transform.name}에게 {attackDamage} 데미지를 준다");
                /*Robot robot = hit.transform.GetComponent<Robot>();
                if(robot != null)
                {
                    robot.TakeDamage(attackDamage);
                }
                ZombieRobot zombieRobot = hit.transform.GetComponent<ZombieRobot>();
                if(zombieRobot != null)
                {
                    zombieRobot.TakeDamage(attackDamage);
                }*/

                if(hitImpactPrefab)
                {
                    GameObject effectGo = Instantiate(hitImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(effectGo, 2f);
                }

                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
                }

                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                }
            }

            //애니메이션 플레이
            animator.SetTrigger(fire);

            //연출 Vfx, Sfx
            //발사 이펙트 플래시 활성화
            muzzleEffect.SetActive(true);
            if(muzzleFlash)
            {
                muzzleFlash.Play();
            }
            
            //발사 사운드 플레이
            pistolShot.Play();

            //0.5초 딜레이
            yield return new WaitForSeconds(0.3f);
            //발사 이펙트 플래시 비활성화
            muzzleEffect.SetActive(false);
            if(muzzleFlash)
            {
                muzzleFlash.Stop();
            }

            yield return new WaitForSeconds(0.7f);

            isFire = false;
        }
        #endregion
    }
}