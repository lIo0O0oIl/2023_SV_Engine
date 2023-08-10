using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class LunchUI : WindowUI
{
    private TextField _dateTextField;
    private Label _lunchLable;

    public LunchUI(VisualElement root) : base(root)
    {
        _root = root;
        _dateTextField = root.Q<TextField>(name: "DateTextField");
        root.Q<Button>(name: "LoadBtn").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        root.Q<Button>(name: "CloseBtn").RegisterCallback<ClickEvent>(OnCloseButtonHandle);
        _lunchLable = root.Q<Label>(name: "LunchLabel");
    }

    private void OnLoadButtonHandle(ClickEvent evt)
    {
        string dateStr = _dateTextField.value;
        Regex regex = new Regex(@"20[0-9]{2}[0-1][0-9][0-3][0-9]");
        if (!regex.IsMatch(dateStr))
        {
            UIController.Instance.Message.AddMessage("올바른 날짜 형식을 지켜주세요 (ex. 20230703)", 3f);
            return;
        }

        NetworkManager.Instance.GetRequest("lunch", $"?date={dateStr}", (type, json) =>
        {
            if (type == MessageType.SUCCESS)
            {
                LunchVO vo = JsonUtility.FromJson<LunchVO>(json);
                string menuStr = vo.menus.Aggregate("", (sum, x) => sum + x + "\n");

                _lunchLable.text = menuStr;
            }
            else
            {
                Debug.LogError(json);
            }
        });
    }

    private void OnCloseButtonHandle(ClickEvent evt)
    {
        _root.RemoveFromHierarchy();        // 하이어라키에서 날 지워줘
       // _root.AddToClassList("fade");       // ㅋㅋ
    }
}
