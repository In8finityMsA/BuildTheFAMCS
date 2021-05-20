using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEndScript : MonoBehaviour
{
    public static TheEndScript Instance { get; private set; }
    public ButtonEndScript btn;


    void Start()
    {
        Instance = this;

        GameObject button = gameObject.transform.GetChild(0).gameObject;
        btn = button.GetComponent<ButtonEndScript>();
        gameObject.SetActive(false);
    }


    void Update()
    {

    }
}
