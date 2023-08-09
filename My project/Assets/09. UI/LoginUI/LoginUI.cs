using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginUI : WindowUI
{
    private TextField _emailField;
    private TextField _passwordField;


    public LoginUI(VisualElement root) : base(root)
    {
        _emailField = _root.Q<TextField>("EmailInput");
        _passwordField = _root.Q<TextField>("PasswordInput");

        _root.Q<Button>("OkBtn").RegisterCallback<ClickEvent>(OnLoginBtnHandle);
        _root.Q<Button>("CancelBtn").RegisterCallback<ClickEvent>(OnCancelBtnHandle);
    }

    private void OnLoginBtnHandle(ClickEvent evt)
    {
        //입력값 검증이 들어가야해. 
        LoginDTO loginDTO = new LoginDTO
        {
            email = _emailField.value,
            password = _passwordField.value
        };
        NetworkManager.Instance.PostRequest("user/login", loginDTO, (type, json) =>
        {
            Debug.Log(type);
            Debug.Log(json);
        });
    }

    private void OnCancelBtnHandle(ClickEvent evt)
    {
        //이건 과제로 줄께
    }
}
