using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDeck : MonoBehaviour
{
    public List<Card> deck = new();
    private Card container;

    public Image[] cardInDeck;

    public GameObject Hand;
    public GameObject enemyMonsterCard;
    public GameObject enemySpellCard;

    public static EnemyDeck instance;

    public int numsOfCard;

    public GameObject shuffleEffect;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        for (int i = 0; i < 3; i++)
        {
            deck.Add(CardDatabase.Data[0]);
            deck.Add(CardDatabase.Data[1]);
            deck.Add(CardDatabase.Data[9]);
            deck.Add(CardDatabase.Data[11]);
            deck.Add(CardDatabase.Data[12]);
            deck.Add(CardDatabase.Data[13]);
            deck.Add(CardDatabase.Data[15]);
            deck.Add(CardDatabase.Data[16]);
            deck.Add(CardDatabase.Data[17]);
            deck.Add(CardDatabase.Data[18]);
        }
        numsOfCard = deck.Count;
        Shuffle();
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
        temp.transform.position = new Vector3(-7.5f, 1.4f);
        temp.transform.localScale = new Vector3(2.5f, 2.5f);
        temp.transform.eulerAngles = new Vector3(0, 0, 180);
        yield return new WaitForSeconds(1f);
        Destroy(temp);
    }

    IEnumerator StartGame()
    {
        GameObject card;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (deck[numsOfCard - 1].type != Type.spell)
                card = Instantiate(enemyMonsterCard, transform.position, transform.rotation);
            else card = Instantiate(enemySpellCard, transform.position, transform.rotation);
            GameControl.instance.cardInEnemyHand++;
            AIControl.instance.cardInHand.Add(card);
        }
        yield return new WaitForSeconds(.5f);
        if (!ScriptableCard.isYourTurn) AIControl.instance.PlayYourTurn();
    }

    public IEnumerator DrawCard()
    {
        GameObject card;
        yield return new WaitForSeconds(0.5f);
        if (deck[numsOfCard - 1].type != Type.spell)
            card = Instantiate(enemyMonsterCard, transform.position, transform.rotation);
        else card = Instantiate(enemySpellCard, transform.position, transform.rotation);
        GameControl.instance.cardInEnemyHand++;
        AIControl.instance.cardInHand.Add(card);
        yield return new WaitForSeconds(.5f);
        if (!ScriptableCard.isYourTurn)
            AIControl.instance.PlayYourTurn();
        GameControl.instance.DrawPhase = false;
    }

    public IEnumerator DrawSpecific(Type type)
    {
        GameObject card;
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].type == type)
            {
                card = Instantiate(enemyMonsterCard, transform.position, transform.rotation);
                GameControl.instance.cardInEnemyHand++;
                AIControl.instance.cardInHand.Add(card);
                yield return new WaitForSeconds(.5f);
            }
        }
    }

    public IEnumerator DrawAmount(int amount)
    {
        GameObject card;
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            if (deck[numsOfCard - 1].type != Type.spell)
                card = Instantiate(enemyMonsterCard, transform.position, transform.rotation);
            else card = Instantiate(enemySpellCard, transform.position, transform.rotation);
            GameControl.instance.cardInEnemyHand++;
            AIControl.instance.cardInHand.Add(card);
        }
    }
}
