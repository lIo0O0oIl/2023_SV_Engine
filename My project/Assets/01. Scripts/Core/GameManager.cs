using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _host;
    [SerializeField] private int _port;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager is running");
        }
        Instance = this;

        NetworkManager.Instence = new NetworkManager(_host, _port);
    }

    #region 디버그 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // 콜백, async await
            NetworkManager.Instence.GetRequest("lunch", "?date=20230704", (type, message) =>
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
}
