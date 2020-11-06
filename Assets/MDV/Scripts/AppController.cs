using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class AppController : MonoBehaviour
{
    public static AppController instance;
    [SerializeField] private UniWebView web;
    [SerializeField] private string _videoId;
    [SerializeField] private string _userId;
    [SerializeField] private string _role;
    [SerializeField] private string clearType = "";
    [SerializeField] private bool sessionStarted = false;
    [SerializeField] private GameObject loader;
    [SerializeField] private bool redirectToHistory = false;
    // [SerializeField] private GameObject showSessionButton;


    [SerializeField] private bool viewCallbackLaunched = false;
    [SerializeField] private bool hideCallbackLaunched = false;
    // Use this for initialization
    void Start()
    {
        // showSessionButton.SetActive(false);
        if (instance == null)
            instance = this;
        web.OnMessageReceived += (view, message) =>
        {
            // Debug.Log("RECEIVING SCHEMA: " + message.RawMessage);

            if (message.Path.Equals("video") || message.Path.Equals("call"))
            {
                string token = UnityWebRequest.UnEscapeURL(message.Args["token"]);
                token = token.Replace("{{E}}", "=");
                _userId = UnityWebRequest.UnEscapeURL(message.Args["userId"]);
                _role = UnityWebRequest.UnEscapeURL(message.Args["role"]);
                // Debug.Log(_userId);
                _videoId = UnityWebRequest.UnEscapeURL(message.Args["idVideo"]);
                // Debug.Log(_videoId);
                string sessionId = UnityWebRequest.UnEscapeURL(message.Args["sessionId"]);
                string action = UnityWebRequest.UnEscapeURL(message.Path);

                if (action.Contains("video"))
                {
                    clearType = "video";
                }
                else if (action.Contains("call"))
                {
                    clearType = "audio";
                }
                OTSDKIOS.StartOTSession("46214682", sessionId, token, clearType, _role);
                sessionStarted = true;
                hideCallbackLaunched = false;
                viewCallbackLaunched = false;
            }
            if (message.Path.Equals("action"))
            {
                if (message.Args["show"].Equals("true"))
                {
                    hideCallbackLaunched = false;
                    OTSDKIOS.ShowSessionView();
                }
            }
            if (message.Path.Equals("redirect"))
            {
                Debug.Log(UnityWebRequest.UnEscapeURL(message.Args["url"]));
                Application.OpenURL("https://"+UnityWebRequest.UnEscapeURL(message.Args["url"]));
            }
        };

        // web.OnPageStarted += (view, url) =>
        // {
        //     loader.SetActive(true);
        // };

        web.OnPageFinished += (view, statusCode, url) =>
        {
            Debug.Log(url);

            loader.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (sessionStarted)
        {
            // Debug.Log("Session Active");
            // Debug.Log(OTSDKIOS.IsConnected());

            if (OTSDKIOS.IsCallback1() && OTSDKIOS.IsCallback2())
            {
                loader.SetActive(true);
                Debug.Log("Session Disconnected");
                sessionStarted = false;
                StartCoroutine(EndCall());
            }
            // if (!hideCallbackLaunched)
            // {
            //     loader.SetActive(true);
            //     if (OTSDKIOS.IsViewHidden())
            //     {
            //         if (_role.Equals("doctor"))
            //         {
            //             hideCallbackLaunched = OTSDKIOS.IsViewHidden();
            //             showSessionButton.SetActive(true);
            //             loader.SetActive(true);
            //             web.Load("http://telemedapp.meteorapp.com/medical-history-session-uniweb?idVideo=" + _videoId);
            //         }

            //     }
            // }
            // if (!viewCallbackLaunched)
            // {
            //     if (OTSDKIOS.IsViewShowing())
            //     {
            //         viewCallbackLaunched = OTSDKIOS.IsViewShowing();
            //         showSessionButton.SetActive(false);
            //     }
            // }
        }
    }

    public void SessionHasEnded()
    {
        StartCoroutine(EndCall());
    }

    public void ShowVideoView()
    {
        viewCallbackLaunched = false;
        OTSDKIOS.ShowSessionView();
    }


    private IEnumerator EndCall()
    {
        UnityWebRequest webRequest = UnityWebRequest.Post("https://midoctorvirtual.com/video/end", string.Empty);
        VideoData vd = new VideoData
        {
            userId = _userId,
            idVideo = _videoId
        };
        string json = JsonUtility.ToJson(vd);
        Debug.Log(json);
        UploadHandlerRaw uH = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        webRequest.uploadHandler = uH;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if (string.IsNullOrEmpty(webRequest.error))
        {
            EndCallResponse endCall = JsonUtility.FromJson<EndCallResponse>(webRequest.downloadHandler.text);
            if (endCall.status.Equals("200"))
            {
                sessionStarted = false;
                loader.SetActive(true);
                if (!_role.Equals("doctor"))
                {
                    web.Load(endCall.url);
                }
            }
        }
    }
}
