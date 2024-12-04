using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class trashManager : MonoBehaviour
{
    public static trashManager instance;

    public TMP_Text heldText;
    public TMP_Text depoText;

    int held = 0;
    int depo = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        heldText.text = "Held: " + held.ToString() + "/5";
        depoText.text = "Deposited: " + depo.ToString();
    }

    public void depositTrash()
    {
        depo += held;
        held = 0;
        heldText.text = "Held: " + held.ToString() + "/5"; //idk how to reference maxTrash so 5 is there temp
        depoText.text = "Deposited: " + depo.ToString();
    }

    public void pickTrash()
    {
        held += 1;
        heldText.text = "Held: " + held.ToString() + "/5";
    }

}
