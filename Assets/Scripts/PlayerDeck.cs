using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new();
    private Card container;

    public Image[] cardInDeck;

    public GameObject Hand;
    public GameObject monsterCard;
    public GameObject spellCard;

    public static PlayerDeck instance;

    public int numsOfCard;

    public GameObject shuffleEffect;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        DataManager.instance.LoadData();
        deck = DataManager.instance.playerDeck;
        numsOfCard = deck.Count;
        Shuffle();
        if (numsOfCard > 30)
        {
            for (int i = 30; i > numsOfCard + 2; i--)
            {
                if (i % 2 == 0)
                    cardInDeck[i / 2].gameObject.SetActive(false);
            }
        }
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        DeckDisplay();
    }

    public void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            container = deck[i];
            int RandomIndex = Random.Range(1, deck.Count);
            deck[i] = deck[RandomIndex];
            deck[RandomIndex] = container;
        }
        StartCoroutine(DeckShuffle());
    }

    void DeckDisplay()
    {
        if (numsOfCard == 30) return;
        if (numsOfCard % 2 == 0) cardInDeck[numsOfCard / 2].gameObject.SetActive(false);
    }

    IEnumerator DeckShuffle()
    {
        GameObject temp = Instantiate(shuffleEffect);
        temp.transform.position = new Vector3(7.7f, -1.6f);
        temp.transform.localScale = new Vector3(2.5f, 2.5f);
        yield return new WaitForSeconds(1f);
        Destroy(temp);
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (deck[numsOfCard - 1].type != Type.spell) 
                Instantiate(monsterCard, transform.position, transform.rotation);
            else Instantiate(spellCard, transform.position, transform.rotation);
            GameControl.instance.cardInHand++;
        }
        if (ScriptableCard.isYourTurn) GameControl.instance.MainPhase = true;
        ScriptableCard.summoned = false;
    }

    public IEnumerator DrawCard()
    {
        yield return new WaitForSeconds(0.5f);
        if (deck[numsOfCard - 1].type != Type.spell)
            Instantiate(monsterCard, transform.position, transform.rotation);
        else Instantiate(spellCard, transform.position, transform.rotation);
        if (ScriptableCard.isYourTurn)
            GameControl.instance.cardInHand++;
        GameControl.instance.DrawPhase = false;
    }

    public IEnumerator DrawAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (deck[numsOfCard - 1].type != Type.spell)
                Instantiate(monsterCard, transform.position, transform.rotation);
            else Instantiate(spellCard, transform.position, transform.rotation);
            GameControl.instance.cardInHand++;
        }
    }
}
