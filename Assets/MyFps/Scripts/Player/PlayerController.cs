using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyFps
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //����
        private CharacterController controller;

        //�Է� - �̵�
        private Vector2 inputMove;

        //�̵�
        [SerializeField]
        private float moveSpeed = 10f;

        //�߷�
        private float gravity = -9.81f;
        [SerializeField]
        private Vector3 velocity;       //�߷� ��꿡 ���� �̵� �ӵ�

        //�׶��� üũ
        public Transform groundCheck;   //�� �ٴ� ��ġ
        [SerializeField] private float checkRange = 0.2f;    //üũ �ϴ� ���� �ݰ�
        [SerializeField] private LayerMask groundMask;       //�׶��� ���̾� �Ǻ�

        //���� ����
        [SerializeField] private float jumpHeight = 1f;

        //ü��
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //������ ȿ��
        public GameObject damageFlash;

        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

        //���� ó��
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "GameOver";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //����
            controller = this.GetComponent<CharacterController>();

            //�ʱ�ȭ
            currentHealth = maxHealth;
        }

        private void Update()
        {
            //���� ������
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -10f;
            }

            //����
            //Global�� �̵�
            //Vector3 moveDir = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;
            //Local�� �̵�
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            //�̵�
            controller.Move(moveDir * Time.deltaTime * moveSpeed);

            //�߷¿� ���� y�� �̵�
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        #endregion

        #region Custom Method
        //Input �ý��ۿ� ����� �Լ�
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && GroundCheck())
            {
                //���� ���̸�ŭ �ٱ� ���� �ӵ� ���ϱ�
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }

        //�׶��� üũ
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }


        //�÷��̾� ������
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"player currentHealth: {currentHealth}");

            //������ ���� Sfx, Vfx
            StartCoroutine(DamageEffect());

            if(currentHealth <= 0 && isDeath == false)
            {
                Die();
            }
        }

        //- ȭ����ü ������ �÷��� ȿ��
        //- ������ ���� 3���� 1 ���� �߻�
        IEnumerator DamageEffect()
        {
            //vfx
            damageFlash.SetActive(true);

            //sfx
            int randNum = Random.Range(1, 4);
            if(randNum == 1)
            {
                hurt01.Play();
            }
            else if (randNum == 2)
            {
                hurt02.Play();
            }
            else
            {
                hurt03.Play();
            }

            yield return new WaitForSeconds(1f);
            damageFlash.SetActive(false);
        }

        private void Die()
        {
            isDeath = true;

            //����ó��
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
