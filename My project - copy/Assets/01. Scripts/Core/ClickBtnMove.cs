using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBtnMove : MonoBehaviour
{
    [SerializeField] private GameObject ClickBtn;
    private Animator moveAnim;

    private void Start()
    {
        moveAnim = ClickBtn.GetComponent<Animator>();
        //moveAnim.SetTrigger("isMove");      // 디버그용
    }

    public void ClickStart()
    {
        ClickBtn.SetActive(true);
        moveAnim.SetTrigger("isMove");
    }

    public void ClickEnd()
    {
        ClickBtn.SetActive(false);
    }
}
