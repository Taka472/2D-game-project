using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance;

    public Text turnText;
    public Button attack;
    // Start is called before the first frame update
    void Start()
    {
        StartRandom();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (ScriptableCard.isYourTurn && !GameControl.instance.selectForEffect && !GameControl.instance.isFirst) 
            attack.gameObject.SetActive(true);
        else attack.gameObject.SetActive(false);
    }

    public async void EndYourTurn()
    {
        if (GameControl.instance.cardInHand <= 6)
        {
            if (!ScriptableCard.isYourTurn) return;
            ScriptableCard.isYourTurn = false;
            GameControl.instance.Attack = false;
            await GameControl.instance.EndTurnStatusCheck();
            GameControl.instance.DrawPhase = true;
            StartCoroutine(EnemyDeck.instance.DrawCard());
            GameControl.instance.isFirst = false;
            GameControl.instance.OutOfNumber = false;
            GameControl.instance.SetSpell = false;
            GameControl.instance.ActivateSpell = false;
            InfoPanelControl.instance.isSelected = null;
            GameControl.instance.EffectPhase();
            GameControl.instance.Summon = false;
            ScriptableCard.summoned = false;
            turnText.text = "Opponent's Turn";
        }
        else
        {
            Image temp = Instantiate(GameControl.instance.tributeSummonNotice);
            temp.transform.SetParent(GameObject.Find("Canvas").transform);
            temp.GetComponent<RectTransform>().localScale = Vector3.one;
            temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            temp.transform.GetChild(2).gameObject.SetActive(false);
            GameControl.instance.tributeSummonText = temp.transform.GetChild(0).GetComponent<Text>();
            GameControl.instance.tributeSummonText.text = "Discard "+ (GameControl.instance.cardInHand - 6) +" to end your turn";
            temp.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public async void EndYourOpponentTurn()
    {
        if (ScriptableCard.isYourTurn) return;
        ScriptableCard.isYourTurn = true;
        ScriptableCard.summoned = false;
        await GameControl.instance.EndTurnStatusCheck();
        GameControl.instance.DrawPhase = true;
        StartCoroutine(PlayerDeck.instance.DrawCard());
        GameControl.instance.isFirst = false;
        GameControl.instance.MainPhase = true;
        GameControl.instance.Attack = false;
        turnText.text = "Your Turn";
    }

    void StartRandom()
    {
        switch(Random.Range(0, 2))
        {
            case 0:
                {
                    ScriptableCard.isYourTurn = true;
                    turnText.text = "Your Turn";
                    break;
                }
            case 1:
                {
                    ScriptableCard.isYourTurn = false;
                    turnText.text = "Opponent's Turn";
                    break;
                }
        }
    }
}
