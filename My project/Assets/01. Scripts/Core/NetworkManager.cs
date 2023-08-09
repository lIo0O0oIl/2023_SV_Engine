using System;
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
    public void GetRequest(string uri, string query, Action<MessageType, string> Callback)
    {
        GameManager.Instance.StartCoroutine(GetCoroutine(uri, query, Callback));
    }

    private IEnumerator GetCoroutine(string uri, string query, Action<MessageType, string> Callback)
    {
        string url = $"{_host}:{_port}/{uri}{query}";
        UnityWebRequest req = UnityWebRequest.Get(url);
        // �ش� url �� �������� �ּ�â�� ģ�Ŷ� ������ ���� �Ѵ�.

        Debug.Log(url);

        yield return req.SendWebRequest();      // �޾ƿ�

        Debug.Log(req);

        if (req.result != UnityWebRequest.Result.Success)
        {
            Callback?.Invoke(MessageType.ERROR, $"{req.responseCode}_Error on Get");
            yield break;
        }

        // Ŭ������ ������
        MessageDTO msg = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);
        //Debug.Log(req.downloadHandler.text);
        Callback?.Invoke(msg.type, msg.message);
    }
}
