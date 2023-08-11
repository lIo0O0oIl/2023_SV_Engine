using UnityEngine;
using UnityEngine.UIElements;

public class UserInfoPanel
{
    private VisualElement _root;
    private Button _infoButton;
    public UserPopOver UserPopOver { get; private set; }

    private UserVO _user;
    public UserVO User
    {
        get => _user;
        set
        {
            _user = value;
            _infoButton.text = _user.name;
            UserPopOver.Username = _user.name;
            UserPopOver.Email = _user.email;
            UserPopOver.Exp = _user.exp;
        }
    }

    public UserInfoPanel(VisualElement root, VisualElement popOverElem, EventCallback<ClickEvent> LogoutHandler)
    {
        _root = root;
        _infoButton = root.Q<Button>("InfoBtn");

        root.Q<Button>("LogoutBtn").RegisterCallback<ClickEvent>(LogoutHandler);

        UserPopOver = new UserPopOver(popOverElem); //팝오버 창 보이게 하고
        _infoButton.RegisterCallback<MouseEnterEvent>(evt =>
        {
            Rect rect = _infoButton.worldBound;
            Vector2 pos = rect.position;
            pos.y += rect.height + 10;
            UserPopOver.PopOver(pos);
        });
        _infoButton.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            UserPopOver.Hide();
        });

    }

    public void Show(bool value)
    {
        if (value)
            _root.RemoveFromClassList("widthzero");
        else
            _root.AddToClassList("widthzero");
    }
}
