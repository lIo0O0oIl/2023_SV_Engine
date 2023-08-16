using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : WindowUI
{
    TopBarUIController topBarUIController;

    private bool _isStart = false;

    private Button _clickBtn;
    private Label _scoreLabel;
    private int _score = 0;

    private Label _timeLabel;
    private float _time = 10;
    private bool _isTimerStart = false;

    private Label _recordLabel;


    public GameUI(VisualElement root, TopBarUIController topBarUI) : base(root)
    {
        topBarUIController = topBarUI;
        topBarUIController.mainGoBtn.RegisterCallback<ClickEvent>(OnCancelBtnHandle);

        _clickBtn = root.Q<Button>("ClickBtn");
        _clickBtn.RegisterCallback<ClickEvent>((evt) => GameManager.Instance.StartCoroutine(GameStart()));
        _scoreLabel = root.Q<Label>("ScoreLabel");
        _timeLabel = root.Q<Label>("TimeLabel");
        _recordLabel = root.Q<Label>("RecordLabel");

        GameManager.Instance.StartCoroutine(Timer());
    }

    private void Click(ClickEvent evt)
    //public void Click(ClickEvent evt)
    {
        if (_isStart && _isTimerStart)
        {
            //Debug.Log("클릭됨");
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

    private IEnumerator Timer()
    {
        while (true)
        {
            if (_isTimerStart)
            {
                Debug.Log("게임 업데이트 코루틴 작동중");
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
                    GameManager.Instance.StartCoroutine(ReStart());
                }
            }
            yield return null;
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

            // 리코드 작성
            RecordVO vo = new RecordVO { score = _score };
            NetworkManager.Instance.PostRequest("record", vo, (type, json) =>
            {
                Debug.Log(json);
            });
            // 여기에 시작 버튼을 지워주는 것이 있었음
        }
    }

    public void RankingUpdate()
    {
        NetworkManager.Instance.GetRequest("record", "", (type, json) =>
        {
            RecordList list = JsonUtility.FromJson<RecordList>(json);
            StringBuilder builder = new StringBuilder();
            list.list.ForEach(item =>
            {
                builder.Append(item.ToString() + "\n");
            });

            _recordLabel.text = builder.ToString();
        });
    }




    private void OnCancelBtnHandle(ClickEvent evt)
    {
        Close();
    }
}
