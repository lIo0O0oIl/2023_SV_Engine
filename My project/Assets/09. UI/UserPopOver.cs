using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UserPopOver
{
    private VisualElement _root;

    private Label _nameLabel;
    private Label _emailLabel;
    private Label _expLabel;

    public string Username
    {
        get => _nameLabel.text;
        set => _nameLabel.text = value;
    }

    public string Email
    {
        get => _emailLabel.text;
        set => _emailLabel.text = value;
    }

    private int _exp;
    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            _expLabel.text = _exp.ToString();
        }
    }

    public UserPopOver(VisualElement root)
    {
        _root = root;
        _nameLabel = root.Q<Label>("NameLabel");
        _emailLabel = root.Q<Label>("EmailLabel");
        _expLabel = root.Q<Label>("EXPLabel");
    }

    public void PopOver(Vector2 pos)
    {
        _root.style.top = new Length(pos.y, LengthUnit.Pixel);
        _root.style.left = new Length(pos.y, LengthUnit.Pixel);

        _root.transform.scale = new Vector3(1, 1, 1);
    }

    public void Hide()
    {
        _root.transform.scale = new Vector3(1, 0, 1);
    }
}
