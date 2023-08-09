using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Windows
{
    Lunch = 1,
    Login = 2,
}
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private VisualTreeAsset _lunchUIAsset; //UI의 프리팹
    [SerializeField] private VisualTreeAsset _loginUIAsset; //UI의 프리팹

    private UIDocument _uiDocument;
    private VisualElement _contentParent;
    private MessageSystem _messageSystem;
    public MessageSystem Message => _messageSystem;

    private Dictionary<Windows, WindowUI> _windowDictionary = new Dictionary<Windows, WindowUI>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple UI Manager is running");
        }
        Instance = this;

        _uiDocument = GetComponent<UIDocument>();
        _messageSystem = GetComponent<MessageSystem>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        Button lunchBtn = root.Q<Button>("LunchBtn");
        lunchBtn.RegisterCallback<ClickEvent>(OnOpenLunchHandle);
        Button loginBtn = root.Q<Button>("LoginBtn");
        loginBtn.RegisterCallback<ClickEvent>(OnOpenLoginHandle);
        _contentParent = root.Q<VisualElement>("Content");

        var messageContainer = root.Q<VisualElement>("MessageContainer");
        _messageSystem.SetContainer(messageContainer);

        #region 윈도우 추가하는 부분
        _windowDictionary.Clear();

        VisualElement lunchRoot = _lunchUIAsset.Instantiate().Q<VisualElement>("LunchContainer");
        _contentParent.Add(lunchRoot);
        LunchUI lunchUI = new LunchUI(lunchRoot);
        lunchUI.Close();
        _windowDictionary.Add(Windows.Lunch, lunchUI);

        VisualElement loginRoot = _loginUIAsset.Instantiate().Q<VisualElement>("LoginWindow");
        _contentParent.Add(loginRoot);
        LoginUI loginUI = new LoginUI(loginRoot);
        loginUI.Close();
        _windowDictionary.Add(Windows.Login, loginUI);

        #endregion
    }

    private void OnOpenLoginHandle(ClickEvent evt)
    {
        foreach (var kvPair in _windowDictionary)
        {
            kvPair.Value.Close();
        }
        _windowDictionary[Windows.Login].Open();
    }

    private void OnOpenLunchHandle(ClickEvent evt)
    {
        foreach (var kvPair in _windowDictionary)
        {
            kvPair.Value.Close();
        }
        _windowDictionary[Windows.Lunch].Open();
    }
}
