using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickGameUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private bool _isStart = false;

    private UnityEngine.UIElements.Button _clickBtn;
    //[SerializeField] public UnityEngine.UI.Button _clickBtn2;
    private Label _scoreLabel;
    private int _score = 0;

    private Label _timeLabel;
    private float _time = 10;
    private bool _isTimerStart = false;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _clickBtn = root.Q<UnityEngine.UIElements.Button>("ClickBtn");
        _clickBtn.RegisterCallback<ClickEvent>((evt) => StartCoroutine(GameStart()));
        _scoreLabel = root.Q<Label>("ScoreLabel");
        _timeLabel = root.Q<Label>("TimeLabel");
    }

    private void Click(ClickEvent evt)
    //public void Click(ClickEvent evt)
    {
        if (_isStart && _isTimerStart)
        {
            Debug.Log("Å¬¸¯µÊ");
            ++_score;
            _scoreLabel.text = $"Score : {_score}";
        }
    }

    private IEnumerator GameStart()
    //public IEnumerator GameStart()
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
                _time = 0;
                StartCoroutine(ReStart());
            }
        }
    }

    private IEnumerator ReStart()
    //public IEnumerator ReStart()
    {
        if (_isStart && !_isTimerStart)
        {
            yield return new WaitForSeconds(2f);
            _clickBtn.text = "Game End!\nRe?";
            _isStart = false;
            _time = 10;
            _score = 0;
        }
    }
}
