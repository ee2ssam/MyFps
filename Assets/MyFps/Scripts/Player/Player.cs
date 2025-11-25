using UnityEngine;
using System.Collections;

namespace MyFps
{
    /// <summary>
    /// 플레이어를 관리(제어하는) 클래스
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Variables
        //씬 이동
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "GameOver";

        //참조
        private PlayerHealth playerHealth;

        //데미지 플래시
        public GameObject damageFlash;

        //데미지 사운드
        public AudioSource hurt01;
        public AudioSource hurt02;
        public AudioSource hurt03;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            playerHealth = GetComponent<PlayerHealth>();
        }

        private void OnEnable()
        {
            //데미지/죽음 이벤트 함수에 등록
            playerHealth.onDamage += OnDamage;
            playerHealth.onDie += OnDie;
        }

        private void OnDisable()
        {
            //데미지/죽음 이벤트 함수에 제거
            playerHealth.onDamage -= OnDamage;
            playerHealth.onDie -= OnDie;
        }
        #endregion


        #region Custom Method
        //데미지 입을때 호출되는 함수
        public void OnDamage()
        {
            StartCoroutine(DamageEffect());
        }

        IEnumerator DamageEffect()
        {
            //화면전체 빨간색 플래쉬 효과
            damageFlash.SetActive(true);

            //데미지 사운드 3개중 1 랜덤 발생
            int randNumber = Random.Range(1, 4); //1, 2, 3
            if (randNumber == 1)
            {
                hurt01.Play();
            }
            else if (randNumber == 2)
            {
                hurt02.Play();
            }
            else if (randNumber == 3)
            {
                hurt03.Play();
            }

            //화면 흔들림 효과
            CinemachineShake.Instance.ShakeCamera(1f, 1f, 1f);

            yield return new WaitForSeconds(1.0f);

            damageFlash.SetActive(false);
        }

        //죽었을때 호출되는 함수
        public void OnDie()
        {
            //게임오버 씬으로 이동, 게임오버 씬(또는 게임오버 UI) 구성 및 기능 구현
            //Debug.Log("게임오버 씬으로 이동");
            fader.FadeTo(loadToScene);
        }
        #endregion

    }
}