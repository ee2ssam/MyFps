using UnityEngine;
using System.Collections;
using TMPro;

namespace MyFps
{
    //타이틀 씬을 관리하는 클래스 : 3초후에 애니키 보이고 10초 후에 메인메뉴 가기
    public class Title : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "MainMenu";

        private bool isShow = false;    //애니키가 보이냐?
        public GameObject anyKey;

#if NET_MODE
        private NetManager netManager;

        //login UI
        public GameObject login;
        public GameObject loginMenu;
        public GameObject loginUI;
        public GameObject loginButton;
        public GameObject newButton;
        public GameObject messageUI;
        public TextMeshProUGUI message;

        public TMP_InputField loginId;
        public TMP_InputField password;
#endif
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {            

        }

        private void OnDisable()
        {
#if NET_MODE
            netManager.OnNetUpdate -= OnUpdateNet;
#endif            
        }

        private void Start()
        {
#if NET_MODE
            netManager = NetManager.Instance;
            netManager.OnNetUpdate += OnUpdateNet;
#endif

            //페이드인 효과
            fader.FadeStart();

            //배경음 플레이
            AudioManager.Instance.PlayBgm("TitleBgm");

            //코루틴 함수 실행
            StartCoroutine(TitleProcess());
        }

        private void Update()
        {
            //애니키가 보인후에 아무키나 누르면 메인메뉴 가기 - old Input
            if (Input.anyKeyDown && isShow)
            {
                StopAllCoroutines();

                AudioManager.Instance.Stop("TitleBgm");
                fader.FadeTo(loadToScene);
            }
        }
        #endregion

        #region Custom Method
        //코루틴 함수 : 3초후에 애니키 보이고 10초 후에 메인메뉴 가기
        IEnumerator TitleProcess()
        {
            yield return new WaitForSeconds(3f);

#if NET_MODE
            ShowLogin();
#else
            isShow = true;
            anyKey.SetActive(true);

            yield return new WaitForSeconds(10f);

            AudioManager.Instance.Stop("TitleBgm");
            fader.FadeTo(loadToScene);
#endif
        }

#if NET_MODE
        //통신 결과 처리
        private void OnUpdateNet(int netResult)
        {
            switch(netManager.netMessage)
            {
                case NetMessage.Login:
                    if(netResult == 0) //로그인 성공
                    {
                        netManager.NetSendUserInfo();
                        Debug.Log("유저 데이터 가져오기");
                    }
                    else if (netResult == 1) //아이디가 없다
                    {
                        //경고창 보여주기
                        ShowMessageUI("유저 아이디가 없습니다");
                    }
                    else //기타 에러
                    {
                        //경고창 보여주기
                        ShowMessageUI("네트워크가 불안정 합니다. 다시 실행해 주세요");
                    }
                    break;

                case NetMessage.RegisterUser:
                    if (netResult == 0) //등록 성공
                    {
                        ShowMessageUI("유저 등록에 성공 했습니다");
                    }
                    else if (netResult == 1) //중복 유저
                    {
                        //경고창 보여주기
                        ShowMessageUI("중복된 아이디가 있습니다");
                    }
                    else //기타 에러
                    {
                        //경고창 보여주기
                        ShowMessageUI("네트워크가 불안정 합니다. 다시 실행해 주세요");
                    }
                    break;
                case NetMessage.UserInfo:
                    if (netResult == 0) //정보 가져오기 성공
                    {
                        //게임 플레이 시작 - 메인메뉴 가기
                        AudioManager.Instance.Stop("TitleBgm");
                        fader.FadeTo(loadToScene);
                    }
                    else //기타 에러
                    {
                        //경고창 보여주기
                        ShowMessageUI("네트워크가 불안정 합니다. 다시 실행해 주세요");
                    }
                    break;
            }
        }

        private void ResetLoginUI()
        {
            loginMenu.SetActive(false);
            loginUI.SetActive(false);
            messageUI.SetActive(false);

            loginButton.SetActive(false);
            newButton.SetActive(false);
            message.text = "";
        }

        private void ShowLogin()
        {
            login.SetActive(true);
            ShowLoginMenu();
        }

        public void ShowLoginMenu()
        {
            ResetLoginUI();
            loginMenu.SetActive(true);
        }

        public void ShowLoginUI()
        {
            ResetLoginUI();
            loginUI.SetActive(true);
            loginButton.SetActive(true);
        }

        
        public void ShowAddUserUI()
        {
            ResetLoginUI();
            loginUI.SetActive(true);
            newButton.SetActive(true);
        }

        public void ShowMessageUI(string msg)
        {
            ResetLoginUI();
            messageUI.SetActive(true);
            message.text = msg;
        }

        public void HideMessageUI()
        {
            if(netManager.netFail == true)
            {
                Application.Quit();
                return;
            }

            ResetLoginUI();
            ShowLoginMenu();
        }

        public void Login()
        {
            if (loginId.text.Length < 2 || loginId.text.Length > 20)
            {
                return;
            }
            if (password.text.Length < 2 || password.text.Length > 20)
            {
                return;
            }

            netManager.NetSendLogin(loginId.text, password.text);
            ResetLoginUI();
        }

        public void RegisterUser()
        {
            if (loginId.text.Length < 2 || loginId.text.Length > 20)
            {
                return;
            }
            if (password.text.Length < 2 || password.text.Length > 20)
            {
                return;
            }

            netManager.NetSendUserRegister(loginId.text, password.text);
            ResetLoginUI();
        }
#endif
        #endregion
    }
}
