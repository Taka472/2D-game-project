using System.IO;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public DeckModel deckModel = new();
    public InventoryModel inventoryModel = new();
    public List<Card> playerDeck = new();
    public int isActive = 0;

    private void Awake()
    {
        LoadData();
        LoadInventoryData();
        instance = this;
    }

    void Start()
    {
        //SaveData();
        //CreateDefaultDeck();
        //LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDefaultDeck()
    {
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Data.txt"), DeckModel.GetJSonFromModel(deckModel, true));
    }

    public void LoadData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Data.txt");
        string data = File.ReadAllText(path);
        deckModel.deck = DeckModel.GetModelFromJSon(data);
        for (int i = 0; i < deckModel.deck[isActive].gameDatas.deckOfCardID.Count; i++)
        {
            for (int x = 0; x < CardDatabase.Data.Count; x++)
            {
                if (deckModel.deck[isActive].gameDatas.deckOfCardID[i] == CardDatabase.Data[x].id)
                {
                    playerDeck.Add(CardDatabase.Data[x]);
                    break;
                }
            }
        }
    }

    public void LoadInventoryData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "InventoryData.txt");
        string data = File.ReadAllText(path);
        inventoryModel.inventory = InventoryModel.GetModelFromJSon(data);
    }

    public void SaveData(string playerFileName, string inventoryFileName)
    {
        string playerPath = Path.Combine(Application.streamingAssetsPath, playerFileName);
        File.WriteAllText(playerPath, DeckModel.GetJSonFromModel(deckModel, true));
        string inventoryPath = Path.Combine(Application.streamingAssetsPath, inventoryFileName);
        File.WriteAllText(inventoryPath, InventoryModel.GetJSonFromModel(inventoryModel, true));
    }
}
