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

    public const string TokenKey = "token";         // ��ū ��ū ��ū
    private void OnLoginBtnHandle(ClickEvent evt)
    {
        //�Է°� ������ ������. 
        LoginDTO loginDTO = new LoginDTO
        {
            email = _emailField.value,
            password = _passwordField.value
        };
        NetworkManager.Instance.PostRequest("user/login", loginDTO, (type, json) =>
        {
            if (type == MessageType.SUCCESS)
            {
                TokenResponseDTO dto = JsonUtility.FromJson<TokenResponseDTO>(json);
                PlayerPrefs.SetString(TokenKey, dto.token);
                Debug.Log(dto.token);
                // �α��� â�� ������ ������ ���ϵ���
                UIController.Instance.SetLogin(dto.user);
                Close();
            }
            else
            {
                Debug.Log("�α��� �ȵ�");
                UIController.Instance.Message.AddMessage(json, 3f);
            }
            //Debug.Log(type);
            //Debug.Log(json);
        });
    }

    private void OnCancelBtnHandle(ClickEvent evt)
    {
        //�̰� ������ �ٲ�
        Close();
    }
}
