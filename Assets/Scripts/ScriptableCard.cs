using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScriptableCard : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    public Card cards;
    public Sprite dragon;
    public Sprite golem;
    public Sprite demon;
    public Sprite angel;
    public Sprite witch;
    public Sprite spell;
    public Image healthSprite;
    public Image attackSprite;
    public Image back;
    public Image glowEffect;

    public int id;
    public string cardName;
    public string description;
    public Type type;
    public int cost;
    public int attack;
    public int health;
    public int originalHealth;
    public int originalAttack;

    public Image DisplayImage;
    public Image typeImage;
    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Text attackText;
    public Text healthText;

    public static ScriptableCard instance;

    public GameObject Hand;

    public bool canBeSummon;
    public static bool summoned = false;
    public static bool isYourTurn;

    public GameObject cardBack;
    public Image InfoNotice;

    public bool haveAttack;

    public bool isSelectedForTribute;
    public bool isSelectedForDiscard;

    public int isBurn;
    public int isInvicible;
    public bool isSelectedForEffect;
    public bool isShielded;
    public bool isBoosted;
    public GameObject target;

    public List<GameObject> effectTarget;
    public GameObject slashEffect;
    public GameObject explodeEffect;
    public GameObject arrow;
    public GameObject temp;

    public static bool isOn = false;

    public AudioSource ffx;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        canBeSummon = false;
        isSelectedForTribute = false;
        isBoosted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Assignment")
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
            DrawCard();
            CardDisplay();
            CheckSummon();
            GlowControl();
            AttackPhase();
            if (health <= 0 && type != Type.spell)
            {
                health = originalHealth;
                if (healthText != null)
                    healthText.text = originalHealth.ToString();
                attack = originalAttack;
                if (attackText != null)
                    attackText.text = originalAttack.ToString();
                glowEffect.gameObject.SetActive(false);
                GameControl.instance.SendToGraveYard(gameObject);
            }
        }
        else
        {
            CardDisplay();
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
    }
    void CardDisplay()
    {
        if (cards != null)
        {
            StartCoroutine(TypeCheck());
            DisplayImage.sprite = cards.Image;
            id = cards.id;
            cardName = cards.cardName;
            description = cards.description;
            type = cards.type;
            cost = cards.cost;
            attack = cards.attack;
            health = cards.health;
            originalHealth = cards.health;
            originalAttack = cards.attack;
            nameText.text = "" + cardName;
            descriptionText.text = "" + description;
            if (type != Type.spell)
            {
                costText.text = "" + cost;
                attackText.text = "" + attack;
                healthText.text = "" + health;
            }
            cards = null;
        }
    }

    IEnumerator TypeCheck()
    {
        yield return null;
        switch (type)
        {
            case Type.spell:
                {
                    typeImage.sprite = spell;
                    break;
                }
            case Type.dragon:
                {
                    typeImage.sprite = dragon;
                    break;
                }
            case Type.golem:
                {
                    typeImage.sprite = golem;
                    break;
                }
            case Type.demon:
                {
                    typeImage.sprite = demon;
                    break;
                }
            case Type.angel:
                {
                    typeImage.sprite = angel;
                    break;
                }
            case Type.witch:
                {
                    typeImage.sprite = witch;
                    break;
                }
        }
    }

    void DrawCard()
    {
        if (transform.name[..5] != "Enemy")
            Hand = GameObject.Find("Hand");
        else Hand = GameObject.Find("EnemyHand");
        if (CompareTag("Card"))
        {
            if (Hand.name == "Hand")
            {
                cards = PlayerDeck.instance.deck[PlayerDeck.instance.numsOfCard - 1];
                PlayerDeck.instance.numsOfCard--;
            } else
            {
                cards = EnemyDeck.instance.deck[EnemyDeck.instance.numsOfCard - 1];
                EnemyDeck.instance.numsOfCard--;
            }
            tag = "Untagged";
        }
    }

    void CheckSummon()
    {
        if (!summoned)
        {
            if (cost <= 4) canBeSummon = true;
            else if (cost <= 6 && GameControl.instance.monsterOnField >= 1) canBeSummon = true;
            else if (GameControl.instance.monsterOnField > 1) canBeSummon = true;
            else canBeSummon = false;
        }
        else canBeSummon = false;
    }

    void GlowControl()
    {
        if (isYourTurn && glowEffect != null && GameControl.instance.MainPhase)
        {
            if (GameControl.instance.OutOfNumber && transform.parent.name == "Hand")
            {
                glowEffect.gameObject.SetActive(true);
            }
            else if (type == Type.spell)
            {
                if (transform.parent.name == "Hand")
                    glowEffect.gameObject.SetActive(true);
                else if (transform.parent.name[..4] == "Spel")
                    glowEffect.gameObject.SetActive(false);
            }
            else if (!summoned)
            {
                if ((canBeSummon && transform.parent.name[..4] == "Hand") || (transform.parent.name[..4] == "Card" && isSelectedForTribute))
                {
                    glowEffect.gameObject.SetActive(true);
                }
                else
                    glowEffect.gameObject.SetActive(false);
            }
            else if (isSelectedForEffect)
                glowEffect.gameObject.SetActive(true);
            else glowEffect.gameObject.SetActive(false);
        }
        else if (glowEffect != null) 
            glowEffect.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Assignment")
        {
            if (!GameControl.instance.TributeSummon && !GameControl.instance.selectForEffect)
            {
                CheckExist();
                if (cardBack.activeSelf && transform.parent.name[..4] != "Spel") return;
                Image temp = Instantiate(InfoNotice);
                InfoPanelControl.instance.summonButton.gameObject.SetActive(false);
                InfoPanelControl.instance.activateSpell.gameObject.SetActive(false);
                InfoPanelControl.instance.setSpell.gameObject.SetActive(false);
                InfoPanelControl.instance.activateSet.gameObject.SetActive(false);
                InfoPanelControl.instance.discard.gameObject.SetActive(false);
                temp.transform.SetParent(GameObject.Find("Canvas").transform);
                temp.GetComponent<RectTransform>().localScale = Vector3.one;
                temp.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                if ((gameObject.name[..4] != "Enem" && transform.parent.name == "Hand") || type == Type.spell)
                {
                    InfoPanelControl.instance.isSelected = gameObject;
                }
                else InfoPanelControl.instance.isSelected = null;
                InfoPanelControl.instance.descriptionData = description;
                InfoPanelControl.instance.typeData = type.ToString();
                InfoPanelControl.instance.typeImageData = typeImage;
                InfoPanelControl.instance.nameData = cardName;
                if (type != Type.spell)
                    InfoPanelControl.instance.statusData = "| Atk: " + attack + " | Hp: " + health + " | Cost: " + cost;
                else
                    InfoPanelControl.instance.statusData = "";
                ffx.PlayOneShot(ffx.clip);
                if (transform.parent.name == "Hand" && isYourTurn)
                {
                    if (GameControl.instance.OutOfNumber)
                    {
                        InfoPanelControl.instance.discard.gameObject.SetActive(true);
                    }
                    else if (type != Type.spell)
                    {
                        if (canBeSummon)
                            InfoPanelControl.instance.summonButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        InfoPanelControl.instance.activateSpell.gameObject.SetActive(true);
                        InfoPanelControl.instance.setSpell.gameObject.SetActive(true);
                    }
                }
                else if (GameControl.instance.spellChainSelect)
                {
                    if (type == Type.spell && transform.parent.name[..4] == "Spel")
                        InfoPanelControl.instance.activateSet.gameObject.SetActive(true);
                }
                else if (transform.parent.name[..4] == "Spel" && cardBack.activeSelf && isYourTurn)
                {
                    InfoPanelControl.instance.activateSet.gameObject.SetActive(true);
                }
            }
            else if (GameControl.instance.selectForEffect)
            {
                if (type != Type.spell && !cardBack.activeSelf && transform.parent.name != "Hand")
                {
                    if (!isSelectedForEffect)
                    {
                        glowEffect.gameObject.SetActive(true);
                        isSelectedForEffect = true;
                        CardEffectControl.instance.effectChain[CardEffectControl.instance.effectChain.Count - 1].GetComponent<ScriptableCard>().effectTarget.Add(eventData.pointerClick);
                        EffectNotice.instance.numOfChosen++;
                    }
                    else
                    {
                        glowEffect.gameObject.SetActive(false);
                        isSelectedForEffect = false;
                        CardEffectControl.instance.effectChain[CardEffectControl.instance.effectChain.Count - 1].GetComponent<ScriptableCard>().effectTarget.Remove(eventData.pointerClick);
                        EffectNotice.instance.numOfChosen--;
                    }
                }
            }
            else if (transform.parent.name[..4] == "Card")
            {
                if (!isSelectedForTribute)
                {
                    isSelectedForTribute = true;
                }
                else isSelectedForTribute = false;
            }
        }
        else
        {
            if (EditorControl.instance.selectedForEdit == null || EditorControl.instance.selectedForEdit != gameObject)
            {
                EditorControl.instance.CardName.text = cardName;
                EditorControl.instance.TypeText.text = "Type: " + type.ToString();
                EditorControl.instance.Cost.text = "Cost: " + cost.ToString();
                EditorControl.instance.ATK.text = "Attack: " + attack.ToString();
                EditorControl.instance.HP.text = "Health: " + health.ToString();
                EditorControl.instance.Description.text = description;
                if (!isOn)
                {
                    glowEffect.gameObject.SetActive(true);
                    EditorControl.instance.selectedForEdit = gameObject;
                    isOn = true;
                }
                else
                {
                    EditorControl.instance.selectedForEdit.GetComponent<ScriptableCard>().glowEffect.gameObject.SetActive(false);
                    glowEffect.gameObject.SetActive(true);
                    EditorControl.instance.selectedForEdit = gameObject;
                }
                ffx.PlayOneShot(ffx.clip);
                EditorControl.instance.isSelectedForEdit = true;
                return;
            }
            else
            {
                EditorControl.instance.CardName.text = "";
                EditorControl.instance.TypeText.text = "";
                EditorControl.instance.Cost.text = "";
                EditorControl.instance.ATK.text = "";
                EditorControl.instance.HP.text = "";
                EditorControl.instance.Description.text = "";
                glowEffect.gameObject.SetActive(false);
                EditorControl.instance.selectedForEdit = null;
                ffx.PlayOneShot(ffx.clip);
                isOn = false;
                EditorControl.instance.isSelectedForEdit = false;
                return;
            }
        }
    }

    void CheckExist()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            for (int i = 0; i < canvas.transform.childCount; i++)
            {
                if (canvas.transform.GetChild(i).name == "CardInfoView(Clone)")
                {
                    Destroy(canvas.transform.GetChild(i).gameObject);
                    break;
                }
            }
        }
    }

    void AttackPhase()
    {
        if (GameControl.instance.Attack && isYourTurn)
        {
            if (!haveAttack && transform.parent.name[..4] == "Card")
            {
                haveAttack = false;
                if (glowEffect != null)
                    glowEffect.gameObject.SetActive(true);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Assignment")
        {
            if (temp != null) Destroy(temp);
            if (haveAttack) return;
            if (GameControl.instance.Attack && isYourTurn)
            {
                if (transform.parent.name[..4] == "Card")
                {
                    if ((eventData.pointerEnter.transform.parent.parent != null ||
                        eventData.pointerEnter.transform.parent.parent != null) &&
                       (eventData.pointerEnter.transform.parent.parent.name == "EnemyHand" ||
                        eventData.pointerEnter.transform.name == "EnemyHand") &&
                        GameControl.instance.canDirectAttack)
                    {
                        GameControl.instance.enemyHealth -= attack;
                        StartCoroutine(GameControl.instance.CameraShake(.5f, .3f));
                        haveAttack = true;
                        if (glowEffect != null)
                            glowEffect.gameObject.SetActive(false);
                    }
                    else if (eventData.pointerEnter.transform.parent.name.Length > 14 &&
                            !GameControl.instance.canDirectAttack)
                    {
                        if (eventData.pointerEnter.transform.parent.name[..16] == "EnemyMonsterCard")
                        {
                            target = eventData.pointerEnter.transform.parent.gameObject;
                            eventData.pointerEnter.transform.parent.GetComponent<ScriptableCard>().target = gameObject;
                            HealthUpdate();
                        }
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameControl.instance.Attack)
            temp = Instantiate(arrow);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameControl.instance.Attack)
        {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            temp.transform.position = eventData.pointerClick.transform.position;
            Vector3 direction = (mouse_position - eventData.pointerClick.transform.position).normalized;
            float rotation_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            temp.transform.rotation = Quaternion.Euler(0, 0, rotation_z);
            temp.GetComponent<SpriteRenderer>().size = new Vector2((Vector3.Distance(mouse_position, eventData.pointerClick.transform.position) - 9) * 2, temp.GetComponent<SpriteRenderer>().size.y);
        }
    }

    public async void HealthUpdate()
    {
        await SlashEffect();
        if (target.GetComponent<ScriptableCard>().isShielded)
            target.GetComponent<ScriptableCard>().health -= attack + 1;
        else if (target.GetComponent<ScriptableCard>().isInvicible == 0)
            target.GetComponent<ScriptableCard>().health -= attack;
        if (isShielded)
            health -= target.GetComponent<ScriptableCard>().attack + 1;
        else if (isInvicible == 0)
            health -= target.GetComponent<ScriptableCard>().attack;
        healthText.text = health.ToString();
        target.GetComponent<ScriptableCard>().healthText.text = ""
            + target.GetComponent<ScriptableCard>().health;
        haveAttack = true;
        CardEffectControl.instance.CardIdCheck(gameObject, true);
        CardEffectControl.instance.CardTypeCheck(gameObject, true);
        CardEffectControl.instance.CardIdCheck(target, true);
        CardEffectControl.instance.CardTypeCheck(target, true);
        await Task.Yield();
    }

    public async void EnemyHealthUpdate()
    {
        await EnemySlashEffect();
        if (target.GetComponent<ScriptableCard>().isShielded)
            target.GetComponent<ScriptableCard>().health -= attack + 1;
        else if (target.GetComponent<ScriptableCard>().isInvicible == 0)
            target.GetComponent<ScriptableCard>().health -= attack;
        if (isShielded)
            health -= target.GetComponent<ScriptableCard>().attack + 1;
        else if (isInvicible == 0)
            health -= target.GetComponent<ScriptableCard>().attack;
        target.GetComponent<ScriptableCard>().healthText.text = target.GetComponent<ScriptableCard>().health.ToString();
        healthText.text = health.ToString();
        CardEffectControl.instance.CardIdCheck(gameObject, true);
        CardEffectControl.instance.CardTypeCheck(gameObject, true);
        CardEffectControl.instance.CardIdCheck(target, true);
        CardEffectControl.instance.CardTypeCheck(target, true);
        await Task.Yield();
    }

    public async Task SlashEffect()
    {
        GameObject temp1 = Instantiate(slashEffect);
        GameObject temp2 = Instantiate(slashEffect);
        temp1.transform.position = gameObject.transform.position;
        temp2.transform.position = target.transform.position;
        temp2.transform.eulerAngles = new Vector3(0, 0, 180);
        await Task.Delay(1200);
        Destroy(temp1);
        Destroy(temp2);
    }

    public async Task EnemySlashEffect()
    {
        GameObject temp1 = Instantiate(slashEffect);
        GameObject temp2 = Instantiate(slashEffect);
        temp1.transform.eulerAngles = new Vector3(0, 0, 180);
        temp1.transform.position = gameObject.transform.position;
        temp2.transform.position = target.transform.position;
        await Task.Delay(1200);
        Destroy(temp1);
        Destroy(temp2);
    }
}
