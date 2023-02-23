using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> Data = new();
    void Awake()
    {
        Data.Add(new Card(1, "Dark Dragon", "+1/+1 to a single unit", Type.dragon, 5, 4, 5, Resources.Load<Sprite>("4")));
        Data.Add(new Card(2, "Dragon's Sage", "+1/+1 to one unit, if it's a dragon, +2/+3", Type.spell, 0, 0, 0, Resources.Load<Sprite>("2")));
        Data.Add(new Card(3, "Demon", "-1/0 to an opponent's unit", Type.demon, 2, 1, 4, Resources.Load<Sprite>("10")));
        Data.Add(new Card(4, "Soul Exchange", "Send one ally to the graveyard to send one unit to the graveyard", Type.spell, 0, 0, 0, Resources.Load<Sprite>("7")));
        Data.Add(new Card(5, "Golem", "Invincible this turn", Type.golem, 7, 7, 8, Resources.Load<Sprite>("51")));
        Data.Add(new Card(6, "Heaven angel", "If this card is summoned, draw 1", Type.angel, 2, 1, 2, Resources.Load<Sprite>("8")));
        Data.Add(new Card(7, "Witch", "If this card is sent to the graveyard, draw 1", Type.witch, 3, 2, 2, Resources.Load<Sprite>("50")));
        Data.Add(new Card(8, "A gift from above", "Shuffle the deck and draw 1", Type.spell, 0, 0, 0, Resources.Load<Sprite>("1")));
        Data.Add(new Card(9, "Treasure Trove", "Both player draw until 6", Type.spell, 0, 0, 0, Resources.Load<Sprite>("6")));
        Data.Add(new Card(10, "Baby dragon", "If this card is sent to the graveyard, draw 1 Dragon type monster from your deck", Type.dragon, 1, 1, 3, Resources.Load<Sprite>("9")));
        Data.Add(new Card(11, "Monster reborn", "Tribute 2 units, revive one monster from graveyard", Type.spell, 0, 0, 0, Resources.Load<Sprite>("11")));
        Data.Add(new Card(12, "Dragon's Valley", "+2/0 to all Dragon type on your field (only one can be used per player)", Type.spell, 0, 0, 0, Resources.Load<Sprite>("12")));
        Data.Add(new Card(13, "Dragon warrior", "+1/+1 for each Dragon type monster on your field (include this card)", Type.dragon, 2, 1, 1, Resources.Load<Sprite>("13")));
        Data.Add(new Card(14, "Dragon Rebirth", "Tribute a Dragon type monster on the field, send 1 Dragon type monster from the graveyard to your hand", Type.spell, 0, 0, 0, Resources.Load<Sprite>("5")));
        Data.Add(new Card(15, "Off-Defense", "Switch health and attack of 1 unit", Type.spell, 0, 0, 0, Resources.Load<Sprite>("19")));
        Data.Add(new Card(16, "Dragon spirit", "+1/+1 for each Dragon type monster in your graveyard", Type.dragon, 5, 2, 2, Resources.Load<Sprite>("15")));
        Data.Add(new Card(17, "Ancient dragon", "Discard 1 to summon, cannot normal summon", Type.dragon, 4, 2, 2, Resources.Load<Sprite>("16")));
        Data.Add(new Card(18, "Forbidden one", "Can only be summon by Forbidden call", Type.dragon, 7, 7, 8, Resources.Load<Sprite>("17")));
        Data.Add(new Card(19, "Forbidden call", "Tribute 2 monsters, summon a Forbidden one from your hand", Type.spell, 0, 0, 0, Resources.Load<Sprite>("18")));
    }
}
