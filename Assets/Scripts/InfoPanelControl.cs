using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelControl : MonoBehaviour
{
    public static InfoPanelControl instance;

    public Image typeImageData;
    public string typeData;
    public string descriptionData;
    public string statusData;
    public string nameData;

    public Image typeImage;
    public Text typeText;
    public Text description;
    public Text status;
    public Text cardName;

    public Button summonButton;
    public Button activateSpell;
    public Button setSpell;
    public Button activateSet;
    public Button discard;

    public GameObject isSelected;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        typeImage.sprite = typeImageData.sprite;
        typeText.text = typeData;
        description.text = descriptionData;
        status.text = statusData;
        cardName.text = nameData;
    }

    public void Close()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
    }

    public void Summon()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        if (isSelected.GetComponent<ScriptableCard>().cost <= 4)
            GameControl.instance.Summon = true;
        else if (isSelected.GetComponent<ScriptableCard>().cost <= 6)
            GameControl.instance.RequestTribute(1);
        else GameControl.instance.RequestTribute(2);
        if (GameControl.instance.ActivateSpell)
            GameControl.instance.ActivateSpell = false;
        if (GameControl.instance.SetSpell)
            GameControl.instance.SetSpell = false;
    }

    public void ActivateEffect()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        GameControl.instance.ActivateSpell = true;
        if (GameControl.instance.Summon)
            GameControl.instance.Summon = false;
        if (GameControl.instance.SetSpell)
            GameControl.instance.SetSpell = false;
    }

    public void SetCard()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        GameControl.instance.SetSpell = true;
        if (GameControl.instance.Summon)
            GameControl.instance.Summon = false;
        if (GameControl.instance.ActivateSpell)
            GameControl.instance.ActivateSpell = false;
    }

    public void ActivateSet()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        isSelected.GetComponent<ScriptableCard>().cardBack.gameObject.SetActive(false);
        CardEffectControl.instance.CardIdCheck(isSelected, false);
    }

    public void DiscardToEndTurn()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        GameControl.instance.Discard();
        if (GameControl.instance.cardInHand < 7) TurnSystem.instance.EndYourTurn();
    }

    public void Discard()
    {
        GameControl.instance.ffx[15].PlayOneShot(GameControl.instance.ffx[15].clip);
        Destroy(gameObject);
        GameControl.instance.Discard();
    }
}
