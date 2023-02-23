using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace AssemblyCSharp
{
    public class DeckModel
    {
        public List<Deck> deck = new();

        public DeckModel()
        {
            deck.Add(new Deck());
        }

        [Serializable]
        public class Deck
        {
            public string name;
            public GameData gameDatas = new();
        }

        [Serializable]
        public class GameData
        {
            public List<int> deckOfCardID;
        }

        public static List<Deck> GetModelFromJSon(string response)
        {
            DeckModel model = JsonUtility.FromJson<DeckModel>(response);
            return model.deck;
        }

        public static string GetJSonFromModel(DeckModel model, bool pretty)
        {
            return JsonUtility.ToJson(model, pretty);
        }
    }
}
