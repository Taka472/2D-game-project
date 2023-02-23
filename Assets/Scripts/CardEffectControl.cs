using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardEffectControl : MonoBehaviour
{
    public static CardEffectControl instance;
    public Image cardEffectNotice;

    public Image spellChainNotice;

    public bool EffectPermit;
    public int needSelected;
    public List<GameObject> effectChain = new();

    public GameObject debuffEffect;
    public GameObject buffEffect;

    void Start()
    {
        instance = this;
    }

    public void CardIdCheck(GameObject card, bool activate)
    {
        switch (card.GetComponent<ScriptableCard>().id)
        {
            case 1:
                {
                    if (!activate && GameControl.instance.monsterOnField > 0 && GameControl.instance.MainPhase) {
                        effectChain.Add(card);
                        needSelected++;
                        GameControl.instance.selectForEffect = true;
                        GameControl.instance.EffectPhase();
                        CreateEffectNotice(card);
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1 && GameControl.instance.MainPhase)
                    {
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health += 1;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack += 1;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health.ToString();
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                        Buff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                    }
                    break;
                }
            case 2:
                {
                    if (GameControl.instance.monsterOnField == 0 && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        if (card.transform.parent.name != "Graveyard")
                            GameControl.instance.SendToGraveYard(card);
                    }
                    else if (!activate && GameControl.instance.MainPhase)
                    {
                        effectChain.Add(card);
                        needSelected++;
                        GameControl.instance.selectForEffect = true;
                        GameControl.instance.EffectPhase();
                        CreateEffectNotice(card);
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1 && card.transform.parent.name != "Graveyard")
                    {
                        if (card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Enem" ||
                            card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Card")
                        {
                            if (card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().type == Type.dragon)
                            {
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health += 3;
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack += 2;
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health.ToString();
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                                Buff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                            }
                            else
                            {
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health += 1;
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack += 1;
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health.ToString();
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                                Buff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                            }
                        }
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
            case 3:
                {
                    if (!activate && GameControl.instance.enemyMonsterOnField > 0 && GameControl.instance.MainPhase)
                    {
                        effectChain.Add(card);
                        needSelected++;
                        GameControl.instance.selectForEffect = true;
                        GameControl.instance.EffectPhase();
                        CreateEffectNotice(card);
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1 && GameControl.instance.MainPhase && !card.GetComponent<ScriptableCard>().isBoosted)
                    {
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack -= 1;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                        card.GetComponent<ScriptableCard>().isBoosted = true;
                        Debuff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                    }
                    break;
                }
            case 4:
                {
                    if ((GameControl.instance.monsterOnField == 0 || GameControl.instance.enemyMonsterOnField == 0) && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        if (card.transform.parent.name != "Graveyard")
                            GameControl.instance.SendToGraveYard(card);
                    }
                    else if (!activate)
                    {
                        effectChain.Add(card);
                        needSelected += 2;
                        GameControl.instance.selectForEffect = true;
                        GameControl.instance.EffectPhase();
                        CreateEffectNotice(card);
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 2 && GameControl.instance.monsterOnField > 0 && GameControl.instance.enemyMonsterOnField > 0 && GameControl.instance.MainPhase)
                    {
                        if ((card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Enem" &&
                            card.GetComponent<ScriptableCard>().effectTarget[1].transform.parent.name[..4] == "Card") ||
                            (card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Card" &&
                            card.GetComponent<ScriptableCard>().effectTarget[1].transform.parent.name[..4] == "Enem"))
                        {
                            GameControl.instance.SendToGraveYard(card.GetComponent<ScriptableCard>().effectTarget[1]);
                            GameControl.instance.SendToGraveYard(card.GetComponent<ScriptableCard>().effectTarget[0]);
                        }
                        card.GetComponent<ScriptableCard>().effectTarget.Clear();
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
            case 5:
                {
                    if (activate)
                    {
                        card.GetComponent<ScriptableCard>().isInvicible = 1;
                    }
                    break;
                }
            case 6:
                {
                    if (!activate && card.transform.parent.name != "Graveyard" && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        effectChain.Add(card);
                        GameControl.instance.EffectUpdate(effectChain);
                    }
                    else if (activate && GameControl.instance.MainPhase && card.transform.parent.name != "Graveyard")
                    {
                        StartCoroutine(PlayerDeck.instance.DrawAmount(1));
                    }
                    break;
                }
            case 7:
                {
                    if (activate && card.transform.parent.name == "Graveyard" && !card.GetComponent<ScriptableCard>().isBoosted)
                    {
                        StartCoroutine(PlayerDeck.instance.DrawCard());
                        card.GetComponent<ScriptableCard>().isBoosted = true;
                    }
                    break;
                }
            case 8:
                {
                    if (!activate && card.transform.parent.name != "Graveyard" && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        effectChain.Add(card);
                        GameControl.instance.EffectUpdate(effectChain);
                    }
                    else if (card.transform.parent.name != "Graveyard" && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        PlayerDeck.instance.Shuffle();
                        StartCoroutine(PlayerDeck.instance.DrawCard());
                        EnemyDeck.instance.Shuffle();                        
                        StartCoroutine(EnemyDeck.instance.DrawCard());
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
            case 9:
                {
                    if (card.transform.parent.name != "Graveyard" && !card.GetComponent<ScriptableCard>().isBoosted && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        StartCoroutine(PlayerDeck.instance.DrawAmount(6 - GameControl.instance.cardInHand));
                        StartCoroutine(EnemyDeck.instance.DrawAmount(6 - GameControl.instance.cardInEnemyHand));
                        GameControl.instance.SendToGraveYard(card);
                        card.GetComponent<ScriptableCard>().isBoosted = true;
                    }
                    break;
                }
            case 12:
                {
                    int count = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GameControl.instance.spellZone[i].transform.childCount == 1)
                        {
                            if (GameControl.instance.spellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id == 12 && !card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                                if (count == 0) count++;
                                else GameControl.instance.SendToGraveYard(card);
                        }
                    }
                    if (card.transform.parent.name != "Graveyard")
                    {
                        for (int i = 0; i < 5; i++)
                        { 
                            if (GameControl.instance.monsterZone[i].transform.childCount == 1)
                            {
                                if (GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type == Type.dragon && !GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBoosted)
                                {
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack += 2;
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attackText.text = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack.ToString();
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBoosted = true;
                                    Buff(GameControl.instance.monsterZone[i].transform.GetChild(0).gameObject);
                                }
                            }
                        }
                        card.GetComponent<ScriptableCard>().isBoosted = true;
                    }
                    else if (card.GetComponent<ScriptableCard>().isBoosted)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GameControl.instance.monsterZone[i].transform.childCount == 1)
                            {
                                if (GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type == Type.dragon && GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBoosted)
                                {
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack -= 2;
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attackText.text = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack.ToString();
                                    GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBoosted = false;
                                    Debuff(GameControl.instance.monsterZone[i].transform.GetChild(0).gameObject);
                                }
                            }
                        }
                        card.GetComponent<ScriptableCard>().isBoosted = false;
                    }
                    break;
                }
            case 15:
                {
                    if (GameControl.instance.monsterOnField == 0 && card.GetComponent<ScriptableCard>().cardBack.activeSelf)
                    {
                        GameControl.instance.SendToGraveYard(card);
                    }
                    else if (!activate && GameControl.instance.monsterOnField > 0 && GameControl.instance.MainPhase)
                    {
                        effectChain.Add(card);
                        needSelected++;
                        GameControl.instance.selectForEffect = true;
                        GameControl.instance.EffectPhase();
                        CreateEffectNotice(card);
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1) 
                    {
                        int temp = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health = temp;
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                        card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health.ToString();
                        Buff(card.GetComponent<ScriptableCard>().effectTarget[0]);
                        card.GetComponent<ScriptableCard>().effectTarget.Clear();
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
        }
    }

    public void CardTypeCheck(GameObject card, bool activate)
    {
        switch (card.GetComponent<ScriptableCard>().type)
        {
            case Type.dragon:
                {
                    if (GameControl.instance.Attack && card.GetComponent<ScriptableCard>().target.GetComponent<ScriptableCard>() != null)
                    {
                        card.GetComponent<ScriptableCard>().target.GetComponent<ScriptableCard>().isBurn = 3;
                    }
                    break;
                }
            case Type.angel:
                {
                    if (!activate && GameControl.instance.monsterOnField > 1)
                    {
                        if (GameControl.instance.MainPhase)
                        { 
                            effectChain.Add(card);
                            GameControl.instance.selectForEffect = true;
                            GameControl.instance.EffectPhase();
                            needSelected += 1;
                            CreatePassiveNotice("Heal an ally 2");
                        }
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1) 
                    {
                        if (card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Enem" ||
                                card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Card")
                        {
                            card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health += 2;
                            card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().health.ToString();
                            Buff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                            card.GetComponent<ScriptableCard>().effectTarget.Clear();
                        }
                    }
                    else card.GetComponent<ScriptableCard>().effectTarget.Clear();
                    break;
                }
            case Type.demon:
                {
                    if (GameControl.instance.Attack && GameControl.instance.canDirectAttack && card.GetComponent<ScriptableCard>().haveAttack)
                    {
                        if (card.GetComponent<ScriptableCard>().health + card.GetComponent<ScriptableCard>().attack > card.GetComponent<ScriptableCard>().originalHealth)
                        {
                            card.GetComponent<ScriptableCard>().health = card.GetComponent<ScriptableCard>().originalHealth;
                            card.GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().health.ToString();
                        }
                        else
                        {
                            card.GetComponent<ScriptableCard>().health += card.GetComponent<ScriptableCard>().attack;
                            card.GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().health.ToString();
                        }
                    }
                    break;
                }
            case Type.witch:
                {
                    if (!activate && GameControl.instance.enemyMonsterOnField > 0)
                    {
                        if (GameControl.instance.MainPhase)
                        {
                            effectChain.Add(card);
                            GameControl.instance.selectForEffect = true;
                            GameControl.instance.EffectPhase();
                            CreatePassiveNotice("-2/0 to one unit");
                            needSelected += 1;
                        }
                    }
                    else if (card.GetComponent<ScriptableCard>().effectTarget.Count == 1)
                    {
                        if (card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Enem" ||
                                card.GetComponent<ScriptableCard>().effectTarget[0].transform.parent.name[..4] == "Card")
                        {
                            card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack -= 2;
                            if (card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack < 0)
                                card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack = 0;
                            card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().effectTarget[0].GetComponent<ScriptableCard>().attack.ToString();
                            Debuff(card.GetComponent<ScriptableCard>().effectTarget[0].gameObject);
                        }
                        card.GetComponent<ScriptableCard>().effectTarget.Clear();
                    }
                    else card.GetComponent<ScriptableCard>().effectTarget.Clear();
                    break;
                }
            case Type.golem:
                {
                    if (GameControl.instance.MainPhase)
                    {
                        card.GetComponent<ScriptableCard>().isShielded = true;
                    }
                    break;
                }
        }
    }

    void CreateEffectNotice(GameObject card)
    {
        Image temp = Instantiate(cardEffectNotice, GameObject.Find("Canvas").transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector3(804, 30);
        temp.transform.GetChild(0).GetComponent<Text>().text = card.GetComponent<ScriptableCard>().description;
    }

    void CreatePassiveNotice(string passiveEffect)
    {
        Image temp = Instantiate(cardEffectNotice, GameObject.Find("Canvas").transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector3(804, 30);
        temp.transform.GetChild(0).GetComponent<Text>().text = passiveEffect;
    }

    async void Buff(GameObject target)
    {
        GameObject temp = Instantiate(buffEffect);
        temp.transform.position = target.transform.position;
        await Task.Delay(1000);
        Destroy(temp);
    }

    async void Debuff(GameObject target)
    {
        GameObject temp = Instantiate(debuffEffect);
        temp.transform.position = target.transform.position;
        await Task.Delay(1000);
        Destroy(temp);
    }
}
