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
}
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public List<ItemSO> itemList;

    [SerializeField] private VisualTreeAsset _lunchUIAsset; //UI�� ������
    [SerializeField] private VisualTreeAsset _loginUIAsset; //UI�� ������
    [SerializeField] private VisualTreeAsset _invenUIAsset;
    [SerializeField] private VisualTreeAsset _invenItemUIAsset;

    private UIDocument _uiDocument;
    private VisualElement _contentParent;
    private MessageSystem _messageSystem;
    public MessageSystem Message => _messageSystem;

    private Dictionary<Windows, WindowUI> _windowDictionary = new Dictionary<Windows, WindowUI>();

    private Button _loginBtn;
    private UserInfoPanel _userInfoPanel;

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
        //_userInfoPanel = new UserInfoPanel(userInfoElem, popOverElem, e => SetLogout());
        _userInfoPanel = new UserInfoPanel(userInfoElem, popOverElem, e => SetLogout());

        Button invenBtn = root.Q<Button>("InventoryBtn");
        invenBtn.RegisterCallback<ClickEvent>(OnOpenInvenHandle);

        var messageContainer = root.Q<VisualElement>("MessageContainer");
        _messageSystem.SetContainer(messageContainer);


        #region ������ �߰��ϴ� �κ�
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //1�� ���� ������ ������ �߰��ǰ�
        {
            int idx = Random.Range(0, itemList.Count);

            InventoryUI inven = _windowDictionary[Windows.Inven] as InventoryUI;
            inven.AddItem(itemList[idx], 3);
        }
    }


    public void SetLogin(UserVO user)
    {
        _loginBtn.style.display = DisplayStyle.None;
        //�α����� �Ǿ��� SetLogin�� ������Ѷ�
        //_userInfoPanel �� ���̰� �����
        _userInfoPanel.Show(true);
        // UserInfoPanel�� �ִ� ��ư�߿��� InfoBtn�� text�� ������ �̸��� �־����.
        _userInfoPanel.User = user;
    }

    public void SetLogout()
    {
        Debug.Log("�α׾ƿ���");
        _loginBtn.style.display = DisplayStyle.Flex;
        _userInfoPanel.Show(false);
        GameManager.Instance.DestroyToken();
    }
}
