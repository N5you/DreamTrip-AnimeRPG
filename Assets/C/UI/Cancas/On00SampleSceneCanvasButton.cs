using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class On00SampleSceneCanvasButton : MonoBehaviour
{
    [SerializeField] [Header("服务器列表")] private RectTransform serverlist;//服务器列表

    [SerializeField] private GameObject hotServerButton; //火爆服务器按钮预制体
    [SerializeField] private GameObject smoothServerButton; //流畅服务器按钮预制体

    private Text nameText;//标题
    private Text userText;//用户名

    private GameObject serverBgImage;//选择服务器面板
    private Text serverBgImageNameText;//选择服务器面板标题
    private Image serverBgImageNameImage;//选择服务器面板标题图
    private Text serverBgImageButtonText; //选择服务器面板按钮标题
    private Image serverBgImageButtonImage; ///选择服务器面板按钮图

    //server
    private GameObject loginBgImage;//登入面板
    private InputField userInputField;//登入，用户名输入框
    private InputField accountInputField;//登入，密码输入框
    private Text loginBgImageNameButton;//登入按钮标题

    private GameObject registeredBgImage; //注册面板
    private InputField registeredUserInputField;//注册，输入用户名
    private InputField registeredAccountInputField;//注册，输入密码
    private InputField determineAccountInputField;//注册，确认密码
    private Text registeredBgImageText;//注册，提示
    private void Start()
    {
        //Destroy(serverlist);
        nameText = transform.Find("NameImage/Text").GetComponent<Text>();
        userText = transform.Find("ServerButton/Text").GetComponent<Text>();

        serverBgImage = transform.Find("ServerBgImage").gameObject;
        serverBgImageNameText = transform.Find("ServerBgImage/NameImage/Text").GetComponent<Text>();
        serverBgImageNameImage = transform.Find("ServerBgImage/NameImage").GetComponent<Image>();
        serverBgImageButtonText = transform.Find("AccountButton/Text").GetComponent<Text>();
        serverBgImageButtonImage = transform.Find("AccountButton").GetComponent<Image>();

        loginBgImage = transform.Find("LoginBgImage").gameObject;
        userInputField = transform.Find("LoginBgImage/UserInputField").GetComponent<InputField>();
        accountInputField = transform.Find("LoginBgImage/AccountInputField").GetComponent<InputField>();
        loginBgImageNameButton = transform.Find("ServerButton/Text").GetComponent<Text>();

        registeredBgImage = transform.Find("RegisteredBgImage").gameObject;
        registeredUserInputField = registeredBgImage.transform.Find("UserInputField").GetComponent<InputField>();
        registeredAccountInputField = registeredBgImage.transform.Find("AccountInputField").GetComponent<InputField>();
        determineAccountInputField = registeredBgImage.transform.Find("DetermineAccountInputField").GetComponent<InputField>();
        registeredBgImageText = registeredBgImage.transform.Find("Text").GetComponent<Text>();

        //if (serverlist != null)
        //{
        //    ServerList serverList = ServerStoreRead.GetServerList();//获取服务器列表
        //    ServerListFetch(serverList.serverList.ToArray());
        //    Debug.Log(233);
        //}

        //serverBgImage.SetActive(false);
        string str = ServerStoreRead.GetLoginUser();
        string[] result = str.Split(',');
        if (result[1] != null) userText.text = result[1];
    }

    //private void ServerListFetch(Server[] serverLists)
    //{
    //    //foreach (var server in serverLists)
    //    //{
    //    //    GameObject gameServer;
    //    //    if (server.Number >= 1000)
    //    //    {
    //    //        gameServer = Instantiate(hotServerButton);
    //    //    }
    //    //    else
    //    //    {
    //    //        gameServer = Instantiate(smoothServerButton);
    //    //    }
    //    //    gameServer.transform.parent = serverlist;
    //    //    gameServer.transform.Find("Text").GetComponent<Text>().text = server.Name;
    //    //    gameServer.GetComponent<Button>().onClick.AddListener(OnSelectServerButton);
    //    //    Debug.Log(66);
    //    //}

    //    ////Debug.Log(serverLists.Length);
    //    ////for (int i = 0; i >= serverLists.Length; i++)
    //    ////{
    //    ////    Server server = serverLists[i];
    //    ////    GameObject gameServer;
    //    ////    if (server.Number >= 1000)
    //    ////    {
    //    ////        gameServer = Instantiate(hotServerButton);
    //    ////    }
    //    ////    else
    //    ////    {
    //    ////        gameServer = Instantiate(smoothServerButton);
    //    ////    }
    //    ////    gameServer.transform.parent = serverlist;
    //    ////    gameServer.transform.Find("Text").GetComponent<Text>().text = server.Name;
    //    ////    gameServer.GetComponent<Button>().onClick.AddListener(OnSelectServerButton);
    //    ////    Debug.Log(66);
    //    ////}

    //    for (int i = 0; i >= 10; i++)
    //    {
    //        GameObject gameServer;
    //        if (Random.Range(0, 2000) >= 1000)
    //        {
    //            gameServer = Instantiate(hotServerButton, serverlist);
    //        }
    //        else
    //        {
    //            gameServer = Instantiate(smoothServerButton, serverlist);
    //        }
    //        gameServer.transform.parent = serverlist;
    //        gameServer.transform.Find("Text").GetComponent<Text>().text = "飞向" + i + "服";
    //        gameServer.GetComponent<Button>().onClick.AddListener(OnSelectServerButton);
    //    }

    //    //foreach (var server in serverLists)
    //    //{
    //    //    GameObject gameServer;
    //    //    if (server.Number >= 1000)
    //    //    {
    //    //        gameServer = Instantiate(hotServerButton);
    //    //    }
    //    //    else
    //    //    {
    //    //        gameServer = Instantiate(smoothServerButton);
    //    //    }
    //    //    gameServer.transform.parent = serverlist;
    //    //    gameServer.transform.Find("Text").GetComponent<Text>().text = server.Name;
    //    //    gameServer.GetComponent<Button>().onClick.AddListener(OnSelectServerButton);
    //    //    Debug.Log(66);
    //    //}
    //}

    public void OnCloseRegisteredButton()
    {
        loginBgImage.SetActive(false);
    }
    public void OnRegisteredButton()//注册面板
    {
        registeredBgImageText.text = "请输入用户名和密码";
        loginBgImage.SetActive(false);
        registeredBgImage.SetActive(true);
    }

    public void OnRegisteredReturnLogin()
    {
        loginBgImage.SetActive(true);
        registeredBgImage.SetActive(false);
    }

    public void OnRegisteredBgImageButton()//注册
    {
        string userString = registeredUserInputField.text;
        userString = userString.Replace(",", ".");//字符串里面禁止有,逗号，如果有，用.点号代替

        string accountString = registeredAccountInputField.text;
        string determineString = determineAccountInputField.text;
        if (userString != null && accountString != null && determineString != null)
        {
            if (accountString != determineString)
            {
                registeredBgImageText.text = "密码不一致";
                return;
            }

            UserStateType userStateType = ServerStoreRead.UserRegistered(userString, accountString);

            if (userStateType == UserStateType.UserExisting)
            {
                registeredBgImageText.text = "该用户名已存在";
                return;
            }

            loginBgImageNameButton.text = userString;
            ServerStoreRead.UserLogin(userString, accountString);
            registeredBgImage.SetActive(false);
        }
    }
    public void OnUserLoginButton()//登入按钮
    {
        loginBgImage.SetActive(true);
    }
    public void OnUserLogin()//用户登入按钮（登入面板）
    {
        string userString = userInputField.text;
        string accountString = accountInputField.text;
        if (userString != null && accountString != null)
        {
            UserStateType userStateType = ServerStoreRead.UserLogin(userString, accountString);
            if (UserStateType.LoginSuccessful == userStateType)
            {
                loginBgImageNameButton.text = userString;
                loginBgImage.SetActive(false);
            }
        }
    }

    public void OnEnterTheGame()//进入游戏按钮
    {

        if (ServerStoreRead.UserIsNoLogin())
        {
            SceneManager.LoadScene(1);
            return;
        }
        nameText.text = "请先登录";
    }

    public void OnSelectServer()//选择服务器按钮
    {
        serverBgImage.SetActive(true);
    }

    public void OnSelectServerButton()//服务器按钮
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string str = button.transform.Find("Text").GetComponent<Text>().text;
        Sprite sprite = button.GetComponent<Image>().sprite;
        //Debug.Log(str);

        serverBgImageNameText.text = str;
        serverBgImageNameImage.sprite = sprite;
        serverBgImageButtonText.text = str;
        serverBgImageButtonImage.sprite = sprite;

        serverBgImage.SetActive(false);
    }
}