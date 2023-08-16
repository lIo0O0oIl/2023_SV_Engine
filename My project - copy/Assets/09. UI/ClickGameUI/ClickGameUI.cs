using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClickGameUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private bool _isStart = false;

    private Button _clickBtn;
    private Label _scoreLabel;
    private int _score = 0;

    private Label _timeLabel;
    private float _time = 10;
    private bool _isTimerStart = false;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _clickBtn = root.Q<Button>("ClickBtn");
        _clickBtn.RegisterCallback<ClickEvent>((evt) => StartCoroutine(GameStart()));
        _scoreLabel = root.Q<Label>("ScoreLabel");
        _timeLabel = root.Q<Label>("TimeLabel");
    }

    private void Click(ClickEvent evt)
    {
        if (_isStart && _isTimerStart)
        {
            Debug.Log("Å¬¸¯µÊ");
            ++_score;
            _scoreLabel.text = $"Score : {_score}";
        }
    }

    private IEnumerator GameStart()
    {
        if (!_isStart)
        {
            _isStart = true;

            for (int i = 3; i >= 1; i--)
            {
                _clickBtn.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            _clickBtn.text = "Click Here!";

            _clickBtn.RegisterCallback<ClickEvent>(Click);

            _isTimerStart = true;
        }
        yield return null;
    }

    public void GameEnd(ClickEvent evt)
    {
        Debug.Log("³¡³²");
    }

    private void Update()
    {
        if (_isTimerStart) 
        {
            _time -= Time.deltaTime;
            if (_time > 0)
            {
                _timeLabel.text = string.Format("{0:0.00}", _time);
            }
            if (_time < 0)
            {
                _clickBtn.text = "Game End!";
                _isTimerStart = false;
                _clickBtn.RegisterCallback<ClickEvent>(GameEnd);
                _time = 0;
            }
        }
    }
}
