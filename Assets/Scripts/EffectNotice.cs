using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectNotice : MonoBehaviour
{
    public static EffectNotice instance;

    public int numOfChosen;

    void Start()
    {
        instance = this;
    }

    public void RunEffect()
    {
        if (numOfChosen == CardEffectControl.instance.needSelected)
        {
            Destroy(gameObject);
            GameControl.instance.EffectUpdate(CardEffectControl.instance.effectChain);
            numOfChosen = 0;
        }
    }
}
