using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public enum MessageType
{
    ERROR = 1,
    SUCCESS = 2,
    EMPTY = 3
}

public class NetworkManager
{
    public static NetworkManager Instance;

    private string _host;
    private int _port;

    public NetworkManager(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public void GetRequest(string uri, string query, Action<MessageType, string> Callback)
    {
        GameManager.Instance.StartCoroutine(GetCoroutine(uri, query, Callback));
    }

    private IEnumerator GetCoroutine(string uri, string query, Action<MessageType, string> Callback)
    {
        string url = $"{_host}:{_port}/{uri}{query}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        //해당 url로 웹브라우저 주소창에 친거랑 동일한 짓을 한다.
        SetRequestToken(req); //만약 토큰이 있으면 토큰을 헤더에 셋팅해서 보낸다.

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Callback?.Invoke(MessageType.ERROR, $"{req.responseCode}_Error on Get");
            yield break;
        }

        //클래스로 따야겠지
        MessageDTO msg = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);

        Callback?.Invoke(msg.type, msg.message);

        req.Dispose();
    }

    public void PostRequest(string uri, PayLoad payload, Action<MessageType, string> Callback)
    {
        GameManager.Instance.StartCoroutine(PostCoroutine(uri, payload, Callback));
    }

    private IEnumerator PostCoroutine(string uri, PayLoad payload, Action<MessageType, string> Callback)
    {
        string url = $"{_host}:{_port}/{uri}";
        //Debug.Log(payload.GetJsonString());
        //UnityWebRequest req = UnityWebRequest.Post(url, payload.GetJsonString(), "application/json");

        UnityWebRequest req = UnityWebRequest.Post(url, payload.GetWWWForm());
        //req.SetRequestHeader("Content-Type", "application/json");
        //여기에가 토큰 셋팅도 해줘야 한다.
        SetRequestToken(req); //만약 토큰이 있으면 토큰을 헤더에 셋팅해서 보낸다.

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(req.responseCode);
            //UIController.Instance.Message.AddMessage($"요청이 실패했습니다. {req.responseCode} Error on post", 3f);
            yield break;
        }

        MessageDTO msg = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);
        Callback?.Invoke(msg.type, msg.message);

        req.Dispose();
    }

    public void DoAuth()
    {
        GetRequest("user", "", (type, json) =>
        {
            if (type == MessageType.SUCCESS)
            {
                //Debug.Log(json); //즉 내가 토큰을 보유하고 있다면 로그인정보들을 받을 수 있다.
                UserVO user = JsonUtility.FromJson<UserVO>(json);
                UIController.Instance.SetLogin(user);       // 토큰으로 로그인
            }
        });
    }

    private void SetRequestToken(UnityWebRequest req)
    {
        if (!string.IsNullOrEmpty(GameManager.Instance.Token))
        {
            req.SetRequestHeader("Authorization", $"Bearer{GameManager.Instance.Token}");
        }
    }
}