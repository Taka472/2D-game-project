using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameControl : MonoBehaviour
{
    public int cardInDeck;
    public int cardInEnemyDeck;
    public int monsterOnField;
    public int spellOnField;
    public GameObject[] monsterZone;
    public GameObject[] spellZone;
    public GameObject GraveYard;
    public List<GameObject> graveYardList;
    public int inGraveYard;
    public int cardInHand;
    public int playerHealth = 30;
    public int enemyMonsterOnField;
    public int enemySpellOnField;
    public GameObject[] enemyMonsterZone;
    public GameObject[] enemySpellZone;
    public GameObject EnemyGraveYard;
    public List<GameObject> enemyGraveYardList;
    public int inEnemyGraveYard;
    public int monsterInEnemyGraveYard;
    public int cardInEnemyHand;
    public int enemyHealth = 30;

    public static GameControl instance;

    public Text healthText;
    public Text enemyHealthText;

    public bool Attack;
    public bool canDirectAttack;
    public bool canBeDirectAttack;

    public Image background;

    public Image tributeSummonNotice;
    public Text tributeSummonText;

    public bool DrawPhase;
    public bool MainPhase;
    public bool Summon;
    public bool TributeSummon;
    public bool ActivateSpell;
    public bool SetSpell;
    public bool isFirst;
    public bool OutOfNumber;

    public Button tributingConfirm;
    public int amountForTribute;

    public Vector2 GraveYardPosition;
    public Vector3 EnemyGraveYardPosition;

    public Button discardToEndTurn;
    public Button attackButton;
    public Button endTurnButton;

    public bool selectForEffect;
    public bool spellChainSelect;
    public bool confirmChainSelect;

    public bool isOn;

    public Image pauseMenu;
    public bool PauseMenuOn;

    public AudioSource bgm;
    public AudioSource[] ffx;
    public AudioSource directHit;

    public Image WinLose;
    public Sprite LoseText;
    public Sprite WinText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerHealth = 30;
        enemyHealth = 30;
        TributeSummon = false;
        isFirst = true;
        confirmChainSelect = false;
        GraveYardPosition = GraveYard.GetComponent<RectTransform>().anchoredPosition + new Vector2(867, 62);
        EnemyGraveYardPosition = new Vector3(100, -70, 0);
        isOn = false;
        PauseMenuOn = false;
        bgm.volume = PlayerPrefs.GetFloat("MusicVolume");
        for (int i = 0; i < ffx.Length; i++)
        {
            ffx[i].volume = PlayerPrefs.GetFloat("FfxVolume");
        }
        GameObject temp = new();
        if (GameObject.FindGameObjectWithTag("BGM") != null)
            GameObject.FindGameObjectWithTag("BGM").transform.SetParent(temp.transform);
        Destroy(temp);
    }

    // Update is called once per frame
    void Update()
    {
        cardInDeck = PlayerDeck.instance.numsOfCard;
        cardInEnemyDeck = EnemyDeck.instance.numsOfCard;
        healthText.text = "" + playerHealth;
        enemyHealthText.text = "" + enemyHealth;
        if (enemyMonsterOnField == 0) canDirectAttack = true;
        else canDirectAttack = false;
        if (monsterOnField == 0) canBeDirectAttack = true;
        else canBeDirectAttack = false;
        Effect();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseMenuOn)
            {
                PauseMenu();
                PauseMenuOn = true;
            }
            else
            {
                PauseMenu();
                PauseMenuOn = false;
            }
        }
        LoadMusicData();
        WinLoseControl();
    }

    public void AttackPhase()
    {
        if (Attack) return;
        MainPhase = false;
        Summon = false;
        ActivateSpell = false;
        SetSpell = false;
        TurnSystem.instance.turnText.text = "Battle";
        for (int i = 0; i < 5; i++)
        {
            if (monsterZone[i].transform.childCount == 1)
                monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = false;
            if (enemyMonsterZone[i].transform.childCount == 1)
                enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().haveAttack = false;
        }
        for (int i = 0; i < 5; i++)
        {
            if (monsterZone[i].transform.childCount == 1)
                CardEffectControl.instance.CardIdCheck(monsterZone[i].transform.GetChild(0).gameObject, true);
            if (spellZone[i].transform.childCount == 1)
                CardEffectControl.instance.CardIdCheck(spellZone[i].transform.GetChild(0).gameObject, true);
        }
        Attack = true;
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        directHit.PlayOneShot(directHit.clip);
        Vector3 startPosition = background.transform.position;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            magnitude -= Time.deltaTime * magnitude;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            background.transform.position = startPosition + new Vector3(x, y, 0);
            yield return null;
        }
        background.transform.position = startPosition;
    }

    public void RequestTribute(int amount)
    {
        Image temp = Instantiate(tributeSummonNotice);
        temp.transform.SetParent(GameObject.Find("Canvas").transform);
        temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        temp.GetComponent<RectTransform>().localScale = Vector3.one;
        string haveS = "";
        if (amount > 1) haveS = "s";
        tributeSummonText = temp.transform.GetChild(0).GetComponent<Text>();
        tributeSummonText.text = "This monster required tributing " + amount + " monster" + haveS + " to summon";
        TributeSummon = true;
        amountForTribute = amount;
        tributingConfirm.gameObject.SetActive(true);
        temp.transform.GetChild(3).gameObject.SetActive(false);
    }

    public void ConfirmTribute()
    {
        ffx[15].PlayOneShot(ffx[15].clip);
        int temp = amountForTribute;
        for (int i = 0; i < monsterZone.Length; i++)
        {
            if (monsterZone[i].transform.childCount == 1)
            {
                if (monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForTribute)
                {
                    temp--;
                }
            }
        }
        if (temp == 0)
        {
            for (int i = 0; i < monsterZone.Length; i++)
            {
                if (monsterZone[i].transform.childCount == 1)
                {
                    if (monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForTribute)
                    {
                        SendToGraveYard(monsterZone[i].transform.GetChild(0).gameObject);
                    }
                }
            }
            for (int x = 0; x < GraveYard.transform.childCount; x++)
            {
                GraveYard.transform.GetChild(x).GetComponent<ScriptableCard>().glowEffect.gameObject.SetActive(false);
                GraveYard.transform.GetChild(x).GetComponent<RectTransform>().anchoredPosition = GraveYardPosition;
                GraveYard.transform.GetChild(x).GetComponent<RectTransform>().localScale = Vector3.one;
            }
            tributingConfirm.gameObject.SetActive(false);
            Summon = true;
        }
        else
        {
            tributingConfirm.gameObject.SetActive(false);
            TributeSummon = false;
            for (int i = 0; i < 5; i++)
            {
                if (monsterZone[i].transform.childCount == 1)
                {
                    if (monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForTribute)
                    {
                        monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForTribute = false;
                    }
                }
            }
        }
    }

    public void Discard()
    {
        cardInHand--;
        SendToGraveYard(InfoPanelControl.instance.isSelected);
        if (InfoPanelControl.instance.isSelected.GetComponent<ScriptableCard>().type != Type.spell)
            monsterOnField++;
        else spellOnField++;
    }

    public async void SendToGraveYard(GameObject card)
    {
        if (card.GetComponent<ScriptableCard>().type == Type.spell && card.transform.parent.name != "EnemyHand")
            await Task.Delay(250);
        card.GetComponent<ScriptableCard>().isSelectedForEffect = false;
        Vector3 originalPosition = card.transform.position;
        GameObject temp = Instantiate(ScriptableCard.instance.explodeEffect);
        temp.transform.position = originalPosition;
        temp.transform.localScale = new Vector3(3f, 3f);
        card.GetComponent<ScriptableCard>().health = card.GetComponent<ScriptableCard>().originalHealth;
        if (card.GetComponent<ScriptableCard>().healthText != null)
            card.GetComponent<ScriptableCard>().healthText.text = card.GetComponent<ScriptableCard>().originalHealth.ToString();
        card.GetComponent<ScriptableCard>().attack = card.GetComponent<ScriptableCard>().originalAttack;
        if (card.GetComponent<ScriptableCard>().attackText != null)
            card.GetComponent<ScriptableCard>().attackText.text = card.GetComponent<ScriptableCard>().originalAttack.ToString();
        if (card.name[..4] == "Mons" || card.name[..4] == "Spel")
        {
            card.transform.SetParent(GraveYard.transform);
            card.GetComponent<RectTransform>().anchoredPosition = GraveYardPosition;
            card.GetComponent<RectTransform>().localScale = Vector3.one;
            inGraveYard++;
            if (card.GetComponent<ScriptableCard>().type != Type.spell) monsterOnField--;
            else spellOnField--;
            graveYardList.Add(card);
            CardEffectControl.instance.CardIdCheck(card, true);
        }
        else if (card.name[..4] == "Enem")
        {
            temp.transform.eulerAngles = new Vector3(0, 0, 180);
            card.transform.SetParent(EnemyGraveYard.transform);
            card.GetComponent<RectTransform>().anchoredPosition = EnemyGraveYardPosition;
            card.GetComponent<RectTransform>().localScale = Vector3.one;
            inEnemyGraveYard++;
            if (card.GetComponent<ScriptableCard>().type != Type.spell) { enemyMonsterOnField--; monsterInEnemyGraveYard++; }
            else enemySpellOnField--;
            enemyGraveYardList.Add(card);
        }
        await Task.Delay(1000);
        Destroy(temp);
    }

    public void EffectPhase()
    {
        for (int i = 0; i < 5; i++)
        {
            if (monsterZone[i].transform.childCount == 1)
                monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForEffect = false;
            if (spellZone[i].transform.childCount == 1)
                spellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForEffect = false;
            if (enemyMonsterZone[i].transform.childCount == 1)
                enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForEffect = false;
            if (enemySpellZone[i].transform.childCount == 1)
                enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isSelectedForEffect = false;
        }
        if (selectForEffect)
        {
            endTurnButton.gameObject.SetActive(false);
            attackButton.gameObject.SetActive(false);
        }
        else
        {
            attackButton.gameObject.SetActive(true);
            endTurnButton.gameObject.SetActive(true);
        }
    }

    public void EffectUpdate(List<GameObject> effectChain)
    {
        for (int i = effectChain.Count - 1; i >= 0; i--)
        {
            CardEffectControl.instance.CardIdCheck(effectChain[i], true);
            CardEffectControl.instance.CardTypeCheck(effectChain[i], true);
        }
        effectChain.Clear();
        CardEffectControl.instance.needSelected = 0;
        selectForEffect = false;
        EffectPhase();
    }

    public async Task EndTurnStatusCheck()
    {
        for (int i = 0; i < 5; i++)
        {
            if (monsterZone[i].transform.childCount == 1)
            {
                if (monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBurn > 0)
                {
                    monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health -= 1;
                    monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().healthText.text = monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health.ToString();
                    monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBurn -= 1;
                    await Task.Delay(500);
                }
                else if (monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isInvicible > 0)
                {
                    monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isInvicible -= 1;
                }
            }
            if (enemyMonsterZone[i].transform.childCount == 1)
            {
                if (enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBurn > 0)
                {
                    enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health -= 1;
                    enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().healthText.text = monsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().health.ToString();
                    enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isBurn -= 1;
                    await Task.Delay(500);
                }
                else if (enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isInvicible > 0)
                {
                    enemyMonsterZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().isInvicible -= 1;
                }
            }
        }
    }

    void Effect()
    {
        if (Summon)
        {
            if (!isOn) 
            {
                for (int i = 0; i < 5; i++)
                {
                    monsterZone[i].GetComponent<Image>().color = Color.green;
                }
                isOn = true;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    spellZone[i].GetComponent<Image>().color = Color.white;
                    monsterZone[i].GetComponent<Image>().color = Color.green;
                }
                isOn = true;
            }          
        }
        else if (ActivateSpell || SetSpell)
        {
            if (!isOn)
            {
                for (int i = 0; i < 5; i++)
                {
                    spellZone[i].GetComponent<Image>().color = Color.green;
                }
                isOn = true;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    monsterZone[i].GetComponent<Image>().color = Color.white;
                    spellZone[i].GetComponent<Image>().color = Color.green;
                }
                isOn = true;
            }
        }
        else if (isOn)
        {
            for (int i = 0; i < 5; i++)
            {
                spellZone[i].GetComponent<Image>().color = Color.white;
                monsterZone[i].GetComponent<Image>().color = Color.white;
            }
            isOn = false;
        }
    }

    void PauseMenu()
    {
        if (!PauseMenuOn)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void LoadMusicData()
    {
        bgm.volume = PlayerPrefs.GetFloat("MusicVolume");
        for (int i = 0; i < ffx.Length; i++)
        {
            ffx[i].volume = PlayerPrefs.GetFloat("FfxVolume");
        }
    }

    void WinLoseControl()
    {
        if (playerHealth <= 0)
        {
            WinLose.gameObject.SetActive(true);
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                WinLose.GetComponent<CanvasGroup>().alpha = i;
            }
            WinLose.transform.GetChild(0).GetComponent<Image>().sprite = LoseText;
        }
        else if (enemyHealth <= 0)
        {
            WinLose.gameObject.SetActive(true);
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                WinLose.GetComponent<CanvasGroup>().alpha = i;
            }
            WinLose.transform.GetChild(0).GetComponent<Image>().sprite = WinText;
        }
        WinLose.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
} 