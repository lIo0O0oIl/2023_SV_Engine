using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TopBarUIController
{
    // ���� �����ؼ� ���� ����

    private VisualElement _root, basicPanel;
    private UserInfoPanel _userInfoPanel;

    public Button mainGoBtn, rankingBtn;

    public TopBarUIController(VisualElement root, VisualElement basic, UserInfoPanel userInfoPanel)
    {
        _root = root;
        basicPanel = basic;
        _userInfoPanel = userInfoPanel;

        mainGoBtn = root.Q<Button>("BackBtn");
        mainGoBtn.RegisterCallback<ClickEvent>(evt => TopBarGameShow(false));
        mainGoBtn.RegisterCallback<ClickEvent>(evt => UIController.Instance.OnOpenRankingHandle(false)) ;

        rankingBtn = root.Q<Button>("RankingBtn");
        rankingBtn.RegisterCallback<ClickEvent>(evt => UIController.Instance.OnOpenRankingHandle(true)) ;    // ���Ʈ�� ���డ����

        _userInfoPanel.BackButtonClick(value => TopBarGameShow(value));
    }

    public void TopBarGameShow(bool value)
    {
        if (value)
        {
            _root.RemoveFromClassList("widthzero");
            basicPanel.AddToClassList("widthzero");
            _userInfoPanel.Show(false);
        }
        else
        {
            _root.AddToClassList("widthzero");
            basicPanel.RemoveFromClassList("widthzero");
            _userInfoPanel.Show(true);
        }
        //Debug.Log($"����ž�� �۵��� {value}");
    }
}
