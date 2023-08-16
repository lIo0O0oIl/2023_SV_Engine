using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum Windows
{
    Lunch = 1,
    Login = 2,
    Inven = 3,
    Game = 4,
}
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public List<ItemSO> itemList;

    [SerializeField] private VisualTreeAsset _lunchUIAsset; //UI의 프리팹
    [SerializeField] private VisualTreeAsset _loginUIAsset; //UI의 프리팹
    [SerializeField] private VisualTreeAsset _invenUIAsset;
    [SerializeField] private VisualTreeAsset _invenItemUIAsset;
    [SerializeField] private VisualTreeAsset _GameUIAsset;

    private UIDocument _uiDocument;
    private VisualElement _contentParent;
    private MessageSystem _messageSystem;
    public MessageSystem Message => _messageSystem;

    private Dictionary<Windows, WindowUI> _windowDictionary = new Dictionary<Windows, WindowUI>();

    private Button _loginBtn;
    private UserInfoPanel _userInfoPanel;

    GameUI gameUI;

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

        _loginBtn = root.Q<Button>("LoginBtn");
        _loginBtn.RegisterCallback<ClickEvent>(OnOpenLoginHandle);
        _contentParent = root.Q<VisualElement>("Content");

        var popOverElem = root.Q<VisualElement>("UserPopOver");
        var userInfoElem = root.Q<VisualElement>("UserInfoPanel");
        _userInfoPanel = new UserInfoPanel(userInfoElem, popOverElem, e => SetLogout());        // 토큰으로

        Button invenBtn = root.Q<Button>("InventoryBtn");
        invenBtn.RegisterCallback<ClickEvent>(OnOpenInvenHandle);

        var messageContainer = root.Q<VisualElement>("MessageContainer");
        _messageSystem.SetContainer(messageContainer);

        // 게임탑바로 UI 변경하기
        TopBarUIController TopBarGame = new TopBarUIController(root.Q<VisualElement>("GamePanel"), root.Q<VisualElement>("BasicPanel"), _userInfoPanel);

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

        VisualElement invenRoot = _invenUIAsset.Instantiate().Q<VisualElement>("InventoryBody");
        _contentParent.Add(invenRoot);
        InventoryUI invenUI = new InventoryUI(invenRoot, _invenItemUIAsset);
        invenUI.Close();
        _windowDictionary.Add(Windows.Inven, invenUI);

        VisualElement gameRoot = _GameUIAsset.Instantiate().Q<VisualElement>("GameWindow");
        VisualElement rankingRoot = _GameUIAsset.Instantiate().Q<VisualElement>("RankingWindow");
        _contentParent.Add(gameRoot);
        _contentParent.Add(rankingRoot);
        gameUI = new GameUI(gameRoot, TopBarGame, root, rankingRoot);
        gameUI.Close();
        _windowDictionary.Add(Windows.Game, gameUI);
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

    private void OnOpenInvenHandle(ClickEvent evt)
    {
        foreach (var kvPair in _windowDictionary)
        {
            kvPair.Value.Close();
        }
        _windowDictionary[Windows.Inven].Open();
    }

    public void OnOpenGameHandle(ClickEvent evt)
    {
        foreach (var kvPair in _windowDictionary)
        {
            kvPair.Value.Close();
        }
        _windowDictionary[Windows.Game].Open();
    }

    public void OnOpenRankingHandle(bool show)
    {
        gameUI.OnOpenRankingWindow(show);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1번 숫자 누르면 아이템 추가되게
        {
            int idx = Random.Range(0, itemList.Count);

            InventoryUI inven = _windowDictionary[Windows.Inven] as InventoryUI;
            inven.AddItem(itemList[idx], 3);
        }
    }

    public void SetLogin(UserVO user)
    {
        _loginBtn.style.display = DisplayStyle.None;
        //로그인이 되었을 SetLogin을 실행시켜라
        //_userInfoPanel 을 보이게 만들고
        _userInfoPanel.Show(true);
        // UserInfoPanel에 있는 버튼중에서 InfoBtn에 text로 유저의 이름을 넣어줘라.
        _userInfoPanel.User = user;
    }

    public void SetLogout()
    {
        Debug.Log("로그아웃됨");
        _loginBtn.style.display = DisplayStyle.Flex;
        _userInfoPanel.Show(false);
        GameManager.Instance.DestroyToken();
    }

    // 이벤트로 넣어주기
    public void ClickBtnClick()
    {
        gameUI.Click();
    }
}
