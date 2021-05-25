using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject newButton;
    public GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        newButton.GetComponent<Button>().onClick.AddListener(OnNewClick);
        continueButton.GetComponent<Button>().onClick.AddListener(OnContinueClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNewClick()
    {

    }

    void OnContinueClick()
    {

    }
}
