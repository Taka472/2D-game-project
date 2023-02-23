using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerClickHandler
{
    public GameObject summonEffect;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameControl.instance.MainPhase)
        {
            if (GameControl.instance.Summon && transform.name[..4] == "Card" && InfoPanelControl.instance.isSelected != null)
            {
                InfoPanelControl.instance.isSelected.transform.SetParent(transform);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().anchoredPosition = transform.InverseTransformPoint(transform.position) + new Vector3(26f, -44f, 0);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .35f);
                ScriptableCard.summoned = true;
                GameControl.instance.monsterOnField++;
                GameControl.instance.cardInHand--;
                GameControl.instance.TributeSummon = false;
                GameControl.instance.Summon = false;
                CardEffectControl.instance.CardTypeCheck(InfoPanelControl.instance.isSelected.gameObject, false);
                CardEffectControl.instance.CardIdCheck(InfoPanelControl.instance.isSelected.gameObject, false);
                Effect();
                for (int i = 0; i < 5; i++)
                {
                    if (GameControl.instance.spellZone[i].transform.childCount == 1)
                    {
                        if (!GameControl.instance.spellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.activeSelf)
                        {
                            CardEffectControl.instance.CardIdCheck(GameControl.instance.spellZone[i].transform.GetChild(0).gameObject, true);
                        }
                    }
                    if (GameControl.instance.enemySpellZone[i].transform.childCount == 1)
                    {
                        if (!GameControl.instance.enemySpellZone[i].transform.GetChild(0).GetComponent<ScriptableCard>().cardBack.activeSelf)
                        {
                            CardEffectControl.instance.CardIdCheck(GameControl.instance.enemySpellZone[i].transform.GetChild(0).gameObject, true);
                        }
                    }
                }
            }
            if (GameControl.instance.ActivateSpell && transform.name[..4] == "Spel" && InfoPanelControl.instance.isSelected != null)
            {
                InfoPanelControl.instance.isSelected.transform.SetParent(transform);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().anchoredPosition = transform.InverseTransformPoint(transform.position) + new Vector3(26f, -44f, 0);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .35f);
                GameControl.instance.spellOnField++;
                GameControl.instance.cardInHand--;
                GameControl.instance.ActivateSpell = false;
                CardEffectControl.instance.CardIdCheck(InfoPanelControl.instance.isSelected.gameObject, false);
                Effect();
            }
            else if (GameControl.instance.SetSpell && transform.name[..4] == "Spel" && InfoPanelControl.instance.isSelected != null)
            {
                InfoPanelControl.instance.isSelected.transform.SetParent(transform);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().anchoredPosition = transform.InverseTransformPoint(transform.position) + new Vector3(26f, -44f, 0);
                InfoPanelControl.instance.isSelected.GetComponent<RectTransform>().localScale = new Vector3(.35f, .35f, .35f);
                InfoPanelControl.instance.isSelected.GetComponent<ScriptableCard>().cardBack.SetActive(true);
                GameControl.instance.spellOnField++;
                GameControl.instance.cardInHand--;
                GameControl.instance.SetSpell = false;
                Effect();
            }
        }
    }

    async void Effect()
    {
        GameObject temp = Instantiate(summonEffect);
        temp.transform.position = transform.position;
        await Task.Delay(1000);
        Destroy(temp);
    }
}