
using System;
using UnityEngine;

[Serializable]
public class RecordVO : PayLoad
{
    public string name;
    public int user_id;
    public int score;

    public string GetJsonString()
    {
        return JsonUtility.ToJson(this);
    }

    public string GetQueryString()
    {
        return "";
    }

    public WWWForm GetWWWForm()
    {
        var form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("score", score);
        return form;
    }

    public string ToStringShow()
    {
        return $"{name} : {score}";
    }
}
