using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEndTileScript : MonoBehaviour
{
    public static TheEndTileScript Instance { get; private set; }
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
