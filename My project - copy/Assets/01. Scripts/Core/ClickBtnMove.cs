using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBtnMove : MonoBehaviour
{
    [SerializeField] private GameObject ClickBtn;
    private Animator moveAnim;

    [SerializeField] private AudioSource bye, pew;

    private void Start()
    {
        moveAnim = ClickBtn.GetComponent<Animator>();
        moveAnim.SetTrigger("isMove");
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
        moveAnim.SetTrigger("isMove");
    }

    public void ClickEnd()
    {
        ClickBtn.SetActive(false);
    }
}
