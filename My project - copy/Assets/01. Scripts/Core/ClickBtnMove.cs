using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBtnMove : MonoBehaviour
{
    [SerializeField] private GameObject ClickBtn;
    [SerializeField] private AudioSource bye, pew;

    private void Awake()
    {
        
    }

    public void ClickStart()
    {
        ClickBtn.SetActive(true);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(1f);
        bye.Play();

    }

    public void ClickEnd()
    {

        ClickBtn.SetActive(false);
    }
}
