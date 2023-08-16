using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _host;
    [SerializeField] private int _port;

    public static GameManager Instance;

    public string Token { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }
        Instance = this;

        NetworkManager.Instance = new NetworkManager(_host, _port);

        // ��ū ��ū ����
        //Debug.Log("��ū Ȯ��");
        Token = PlayerPrefs.GetString(LoginUI.TokenKey, string.Empty);
        if (!string.IsNullOrEmpty(Token))       // ! �̰� �Ⱥ��ΰ� �����̾�..?
        {
            NetworkManager.Instance.DoAuth();
        }
    }

    public void DestroyToken()
    {
        PlayerPrefs.DeleteKey(LoginUI.TokenKey);
        Token = string.Empty;
    }

    // ������� ���� ���� �ڵ�
    private void Update()
    {

    }
}
