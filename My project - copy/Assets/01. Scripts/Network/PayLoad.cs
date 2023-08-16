using UnityEngine;

public interface PayLoad
{
    public string GetJsonString();
    public string GetQueryString();
    public WWWForm GetWWWForm();
}