using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AIControl : MonoBehaviour
{
    public static AIControl instance;

    public bool haveSummon;
    public List<GameObject> cardInHand;
    public Image hand;

    public GameObject selectedSlot;

    public bool isSet;

    public int countBelow4;
    public int countBelow6;
    public int countAbove;
    public int countOnField4;
    public int countOnField6;
    public int countSpell;

    public GameObject target;
    public int enemyAttack;

    public GameObject summon;
    public GameObject buff;
    public GameObject debuff;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayYourTurn()
    {
        DragonAIControl();
    }

    async void DragonAIControl()
    {
        haveSummon = false;
        if (GameControl.instance.enemyMonsterOnField == 5 && GameControl.instance.enemySpellOnField == 5)
            await WinAttack();
        await DragonUseSetSpell();
        CountCard();
        for (int i = 0; i < cardInHand.Count; i++)
        {
            CountCard();
            if (cardInHand[i].GetComponent<ScriptableCard>().type != Type.spell && cardInHand[i].GetComponent<ScriptableCard>().canBeSummon)
            {
                if (!haveSummon)
                {
                    if (cardInHand[i].GetComponent<ScriptableCard>().id == 17 && cardInHand.Count > 1)
                    {
                        for (int x = 0; x < cardInHand.Count; x++)
                        {
                            if (cardInHand[x].GetComponent<ScriptableCard>().id == 10 || cardInHand[x].GetComponent<ScriptableCard>().id == 13 || cardInHand[x].GetComponent<ScriptableCard>().type == Type.spell)
                            {
                                cardInHand[x].GetComponent<ScriptableCard>().cardBack.SetActive(false);
                                cardInHand[x].GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180);
                                GameControl.instance.SendToGraveYard(cardInHand[x]);
                                if (cardInHand[x].GetComponent<ScriptableCard>().type != Type.spell)
                                    GameControl.instance.enemyMonsterOnField++;
                                else GameControl.instance.enemySpellOnField++;
                                GameControl.instance.cardInEnemyHand--;
                                cardInHand.RemoveAt(x);
                                if (x < i) i--;
                                SelectingSlot();
                                await OnSummon(i);
                                GameControl.instance.enemyMonsterOnField++;
                                GameControl.instance.cardInEnemyHand--;
                                cardInHand.RemoveAt(i);
                                i = 0;
                                break;
                            }
                        }
                    }
                    else if (countAbove > 0 && countOnField4 + countOnField6 > 1 && cardInHand[i].GetComponent<ScriptableCard>().cost > 6)
                    {
                        SelectingForTribute(2);
                        SelectingSlot();
                        await OnSummon(i);
                        haveSummon = true;
                        GameControl.instance.enemyMonsterOnField++;
                        GameControl.instance.cardInEnemyHand--;
                        cardInHand.RemoveAt(i);
                        i = 0;
                    }
                    else if (countBelow6 > 0 && countOnField4 > 0 && cardInHand[i].GetComponent<ScriptableCard>().cost > 4)
                    {
                        SelectingForTribute(1);
                        SelectingSlot();
                        await OnSummon(i);
                        haveSummon = true;
                        GameControl.instance.enemyMonsterOnField++;
                        GameControl.instance.cardInEnemyHand--;
                        cardInHand.RemoveAt(i);
                        i = 0;
                    }
                    else if (cardInHand[i].GetComponent<ScriptableCard>().cost <= 4)
                    {
                        SelectingSlot();
                        await OnSummon(i);
                        haveSummon = true;
                        GameControl.instance.enemyMonsterOnField++;
                        GameControl.instance.cardInEnemyHand--;
                        cardInHand.RemoveAt(i);
                        i = 0;
                    }
                }
            }
            else if (GameControl.instance.enemySpellOnField != 5)
            {
                if (GameControl.instance.enemyMonsterOnField == 0)
                    isSet = true;
                else isSet = false;
                if (cardInHand[i].GetComponent<ScriptableCard>().id == 2)
                {
                    SelectingSpellSlot();
                    await UseSpell(i);
                    GameControl.instance.enemySpellOnField++;
                    GameControl.instance.cardInEnemyHand--;
                    cardInHand.RemoveAt(i);
                    i = 0;
                }
                else if (cardInHand[i].GetComponent<ScriptableCard>().id == 12)
                {
                    SelectingSpellSlot();
                    await UseSpell(i);
                    GameControl.instance.enemySpellOnField++;
                    GameControl.instance.cardInEnemyHand--;
                    cardInHand.RemoveAt(i);
                    i = 0;
                }
                else if (cardInHand[i].GetComponent<ScriptableCard>().id == 14 && GameControl.instance.monsterInEnemyGraveYard > 0 && countOnField4 > 0)
                {
                    SelectingSpellSlot();
                    await UseSpell(i);
                    GameControl.instance.enemySpellOnField++;
                    GameControl.instance.cardInEnemyHand--;
                    cardInHand.RemoveAt(i);
                    i = 0;
                }
                else if (cardInHand[i].GetComponent<ScriptableCard>().id == 19 && GameControl.instance.enemyMonsterOnField > 1)
                {
                    bool isFound = false;
                    for (int x = 0; x < cardInHand.Count; x++)
                    {
                        if (cardInHand[x].GetComponent<ScriptableCard>().id == 18)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound)
                    {
                        SelectingSpellSlot();
                        await UseSpell(i);
                        GameControl.instance.enemySpellOnField++;
                        GameControl.instance.cardInEnemyHand--;
                        cardInHand.RemoveAt(i);
                        i--;
                        for (int x = 0; x < cardInHand.Count; x++)
                        {
                            if (cardInHand[x].GetComponent<ScriptableCard>().id == 18)
                            {
                                SelectingSlot();
                                await OnSummon(x);
                                GameControl.instance.enemyMonsterOnField++;
                                GameControl.instance.cardInEnemyHand--;
                                cardInHand.RemoveAt(x);
                                i = 0;
                                break;
                            }
                        }
                    }
                }
            }
            for (int x = 0; x < 5; x++)
            {
                if (GameControl.instance.enemySpellZone[x].transform.childCount == 1)
                {
                    if (GameControl.instance.enemySpellZone[x].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.activeSelf) continue;
                    if (GameControl.instance.enemySpellZone[x].transform.GetChild(0).GetComponent<ScriptableCard>().id == 12)
                        CheckIDCard(GameControl.instance.enemySpellZone[x].transform.GetChild(0).gameObject);
                }
            }
        }
        CountCard();
        if (!GameControl.instance.isFirst && GameControl.instance.enemyMonsterOnField > 0) await WinAttack();
        EndYourTurn();
    }

    void CountCard()
    {
        countBelow4 = 0;
        countBelow6 = 0;
        countAbove = 0;
        countSpell = 0;
        countOnField4 = 0;
        countOnField6 = 0;
        for (int i = 0; i < cardInHand.Count; i++)
        {
            if (cardInHand[i].GetComponent<ScriptableCard>().type == Type.spell) countSpell++;
            else if (cardInHand[i].GetComponent<ScriptableCard>().cost <= 4) countBelow4++;
            else if (cardInHand[i].GetComponent<ScriptableCard>().cost <= 6) countBelow6++;
            else countAbove++;
            if (i < 5)
            {
                if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                {
                    if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cost <= 4 && GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type != Type.spell)
                        countOnField4++;
                    else if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cost <= 6 && GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type != Type.spell)
                        countOnField6++;
                }
            }
        }
    }

    void SelectingSlot()
    {
        if (GameControl.instance.enemyMonsterZone[2].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemyMonsterZone[2];
        else if (GameControl.instance.enemyMonsterZone[3].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemyMonsterZone[3];
        else if (GameControl.instance.enemyMonsterZone[1].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemyMonsterZone[1];
        else if (GameControl.instance.enemyMonsterZone[4].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemyMonsterZone[4];
        else if (GameControl.instance.enemyMonsterZone[0].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemyMonsterZone[0];
    }

    void SelectingSpellSlot()
    {
        if (GameControl.instance.enemySpellZone[2].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemySpellZone[2];
        else if (GameControl.instance.enemySpellZone[3].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemySpellZone[3];
        else if (GameControl.instance.enemySpellZone[1].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemySpellZone[1];
        else if (GameControl.instance.enemySpellZone[4].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemySpellZone[4];
        else if (GameControl.instance.enemySpellZone[0].transform.childCount == 0)
            selectedSlot = GameControl.instance.enemySpellZone[0];
    }

    async Task OnSummon(int i)
    {
        cardInHand[i].transform.SetParent(selectedSlot.transform);
        selectedSlot.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(37, -36, 0);
        selectedSlot.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .35f);
        selectedSlot.transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
        selectedSlot.transform.GetChild(0).GetComponent<ScriptableCard>().healthSprite.gameObject.SetActive(true);
        selectedSlot.transform.GetChild(0).GetComponent<ScriptableCard>().attackSprite.gameObject.SetActive(true);
        selectedSlot.transform.GetChild(0).eulerAngles = new Vector3(0, 0, 180);
        SummonEffect(selectedSlot.transform.GetChild(0).gameObject);
        CheckIDCard(cardInHand[i]);
        await Task.Delay(500);
    }

    async Task UseSpell(int i)
    {
        cardInHand[i].transform.SetParent(selectedSlot.transform);
        selectedSlot.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector3(37, -36, 0);
        selectedSlot.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .35f);
        if (!isSet)
            selectedSlot.transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
        selectedSlot.transform.GetChild(0).eulerAngles = new Vector3(0, 0, 180);
        SummonEffect(selectedSlot.transform.GetChild(0).gameObject);
        if (!isSet) CheckIDCard(cardInHand[i]);
        await Task.Delay(500);
    }

    async Task DragonUseSetSpell()
    {
        if (GameControl.instance.enemySpellOnField == 0 || GameControl.instance.enemyMonsterOnField == 0) return;
        for (int i = 0; i < 5; i++)
        {
            if (GameControl.instance.enemySpellZone[i].transform.childCount == 1)
            {
                if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.activeSelf)
                {
                    if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id == 2 && GameControl.instance.enemyMonsterOnField > 0)
                        GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    else if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id == 12)
                        GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    else if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id == 14 && GameControl.instance.enemyMonsterOnField > 0 && GameControl.instance.inEnemyGraveYard > 0)
                        GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    else if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id == 19 && GameControl.instance.enemyMonsterOnField > 1)
                    {
                        bool isFound = false;
                        for (int x = 0; x < cardInHand.Count; x++)
                        {
                            if (cardInHand[x].GetComponent<ScriptableCard>().id == 18)
                            {
                                isFound = true;
                                break;
                            }
                        }
                        if (isFound)
                        {
                            GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.SetActive(false);
                            for (int x = 0; x < cardInHand.Count; x++)
                            {
                                if (cardInHand[x].GetComponent<ScriptableCard>().id == 18)
                                {
                                    SelectingSlot();
                                    await OnSummon(x);
                                    GameControl.instance.enemyMonsterOnField++;
                                    GameControl.instance.cardInEnemyHand--;
                                    cardInHand.RemoveAt(x);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                    await Task.Delay(500);
                    if (GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().id != 19)
                        CheckIDCard(GameControl.instance.enemySpellZone[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }

    void SelectingForTribute(int amount)
    {
        if (amount > 1)
        {
            for (int run = amount; run > 0; run--)
            {
                GameObject minHealth = null;
                for (int i = 0; i < 5; i++)
                {
                    if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1 &&
                        GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cost <= 6)
                    {
                        if (minHealth == null)
                        {
                            minHealth = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).gameObject;
                        }
                        else
                        {
                            if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health < minHealth.GetComponent<ScriptableCard>().health &&
                                GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cost <= 6)
                                minHealth = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).gameObject;
                        }
                    }
                }
                GameControl.instance.SendToGraveYard(minHealth);
            }
        }
        else
        {
            GameObject minHealth = null;
            for (int i = 0; i < 5; i++)
            {
                if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                {
                    if (minHealth == null)
                    {
                        minHealth = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).gameObject;
                    }
                    else
                    {
                        if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health < minHealth.GetComponent<ScriptableCard>().health)
                            minHealth = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).gameObject;
                    }
                }
            }
            GameControl.instance.SendToGraveYard(minHealth);
        }
    }

    async Task WinAttack()
    {
        CountCard();
        TurnSystem.instance.turnText.text = "Battle";
        for (int i = 0; i < 5; i++)
        {
            if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
            {
                GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = false;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (GameControl.instance.canBeDirectAttack)
            {
                if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                {
                    if (!GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                    {
                        GameControl.instance.playerHealth -= GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                        StartCoroutine(GameControl.instance.CameraShake(.5f, .1f));
                        GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = true;
                        await Task.Delay(1500);
                    }
                }
            }
            else
            {
                GameControl.instance.Attack = true;
                enemyAttack = GameControl.instance.enemyMonsterOnField;
                if (enemyAttack != 0)
                {
                    if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                    {
                        SelectingTarget();
                        if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack >= target.GetComponent<ScriptableCard>().health &&
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health > target.GetComponent<ScriptableCard>().attack &&
                            !GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                        {
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().target = target;
                            target.GetComponent<ScriptableCard>().target = GameControl.instance.enemyMonsterZone[i].gameObject;
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().EnemyHealthUpdate();
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = true;
                            enemyAttack--;
                            await Task.Delay(1500);
                        }
                    }
                }
            }
        }
        if (enemyAttack > 0) await EqualAttack();
    }

    async Task EqualAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            if (GameControl.instance.canBeDirectAttack)
            {
                if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                {
                    if (!GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                    {
                        GameControl.instance.playerHealth -= GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                        GameControl.instance.CameraShake(.5f, 7);
                        GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = true;
                        await Task.Delay(1500);
                    }
                }
            }
            else
            {
                if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                {
                    if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                        continue;
                    else
                    {
                        SelectingTarget();
                        if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack >= target.GetComponent<ScriptableCard>().health &&
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health <= target.GetComponent<ScriptableCard>().attack &&
                            !GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                        {
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().target = target;
                            target.GetComponent<ScriptableCard>().target = GameControl.instance.enemyMonsterZone[i].gameObject;
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().EnemyHealthUpdate();
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = true;
                            enemyAttack--;
                            await Task.Delay(1500);
                        }
                        else if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack < target.GetComponent<ScriptableCard>().health &&
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health > target.GetComponent<ScriptableCard>().attack &&
                            !GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack)
                        {
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().target = target;
                            target.GetComponent<ScriptableCard>().target = GameControl.instance.enemyMonsterZone[i].gameObject;
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().EnemyHealthUpdate();
                            GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = true;
                            enemyAttack--;
                            await Task.Delay(1500);
                        }
                    }
                }
            }
        }
    }
    void SelectingTarget()
    {
        float minHealth = 100;
        for (int i = 0; i < 5; i++)
        {
            if (GameControl.instance.monsterZone[i].transform.childCount == 1)
            {
                if (GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health < minHealth)
                {
                    target = GameControl.instance.monsterZone[i].transform.GetChild(0).gameObject;
                    minHealth = GameControl.instance.monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health;
                }
            }
        }
    }

    void EndYourTurn()
    {
        while (cardInHand.Count > 6)
        {
            for (int i = 0; i < cardInHand.Count; i++)
            {
                if (countAbove > countBelow4 && countAbove > countBelow6 && countAbove > countSpell && cardInHand[i].GetComponent<ScriptableCard>().cost > 6)
                {
                    cardInHand[i].GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    cardInHand[i].transform.eulerAngles = new Vector3(0, 0, 180);
                    GameControl.instance.SendToGraveYard(cardInHand[i]);
                    if (cardInHand[i].GetComponent<ScriptableCard>().type != Type.spell)
                        GameControl.instance.enemyMonsterOnField++;
                    else GameControl.instance.enemySpellOnField++;
                    cardInHand.RemoveAt(i);
                    break;
                }
                else if (countBelow6 > countBelow4 && countBelow6 > countAbove && countBelow6 > countSpell && cardInHand[i].GetComponent<ScriptableCard>().cost > 4 && cardInHand[i].GetComponent<ScriptableCard>().cost <= 6)
                {
                    cardInHand[i].GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    cardInHand[i].transform.eulerAngles = new Vector3(0, 0, 180);
                    GameControl.instance.SendToGraveYard(cardInHand[i]);
                    if (cardInHand[i].GetComponent<ScriptableCard>().type != Type.spell)
                        GameControl.instance.enemyMonsterOnField++;
                    else GameControl.instance.enemySpellOnField++;
                    cardInHand.RemoveAt(i);
                    break;
                }
                else if (countSpell > countAbove && countSpell > countBelow6 && countSpell > countBelow4 && cardInHand[i].GetComponent<ScriptableCard>().type == Type.spell)
                {
                    cardInHand[i].GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    cardInHand[i].transform.eulerAngles = new Vector3(0, 0, 180);
                    GameControl.instance.SendToGraveYard(cardInHand[i]);
                    if (cardInHand[i].GetComponent<ScriptableCard>().type != Type.spell)
                        GameControl.instance.enemyMonsterOnField++;
                    else GameControl.instance.enemySpellOnField++;
                    cardInHand.RemoveAt(i);
                    break;
                }
                else if (cardInHand[i].GetComponent<ScriptableCard>().cost <= 4)
                {
                    cardInHand[i].GetComponent<ScriptableCard>().cardBack.SetActive(false);
                    cardInHand[i].transform.eulerAngles = new Vector3(0, 0, 180);
                    GameControl.instance.SendToGraveYard(cardInHand[i]);
                    if (cardInHand[i].GetComponent<ScriptableCard>().type != Type.spell)
                        GameControl.instance.enemyMonsterOnField++;
                    else GameControl.instance.enemySpellOnField++;
                    cardInHand.RemoveAt(i);
                    break;
                }
            }
        }
        TurnSystem.instance.EndYourOpponentTurn();
    }

    void CheckIDCard(GameObject card)
    {
        switch(card.GetComponent<ScriptableCard>().id)
        {
            case 1:
                {
                    GameObject effectTarget;
                    int maxAttack = 0;
                    int save = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                        {
                            if (maxAttack == 0)
                            {
                                maxAttack = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                            else if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack > maxAttack)
                            {
                                maxAttack = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                        }
                    }
                    effectTarget = GameControl.instance.enemyMonsterZone[save].transform.GetChild(0).gameObject;
                    effectTarget.GetComponent<ScriptableCard>().health += 1;
                    effectTarget.GetComponent<ScriptableCard>().attack += 1;
                    effectTarget.GetComponent<ScriptableCard>().healthText.text = effectTarget.GetComponent<ScriptableCard>().health.ToString();
                    effectTarget.GetComponent<ScriptableCard>().attackText.text = effectTarget.GetComponent<ScriptableCard>().attack.ToString();
                    BuffEffect(effectTarget);
                    break;
                }
            case 2:
                {
                    GameObject effectTarget;
                    int maxAttack = 0;
                    int save = -1;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                        {
                            if (maxAttack == 0)
                            {
                                maxAttack = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                            else if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack > maxAttack)
                            {
                                maxAttack = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                        }
                    }
                    if (save != -1)
                    {
                        effectTarget = GameControl.instance.enemyMonsterZone[save].transform.GetChild(0).gameObject;
                        effectTarget.GetComponent<ScriptableCard>().attack += 2;
                        effectTarget.GetComponent<ScriptableCard>().health += 3;
                        effectTarget.GetComponent<ScriptableCard>().attackText.text = effectTarget.GetComponent<ScriptableCard>().attack.ToString();
                        effectTarget.GetComponent<ScriptableCard>().healthText.text = effectTarget.GetComponent<ScriptableCard>().health.ToString();
                        BuffEffect(effectTarget);
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
            case 10:
                {
                    if (card.transform.parent.name == "EnemyGraveyard")
                    {
                        EnemyDeck.instance.DrawSpecific(Type.dragon);
                    }
                    break;
                }
            case 12:
                {
                    if (card.transform.parent.name != "EnemyGraveyard")
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                            {
                                if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type == Type.dragon)
                                {
                                    GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack += 2;
                                    GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attackText.text = GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().attack.ToString();
                                    BuffEffect(GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).gameObject);
                                }
                            }
                        }
                        GameControl.instance.SendToGraveYard(card);
                    }
                    break;
                }
            case 13:
                {
                    int boostHP = 0;
                    int boostATK = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GameControl.instance.enemyMonsterZone[i].transform.childCount == 1)
                        {
                            if (GameControl.instance.enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().type == Type.dragon)
                            {
                                boostHP++;
                                boostATK++;
                            }
                        }
                    }
                    card.GetComponent<ScriptableCard>().attack += boostATK;
                    card.GetComponent<ScriptableCard>().health += boostHP;
                    card.GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().attack.ToString();
                    card.GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().health.ToString();
                    break;
                }
            case 14:
                {
                    SelectingForTribute(1);
                    int maxAttack = 0;
                    int save = -1;
                    for (int i = 0; i < GameControl.instance.inEnemyGraveYard; i++)
                    {
                        if (GameControl.instance.enemyGraveYardList[i].GetComponent<ScriptableCard>().type == Type.dragon)
                        {
                            if (maxAttack == 0)
                            {
                                maxAttack = GameControl.instance.enemyGraveYardList[i].GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                            else if (GameControl.instance.enemyGraveYardList[i].GetComponent<ScriptableCard>().attack > maxAttack)
                            {
                                maxAttack = GameControl.instance.enemyGraveYardList[i].GetComponent<ScriptableCard>().attack;
                                save = i;
                            }
                        }
                    }
                    if (save != -1)
                    {
                        GameObject temp = GameControl.instance.enemyGraveYardList[save].gameObject;
                        temp.transform.SetParent(GameObject.Find("EnemyHand").transform);
                        temp.transform.localScale = Vector3.one;
                        temp.transform.eulerAngles = new Vector3(25, 0);
                        temp.transform.position = new Vector3(transform.position.x, transform.position.y);
                        temp.GetComponent<ScriptableCard>().cardBack.SetActive(true);
                        temp.GetComponent<ScriptableCard>().healthSprite.gameObject.SetActive(false);
                        temp.GetComponent<ScriptableCard>().attackSprite.gameObject.SetActive(false);
                    }
                    GameControl.instance.SendToGraveYard(card);
                    break;
                }
            case 16:
                {
                    int boostAtk = 1;
                    int boostHP = 1;
                    for (int i = 0; i < GameControl.instance.inEnemyGraveYard; i++)
                    {
                        if (GameControl.instance.enemyGraveYardList[i].GetComponent<ScriptableCard>().type == Type.dragon)
                        {
                            boostAtk++;
                            boostHP++;
                        }
                    }
                    card.GetComponent<ScriptableCard>().attack += boostAtk;
                    card.GetComponent<ScriptableCard>().health += boostHP;
                    card.GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().attack.ToString();
                    card.GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().health.ToString();
                    break;
                }
            case 19:
                {
                    SelectingForTribute(2);
                    GameControl.instance.SendToGraveYard(card);
                    break;
                }
        }
    }

    async void SummonEffect(GameObject target)
    {
        GameObject temp = Instantiate(summon);
        temp.transform.position = target.transform.position;
        temp.transform.localScale = new Vector3(-3.5f, -3.5f);
        await Task.Delay(1000);
        Destroy(temp);
    }

    async void BuffEffect(GameObject target)
    {
        GameObject temp = Instantiate(buff);
        temp.transform.position = target.transform.position;
        temp.transform.localScale = new Vector3(-3.5f, -3.5f);
        await Task.Delay(1000);
        Destroy(temp);
    }

    async void DebuffEffect(GameObject target)
    {
        GameObject temp = Instantiate(debuff);
        temp.transform.position = target.transform.position;
        temp.transform.localScale = new Vector3(-3.5f, -3.5f);
        await Task.Delay(1000);
        Destroy(temp);
    }
}