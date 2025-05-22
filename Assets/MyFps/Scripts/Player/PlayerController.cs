using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace MyFps
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        private CharacterController controller;

        //입력 - 이동
        private Vector2 inputMove;

        //이동
        [SerializeField]
        private float moveSpeed = 10f;

        //중력
        private float gravity = -9.81f;
        [SerializeField]
        private Vector3 velocity;       //중력 계산에 의한 이동 속도

        //그라운드 체크
        public Transform groundCheck;   //발 바닥 위치
        [SerializeField] private float checkRange = 0.2f;    //체크 하는 구의 반경
        [SerializeField] private LayerMask groundMask;       //그라운드 레이어 판별

        //점프 높이
        [SerializeField] private float jumpHeight = 1f;

        //체력
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 20;

        private bool isDeath = false;

        //데미지 효과
        public GameObject damageFlash;

        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;

        //죽음 처리
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "GameOver";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //참조
            controller = this.GetComponent<CharacterController>();

            //초기화
            currentHealth = maxHealth;
        }

        private void Update()
        {
            //땅에 있으면
            bool isGrounded = GroundCheck();
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = -10f;
            }

            //방향
            //Global축 이동
            //Vector3 moveDir = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;
            //Local축 이동
            Vector3 moveDir = transform.right * inputMove.x + transform.forward * inputMove.y;

            //이동
            controller.Move(moveDir * Time.deltaTime * moveSpeed);

            //중력에 따른 y축 이동
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        #endregion

        #region Custom Method
        //Input 시스템에 등록할 함수
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && GroundCheck())
            {
                //점프 높이만큼 뛰기 위한 속도 구하기
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }

        //그라운드 체크
        bool GroundCheck()
        {
            return Physics.CheckSphere(groundCheck.position, checkRange, groundMask);
        }


        //플레이어 데미지
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Debug.Log($"player currentHealth: {currentHealth}");

            //데미지 연출 Sfx, Vfx
            StartCoroutine(DamageEffect());

            if(currentHealth <= 0 && isDeath == false)
            {
                Die();
            }
        }

        //- 화면전체 빨간색 플래쉬 효과
        //- 데미지 사운드 3개중 1 랜덤 발생
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

            //죽음처리
            fader.FadeTo(loadToScene);
        }
        #endregion
    }
}
