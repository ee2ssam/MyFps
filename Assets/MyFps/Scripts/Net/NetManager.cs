using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net;
using System.IO;
using System.Text;

namespace MyFps
{
    public class NetManager : PersistanceSingleton<NetManager>
    {
        //접속할 서버 URL(dev,live)
        private string serverUrl = "http://192.168.105.150:8500/api";

        Dictionary<HttpWebRequest, object> mRequestData = new Dictionary<HttpWebRequest, object>();
        public delegate void WWWRequestFinished(string pSuccess, string pData);

        public NetMessage netMessage = NetMessage.Version;
        private string netError = "";

        public int netResult = -1; // 응답 성공: 0, 실패: 0 이외의 값
        public bool netFail = false; //네트워크 불안정

        private string userId = "";

        //응답 성공시 호출되는 이벤트 함수
        public UnityAction<int> OnNetUpdate;

        //서버 요청
        public void POST(string url, string post, WWWRequestFinished pDelegate)
        {
            Debug.Log(url + ": " + post);

            var bytes = Encoding.UTF8.GetBytes(post);

            HttpWebRequest aWww = (HttpWebRequest)WebRequest.Create(url);
            aWww.Method = "POST";
            aWww.ContentType = "application/json";
            aWww.MediaType = "application/json";
            aWww.Accept = "application/json";
            aWww.ContentLength = bytes.Length;

            Stream requestStream = aWww.GetRequestStream();            
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            mRequestData[aWww] = pDelegate;
            StartCoroutine(WaitForRequest(aWww));
        }

        IEnumerator WaitForRequest(HttpWebRequest pWww)
        {
            yield return pWww;

            string aSuccess = "sucess";

            HttpWebResponse httpWebResponse;

            using (httpWebResponse = (HttpWebResponse)pWww.GetResponse())
            {
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string result = streamReader.ReadToEnd();

                WWWRequestFinished aDelegate = (WWWRequestFinished)mRequestData[pWww];
                aDelegate(aSuccess, result);
            }
        }


        private void ReciveResult(string pSuccess, string pData)
        {
            if(pSuccess.Equals("sucess"))
            {
                switch(netMessage)
                {
                    case NetMessage.Login:
                        UserLoginResult loginResult = JsonUtility.FromJson<UserLoginResult>(pData);
                        if(loginResult.result == 0)
                        {
                            Debug.Log("로그인 성공");
                            netResult = 0;
                            userId = loginResult.userId;
                        }
                        else if (loginResult.result == 1)
                        {
                            Debug.Log("아이디가 없습니다");
                            netResult = 1;                            
                        }
                        else
                        {
                            Debug.Log("로그인 실패");
                            netResult = 2;
                            netFail = true;
                        }
                        break;

                    case NetMessage.RegisterUser:
                        UserRegisterResult registerResult
                            = JsonUtility.FromJson<UserRegisterResult>(pData);
                        if (registerResult.result == 0)
                        {
                            Debug.Log("등록 성공");
                            netResult = 0;
                        }
                        else if (registerResult.result == 1)
                        {
                            Debug.Log("중복 아이디");
                            netResult = 1;
                        }
                        else
                        {
                            Debug.Log("등록 실패");
                            netResult = 2;
                            netFail = true;
                        }
                        break;

                    case NetMessage.UserInfo:
                        UserInfoResult userInfoResult 
                            = JsonUtility.FromJson<UserInfoResult>(pData);
                        if(userInfoResult.result == 0)
                        {
                            netResult = 0;
                            //게임에 필요한 유저 정보 셋팅
                        }
                        else
                        {
                            netResult = 1;
                            netFail = true;
                        }
                        break;

                    case NetMessage.Levelup:
                        UserLevelupResult levelupResult
                            = JsonUtility.FromJson<UserLevelupResult>(pData);
                        if(levelupResult.result == 0)
                        {
                            netResult = 0;

                            //클라이언트 레벨을 셋팅
                            //levelupResult.level
                        }
                        else
                        {
                            netResult = 1;
                            netFail = true;
                        }
                        break;
                }

                OnNetUpdate?.Invoke(netResult);
            }
        }

        //로그인 요청
        public void NetSendLogin(string id, string password)
        {
            //네트워크 상태값 설정
            netResult = -1;
            netFail = false;
            netMessage = NetMessage.Login;

            //보내는 메세지 가공
            UserLogin userLogin = new UserLogin();
            userLogin.protocol = (int)netMessage;
            userLogin.userId = id;
            userLogin.password = password;
            string json = JsonUtility.ToJson(userLogin);

            //요청
            string requestUrl = serverUrl + "/UserLoginServices";
            POST(requestUrl, json, ReciveResult);

        }
        
        //유저 등록
        public void NetSendUserRegister(string id, string password)
        {
            //네트워크 상태값 설정
            netResult = -1;
            netFail = false;
            netMessage = NetMessage.RegisterUser;

            //보내는 메세지 가공
            UserRegister userRegister = new UserRegister();
            userRegister.protocol = (int)netMessage;
            userRegister.userId = id;
            userRegister.password = password;
            string json = JsonUtility.ToJson(userRegister);

            //요청
            string requestUrl = serverUrl + "/UserAddServices";
            POST(requestUrl, json, ReciveResult);
        }

        //유저 정보 가져오기
        public void NetSendUserInfo()
        {
            //네트워크 상태값 설정
            netResult = -1;
            netFail = false;
            netMessage = NetMessage.UserInfo;

            //보내는 메세지 가공
            UserInfo userInfo = new UserInfo();
            userInfo.protocol = (int)netMessage;
            userInfo.userId = userId;
            string json = JsonUtility.ToJson(userInfo);

            //요청
            string requestUrl = serverUrl + "/UserInfoServices";
            POST(requestUrl, json, ReciveResult);
        }

        //레벨업
        public void NetSendUserLevelup()
        {
            //네트워크 상태값 설정
            netResult = -1;
            netFail = false;
            netMessage = NetMessage.Levelup;

            //보내는 메세지 가공
            UserLevelup userLevelup = new UserLevelup();
            userLevelup.protocol = (int)netMessage;
            userLevelup.userId = userId;
            string json = JsonUtility.ToJson(userLevelup);

            //요청
            string requestUrl = serverUrl + "/LevelupService";
            POST(requestUrl, json, ReciveResult);
        }

    }


    //프로토콜
    public enum NetMessage
    {
        Version = 0,
        Login = 1101,
        RegisterUser,
        UserInfo,
        Levelup,
        AddGold,
    }

    //로그인 요청 : 1101
    [Serializable]
    public class UserLogin
    {
        public int protocol;
        public string userId;
        public string password;
    }

    //로그인 응답
    [Serializable]
    public class UserLoginResult
    {
        public int protocol;
        public int result;
        public string userId;
    }

    //유저 등록 요청 : 1102
    [Serializable]
    public class UserRegister
    {
        public int protocol;
        public string userId;
        public string password;
    }

    //유저 등록 응답
    [Serializable]
    public class UserRegisterResult
    {
        public int protocol;
        public int result;
        public string userId;
    }

    //유저 정보 가져오기 요청 : 1103
    [Serializable]
    public class UserInfo
    {
        public int protocol;
        public string userId;
    }

    //유저 정보 가져오기 응답
    [Serializable]
    public class UserInfoResult
    {
        public int protocol;
        public int result;
        public string userId;
        public int level;
        public int gold;
    }

    //유저 레벨업 요청 : 1104
    [Serializable]
    public class UserLevelup
    {
        public int protocol;
        public string userId;
    }

    //유저 레벨업 응답
    [Serializable]
    public class UserLevelupResult
    {
        public int protocol;
        public int result;
        public string userId;
        public int level;
    }

}

