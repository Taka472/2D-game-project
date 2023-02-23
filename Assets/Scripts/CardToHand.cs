using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardToHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject It;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Assignment")
        {
            if (transform.name[..5] != "Enemy")
                Hand = GameObject.Find("Hand");
            else Hand = GameObject.Find("EnemyHand");
            It.transform.SetParent(Hand.transform);
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3(transform.position.x, transform.position.y);
            It.transform.eulerAngles = new Vector3(25, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
