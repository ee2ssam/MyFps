using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyFps
{
    //�ǽ��� ���� Ŭ����
    public class PistolShoot : MonoBehaviour
    {
        #region Variable
        //����
        private Animator animator;
        public Transform firePoint;

        public GameObject muzzleEffect;
        public AudioSource pistolShot;

        //�ѱ� �߻� ����Ʈ
        public ParticleSystem muzzleFlash;
        //�ǰ� ����Ʈ - �淿�� �ǰݵǴ� �������� ����Ʈ ȿ�� �߻�
        public GameObject hitImpactPrefab;
        //hit ��ݰ���
        [SerializeField]
        private float impactForce = 10f;

        //�������
        private bool isFire = false;

        //���ݷ�
        [SerializeField]
        private float attackDamage = 5f;

        //���� �ִ� ��Ÿ�
        private float maxAttackDistance = 200f;

        //�ִϸ��̼� �Ķ����
        private string fire = "Fire";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            animator = this.GetComponent<Animator>();
        }

        private void OnDrawGizmosSelected()
        {
            //FirePoint���� DrawRay(Red) �ִ� 200����
            //���̸� ���� 200 �ȿ� �浹ü�� ������ �浹ü ���� ���̸� �׸���
            //�浹ü�� ������ ���̸� 200���� �׸���
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
        public void OnFire(InputAction.CallbackContext context)
        {
            if(context.started && isFire == false) //keydown, bottondown
            {
                StartCoroutine(Shoot());
            }
        }

        //��
        IEnumerator Shoot()
        {
            //���� ���� �߻���(1�ʵ���)  �߻簡 �ȵǵ��� �Ѵ�
            isFire = true;

            //���̸� ���� 200 �ȿ� ��(�κ�)�� ������ ��(�κ�)���� �������� �ش�
            RaycastHit hit;
            bool isHit = Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out hit, maxAttackDistance);
            if(isHit)
            {
                Debug.Log($"{hit.transform.name}���� {attackDamage} �������� �ش�");
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

            //�ִϸ��̼� �÷���
            animator.SetTrigger(fire);

            //���� Vfx, Sfx
            //�߻� ����Ʈ �÷��� Ȱ��ȭ
            muzzleEffect.SetActive(true);
            if(muzzleFlash)
            {
                muzzleFlash.Play();
            }
            
            //�߻� ���� �÷���
            pistolShot.Play();

            //0.5�� ������
            yield return new WaitForSeconds(0.3f);
            //�߻� ����Ʈ �÷��� ��Ȱ��ȭ
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