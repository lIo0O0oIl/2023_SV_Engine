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
        // �ش� url �� �������� �ּ�â�� ģ�Ŷ� ������ ���� �Ѵ�.

        yield return req.SendWebRequest();      // ���� �޾ƿ�

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(url);
            Debug.LogError($"{req.responseCode}_Error on Get");
            yield break;
        }

        Debug.Log(req.downloadHandler.text);
    }
}
