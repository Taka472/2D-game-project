using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TributeNotice : MonoBehaviour
{
    public TributeNotice instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Confirm()
    {
        GameControl.instance.TributeSummon = true;
        Destroy(gameObject);
    }

    public void Cancel()
    {
        GameControl.instance.TributeSummon = false;
        Destroy(gameObject);
    }

    public void DiscardConfirm()
    {
        GameControl.instance.OutOfNumber = true;
        Destroy(gameObject);
    }
}
