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

    //private Label _recordLabel;
    struct Record
    {
        public Label name;
        public Label score;
    }
    private Record[] _records = new Record[3];

    private VisualElement _BigRoot;

    private VisualElement _RankingRoot;

    public GameUI(VisualElement root, TopBarUIController topBarUI, VisualElement bigRoot, VisualElement rankingRoot) : base(root)
    {
        _RankingRoot = rankingRoot;

        topBarUIController = topBarUI;
        _BigRoot = bigRoot.Q<VisualElement>("TopBar");
        topBarUIController.mainGoBtn.RegisterCallback<ClickEvent>(OnCancelBtnHandle);

        _clickBtn = root.Q<Button>("ClickBtn");
        _clickBtn.RegisterCallback<ClickEvent>((evt) => GameManager.Instance.StartCoroutine(GameStart()));
        _scoreLabel = root.Q<Label>("ScoreLabel");
        _timeLabel = root.Q<Label>("TimeLabel");
        for (int i = 0; i < _records.Length; i++)
        {
            VisualElement number = rankingRoot.Q<VisualElement>((i + 1).ToString());
            _records[i].name = number.Q<Label>("NameLabel");
            _records[i].score = number.Q<Label>("ScoreLabel");
        }

        Button rankingCancelBtn = rankingRoot.Q<Button>("CancelBtn");
        rankingCancelBtn.RegisterCallback<ClickEvent>(evt => OnOpenRankingWindow(false));

        GameManager.Instance.StartCoroutine(Timer());

        NetworkManager.Instance.GetRequest("record", "", (type, json) =>
        {
            RecordList list = JsonUtility.FromJson<RecordList>(json);
            int now = 0;
            list.list.ForEach(item =>
            {
                _records[now].name.text = item.name;
                _records[now].score.text = item.score.ToString();
                now++;
            });
        });

    }

    public void OnOpenRankingWindow(bool show)
    {
        if (show)
            _RankingRoot.RemoveFromClassList("fade");
        else
            _RankingRoot.AddToClassList("fade");
        Debug.Log($"앗 {show}");
    }

    public void Click()
    {
        if (_isStart && _isTimerStart)
        {
            //Debug.Log("클릭됨");
            ++_score;
            _scoreLabel.text = _score.ToString();
        }
    }

    private IEnumerator GameStart()
    //public IEnumerator GameStart()
    {
        if (!_isStart)
        {
            _isStart = true;

            //_BigRoot.AddToClassList("visibility");
            _BigRoot.style.bottom = 120;

            for (int i = 3; i >= 1; i--)
            {
                _clickBtn.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            //_clickBtn.text = "Click Here!";       // 클릭시작
            _clickBtn.style.display = DisplayStyle.None;
            GameManager.Instance.ClickStart();
            //_clickBtn.RegisterCallback<ClickEvent>(Click);

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
                    _timeLabel.text = string.Format("0.00");
                    GameManager.Instance.ClickEnd();
                    _clickBtn.text = "Game End!";
                    _clickBtn.style.display = DisplayStyle.Flex;
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

            //_BigRoot.RemoveFromClassList("visibility");

            _clickBtn.text = "Game End!\nRe?";
            _isStart = false;
            _time = 10;

            // 리코드 작성
            RecordVO vo = new RecordVO { score = _score };
            NetworkManager.Instance.PostRequest("record", vo, (type, json) =>
            {
                //Debug.Log(json);
                UIController.Instance.Message.AddMessage(json, 3f);
                RankingUpdate();
            });

            _BigRoot.style.bottom = 0;

            _score = 0;
        }
    }

    public void RankingUpdate()
    {
        NetworkManager.Instance.GetRequest("record", "", (type, json) =>
        {
            RecordList list = JsonUtility.FromJson<RecordList>(json);
            int now = 0;
            list.list.ForEach(item =>
            {
                _records[now].name.text = item.name;
                _records[now].score.text = item.score.ToString();
                now++;
            });
        });
        Debug.Log("업데이트 완료!");
    }

    private void OnCancelBtnHandle(ClickEvent evt)
    {
        Close();
    }
}
