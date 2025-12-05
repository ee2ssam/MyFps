using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace MyFps
{
    /// <summary>
    /// 메인메뉴 씬을 관리하는 클래스
    /// 메인메뉴 버튼기능, 신페이더 기능
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "PlayScene01";

        //UI
        public GameObject mainMenuUI;
        public GameObject optionUI;
        public GameObject creditUI;

        public GameObject loadGameButton;

        //Option - 볼륨관리
        public AudioMixer audioMixer;

        public Slider bgmSlider;
        public Slider sfxSlider;

        //씬번호
        private int sceneNumber = -1;

        //AudioMixer, PlyaerPrefs 파라미터
        private const string BgmVolume = "BgmVolume";
        private const string SfxVolume = "SfxVolume";        
        private const string SceneNumber = "SceneNumber";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //저장된 데이터 불러와서 게임 데이터 초기화
            GameDataInit();

            //로드게임 버튼 셋팅
            if(sceneNumber < 0)
            {
                loadGameButton.SetActive(false);
            }
            else
            {
                loadGameButton.SetActive(true);
            }

            //페이드인 시작
            fader.FadeStart();

            //배경음 플레이
            AudioManager.Instance.PlayBGM("MenuMusic");

            //커서 초기화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //초기화

        }
        #endregion

        #region Custom Method
        //버튼 대응 함수 구현, Debug.Log("  버튼 클릭")
        public void NewGame()
        {
            //버튼 효과음
            AudioManager.Instance.Play("ButtonHit");

            fader.FadeTo(loadToScene);
        }

        public void LoadGame()
        {
            //버튼 효과음
            AudioManager.Instance.Play("ButtonHit");

            fader.FadeTo(sceneNumber);
        }

        public void Options()
        {
            //Debug.Log("Goto Option Menu");
            ShowOptionUI();
        }

        public void Credits()
        {
            //Debug.Log("Goto Credits Menu");
            ShowCreditUI();
        }

        public void QuitGame()
        {
            //치팅: 저장된 데이터 리셋
            PlayerPrefs.DeleteAll();

            //게임종료
            Application.Quit();

            Debug.Log("QuitGame");
        }

        //옵션 보이기
        private void ShowOptionUI()
        {
            mainMenuUI.SetActive(false);
            optionUI.SetActive(true);
        }

        //옵션 감추기
        public void HideOptionUI()
        {
            //옵션 데이터 저장
            SaveOptions();

            //UI
            optionUI.SetActive(false);
            mainMenuUI.SetActive(true);

            //버튼 효과음
            AudioManager.Instance.Play("ButtonHit");
        }

        //옵션 배경음 볼륨 변경시 호출
        public void SetBgmVolume(float value)
        {
            //value 값 저장
            //PlayerPrefs.SetFloat(BgmVolume, value);

            //믹서 적용
            audioMixer.SetFloat(BgmVolume, value);
        }

        //옵션 효과음 보률 변경시 호출
        public void SetSfxVolume(float value)
        {
            //value 값 저장
            //PlayerPrefs.SetFloat(SfxVolume, value);

            //믹서 적용
            audioMixer.SetFloat(SfxVolume, value);
        }

        //옵션 데이터 저장하기
        private void SaveOptions()
        {
            Debug.Log("Save Option Data");

            //볼륨 값
            PlayerPrefs.SetFloat(BgmVolume, bgmSlider.value);
            PlayerPrefs.SetFloat(SfxVolume, sfxSlider.value);

            //기타 옵션 값
            //...
        }

        //옵션 데이터 불러오기
        public void LoadOptions()
        {
            Debug.Log("Load Option Data");

            //배경음 볼륨 값
            float bgmVolume = PlayerPrefs.GetFloat(BgmVolume, 0f);
            Debug.Log($"bgmVolume: {bgmVolume}");
            audioMixer.SetFloat(BgmVolume, bgmVolume);              //믹서 적용
            bgmSlider.value = bgmVolume;                            //UI 적용

            //효과음 볼륨 값
            float sfxVolume = PlayerPrefs.GetFloat(SfxVolume, 0f);
            Debug.Log($"sfxVolume: {sfxVolume}");
            audioMixer.SetFloat (SfxVolume, sfxVolume);             //믹서 적용
            sfxSlider.value = sfxVolume;                            //UI 적용

            //기타 옵션 값
            //...
        }

        private void ShowCreditUI()
        {
            mainMenuUI.SetActive(false);
            creditUI.SetActive(true);
        }

        private void GameDataInit()
        {
            //옵션 데이터
            LoadOptions();

            //플레이 데이터
            //sceneNumber = PlayerPrefs.GetInt(SceneNumber, -1);
            //Debug.Log($"Load SceneNumber: {sceneNumber}");

            PlayData playData = SaveLoad.LoadData();

            //PlayerStats의 데이터 초기화
            PlayerStats.Instance.PlayerStatsInitialize(playData);
        }
        #endregion
    }
}