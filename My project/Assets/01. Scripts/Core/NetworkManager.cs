using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum MessageType
{
    ERROR = 1,
    SUCCESS = 2,
    ENPTY = 3
}

public class NetworkManager
{
    public static NetworkManager Instence;

    private string _host;
    private int _port;

    public NetworkManager(string host, int port)
    {
        _host = host;
        _port = port;
    }

    // Uniform Resource Locator
    // Uniform Resource Indicator
    public void GetRequest(string uri, string query)
    {
        GameManager.Instance.StartCoroutine(GetCoroutine(uri, query));
    }

    private IEnumerator GetCoroutine(string uri, string query)
    {
        string url = $"{_host}:{_port}/{uri}{query}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        // 해당 url 로 웹브라우저 주소창에 친거랑 동일한 짓을 한다.

        yield return req.SendWebRequest();      // 뭐든 받아옴

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(url);
            Debug.LogError($"{req.responseCode}_Error on Get");
            yield break;
        }

        Debug.Log(req.downloadHandler.text);
    }
}
