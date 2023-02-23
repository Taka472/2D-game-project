using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorControl : MonoBehaviour
{
    public static EditorControl instance;

    public List<int> inventoryCardID = new();
    public List<Card> inventory = new();
    public GameObject inventoryPanel;
    public GameObject deckPanel;
    public GameObject monsterCard;
    public GameObject spellCard;

    public Text CardName;
    public Text TypeText;
    public Text Cost;
    public Text ATK;
    public Text HP;
    public Text Description;

    public Button addButton;
    public Button removeButton;

    public bool isSelectedForEdit;

    public List<Card> playerDeck;
    public GameObject selectedForEdit;

    public Slider musicSlider;
    public Slider ffxSlider;
    public AudioSource bgm;
    public AudioSource[] ffx;

    public Image pauseMenu;
    public bool pauseIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        int pos = 0;
        inventoryCardID = DataManager.instance.inventoryModel.inventory.gameData.InventoryCardID;
        for (int i = 0; i < inventoryCardID.Count; i++)
        {
            for (int x = 0; x < CardDatabase.Data.Count; x++)
            {
                if (inventoryCardID[i] == CardDatabase.Data[x].id)
                {
                    inventory.Add(CardDatabase.Data[x]);
                    break;
                }
            }
        }
        while (pos < inventory.Count)
        {
            if (pos == 0 || inventory[pos].cost <= inventory[pos - 1].cost)
            {
                pos++;
            }
            else
            {
                Card temp = inventory[pos];
                inventory[pos] = inventory[pos - 1];
                inventory[pos - 1] = temp;
                pos--;
            }
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            GameObject temp = null;
            if (inventory[i].type != Type.spell)
                temp = Instantiate(monsterCard, inventoryPanel.transform);
            else temp = Instantiate(spellCard, inventoryPanel.transform);
            temp.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f);
            temp.GetComponent<ScriptableCard>().glowEffect.gameObject.SetActive(false);
            ScriptableCard.instance.cards = inventory[i];
        }
        GnomeSortObject(inventoryPanel);
        playerDeck = DataManager.instance.playerDeck;
        pos = 0;
        while (pos < playerDeck.Count)
        {
            if (pos == 0 || playerDeck[pos].cost <= playerDeck[pos - 1].cost)
            {
                pos++;
            }
            else
            {
                Card temp = playerDeck[pos];
                playerDeck[pos] = playerDeck[pos - 1];
                playerDeck[pos - 1] = temp;
                pos--;
            }
        }
        for (int i = 0; i < playerDeck.Count; i++)
        {
            GameObject temp = null;
            if (playerDeck[i].type != Type.spell)
                temp = Instantiate(monsterCard, deckPanel.transform);
            else temp = Instantiate(spellCard, deckPanel.transform);
            temp.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f);
            temp.GetComponent<ScriptableCard>().glowEffect.gameObject.SetActive(false);
            ScriptableCard.instance.cards = playerDeck[i];
        }
        bgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        ffxSlider.value = PlayerPrefs.GetFloat("FfxVolume");
    }

    void Update()
    {
        if (isSelectedForEdit && selectedForEdit.transform.parent.parent.name == "InventoryScroll" && deckPanel.transform.childCount < 30)
        {
            addButton.gameObject.SetActive(true);
            removeButton.gameObject.SetActive(false);
            addButton.transform.position = selectedForEdit.transform.position + new Vector3(0, 1, 0);
        }
        else if (isSelectedForEdit && selectedForEdit.transform.parent.parent.name == "DeckScroll")
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(true);
            removeButton.transform.position = selectedForEdit.transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            addButton.gameObject.SetActive(false);
            removeButton.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseIsOn)
            {
                pauseMenu.gameObject.SetActive(true);
                pauseIsOn = true;
            }
            else
            {
                pauseMenu.gameObject.SetActive(false);
                pauseIsOn = false;
            }
        }
        bgm.volume = PlayerPrefs.GetFloat("MusicVolume");
        for (int i = 0; i < ffx.Length; i++)
        {
            ffx[i].volume = PlayerPrefs.GetFloat("FfxVolume");
        }
    }

    public void Add()
    {
        for (int i = 0; i < CardDatabase.Data.Count; i++)
        {
            if (CardDatabase.Data[i].id == selectedForEdit.GetComponent<ScriptableCard>().id)
            {
                playerDeck.Add(CardDatabase.Data[i]);
                inventory.Remove(CardDatabase.Data[i]);
                inventoryCardID.Remove(CardDatabase.Data[i].id);
                break;
            }    
        }
        GnomeSort(playerDeck);
        GnomeSort(inventory);
        selectedForEdit.transform.SetParent(deckPanel.transform);
        GnomeSortObject(deckPanel);
        ffx[0].PlayOneShot(ffx[0].clip);
    }

    public void Remove()
    {
        for (int i = 0; i < CardDatabase.Data.Count; i++)
        {
            if (CardDatabase.Data[i].id == selectedForEdit.GetComponent<ScriptableCard>().id)
            {
                playerDeck.Remove(CardDatabase.Data[i]);
                inventory.Add(CardDatabase.Data[i]);
                inventoryCardID.Add(CardDatabase.Data[i].id);
                break;
            }
        }
        selectedForEdit.transform.SetParent(inventoryPanel.transform);
        GnomeSortObject(inventoryPanel);
        GnomeSort(inventory);
        ffx[0].PlayOneShot(ffx[0].clip);
    }

    void GnomeSort(List<Card> cardID)
    {
        int pos = 0;
        while (pos < cardID.Count)
        {
            if (pos == 0 || cardID[pos].id <= cardID[pos - 1].id)
            {
                pos++;
            }
            else
            {
                Card temp = cardID[pos];
                cardID[pos] = cardID[pos - 1];
                cardID[pos - 1] = temp;
                pos--;
            }
        }
    }

    void GnomeSortObject(GameObject deckPanel)
    {
        int pos = 0;
        while (pos < deckPanel.transform.childCount)
        {
            if (pos == 0 || deckPanel.transform.GetChild(pos).GetComponent<ScriptableCard>().cost < deckPanel.transform.GetChild(pos - 1).GetComponent<ScriptableCard>().cost)
            {
                pos++;
            }
            else if (deckPanel.transform.GetChild(pos).GetComponent<ScriptableCard>().cost == deckPanel.transform.GetChild(pos - 1).GetComponent<ScriptableCard>().cost)
            {
                if (deckPanel.transform.GetChild(pos).GetComponent<ScriptableCard>().id <= deckPanel.transform.GetChild(pos - 1).GetComponent<ScriptableCard>().id)
                {
                    pos++;
                }
                else
                {
                    deckPanel.transform.GetChild(pos).SetSiblingIndex(pos - 1);
                    pos--;
                }
            }
            else
            {
                deckPanel.transform.GetChild(pos).SetSiblingIndex(pos - 1);
                pos--;
            }
        }
    }

    public void ToMainMenu()
    {
        ffx[0].PlayOneShot(ffx[0].clip);
        SceneManager.LoadScene(0);
    }

    public void AudioChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void FFXChange()
    {
        PlayerPrefs.SetFloat("FfxVolume", ffxSlider.value);
    }

    public void Save()
    {
        GnomeSort(playerDeck);
        DataManager.instance.deckModel.deck[DataManager.instance.isActive].gameDatas.deckOfCardID.Clear();
        for (int i = 0; i < playerDeck.Count; i++)
        {
            DataManager.instance.deckModel.deck[DataManager.instance.isActive].gameDatas.deckOfCardID.Add(playerDeck[i].id);
        }
        int pos = 0;
        while (pos < inventoryCardID.Count)
        {
            if (pos == 0 || inventoryCardID[pos] <= inventoryCardID[pos - 1])
            {
                pos++;
            }
            else
            {
                int temp = inventoryCardID[pos];
                inventoryCardID[pos] = inventoryCardID[pos - 1];
                inventoryCardID[pos - 1] = temp;
                pos--;
            }
        }
        DataManager.instance.inventoryModel.inventory.gameData.InventoryCardID = inventoryCardID;
        DataManager.instance.SaveData("Data.txt", "InventoryData.txt");
    }

    public void SaveAndActive()
    {
        DataManager.instance.playerDeck = playerDeck;
        Save();
        SceneManager.LoadScene(0);
    }
}
