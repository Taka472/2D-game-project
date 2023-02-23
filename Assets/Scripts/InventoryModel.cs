using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    public class InventoryModel
    {
        public Inventory inventory = new();

        [Serializable]
        public class Inventory
        {
            public string name;
            public GameData gameData = new();
        }

        [Serializable]
        public class GameData
        {
            public List<int> InventoryCardID = new();
        }

        public static Inventory GetModelFromJSon(string response)
        {
            InventoryModel model = JsonUtility.FromJson<InventoryModel>(response);
            return model.inventory;
        }

        public static string GetJSonFromModel(InventoryModel model, bool pretty)
        {
            return JsonUtility.ToJson(model, pretty);
        }
    }
}