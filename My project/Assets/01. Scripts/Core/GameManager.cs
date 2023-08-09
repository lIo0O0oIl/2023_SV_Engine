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

    #region ����� �ڵ�
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            NetworkManager.Instence.GetRequest("lunch", "?date=20230704");
        }
    }
    #endregion
}