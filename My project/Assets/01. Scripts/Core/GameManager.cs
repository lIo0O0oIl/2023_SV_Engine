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

    #region 디버그 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 콜백, async await
            NetworkManager.Instance.GetRequest("lunch", "?date=20230704", (type, message) =>
            {
                if (type == MessageType.SUCCESS)
                {
                    LunchVO lunch = JsonUtility.FromJson<LunchVO>(message);

                    foreach (string menu in lunch.menus)
                    {
                        Debug.Log(menu);
                    }
                }
                else
                {
                    Debug.Log(message);
                }
            });
        }
    }

    #endregion

    public void DestroyToken()
    {
        PlayerPrefs.DeleteKey(LoginUI.TokenKey);
        Token = string.Empty;
    }
}
