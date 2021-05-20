using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    public static EndScript Instance { get; private set; }
    public ButtonScript btn;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        GameObject button = gameObject.transform.GetChild(0).gameObject;
        btn = button.GetComponent<ButtonScript>();
        //btn = script;
        //button.onClick.AddListener(OnClick);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
