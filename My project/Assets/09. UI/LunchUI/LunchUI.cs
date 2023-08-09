using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LunchUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private TextField _dateTextField;
    private Label _lunchLable;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _dateTextField = _uiDocument.rootVisualElement.Q<TextField>(name: "DateTextField");
        _uiDocument.rootVisualElement.Q<Button>(name: "LoadBtn").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        _uiDocument.rootVisualElement.Q<Button>(name: "CloseBtn").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        _lunchLable = _uiDocument.rootVisualElement.Q<Label>(name: "LunchLable");
    }

    private void OnLoadButtonHandle(ClickEvent evt)
    {
        string dateStr = _dateTextField.value;

        // 뭔가 함

        string meunStr = "아무튼 메뉴";
        _lunchLable.text = meunStr;
    }

    private void OnCloseButtonHandle(ClickEvent evt)
    {
        // Do nothing
    }
}
