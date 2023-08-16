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

        // 토큰 토큰 관룐
        //Debug.Log("토큰 확인");
        Token = PlayerPrefs.GetString(LoginUI.TokenKey, string.Empty);
        if (!string.IsNullOrEmpty(Token))       // ! 이거 안붙인거 때문이야..?
        {
            NetworkManager.Instance.DoAuth();
        }
    }

    public void DestroyToken()
    {
        PlayerPrefs.DeleteKey(LoginUI.TokenKey);
        Token = string.Empty;
    }

    // 여기부터 게임 관련 코드
    private void Update()
    {

    }
}
