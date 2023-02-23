using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SpellChain : MonoBehaviour
{
    public static SpellChain instance;
    public bool isClick;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        isClick = false;
    }

    public void Confirm()
    {
        Destroy(gameObject);
        GameControl.instance.spellChainSelect = true;
        GameControl.instance.confirmChainSelect = true;
        isClick = true;
    }

    public void Close()
    {
        Destroy(gameObject);
        GameControl.instance.spellChainSelect = false;
        GameControl.instance.confirmChainSelect = false;
        isClick = true;
    }
}
