using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerWordsScript : MonoBehaviour
{
    public GameObject wordPrefab;
    public GameObject tilePrefab;
    public bool mouseDown;
    public float time;
    public Rect cameraRect;

    public static ManagerWordsScript Instance { get; private set; }
    char[] array;
    string text;
    List<GameObject> words = new List<GameObject>();
    List<GameObject> tiles = new List<GameObject>();
    Dictionary<GameObject, int> dict = new Dictionary<GameObject, int>();
    List<char> letters = new List<char>();

    private bool isClicked = false;
    // Start is called before the first frame update
    private void Awake()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        letters = new List<char>() { 'l', 'o', 'v', 'e', 'r' };
        time = 10;
        mouseDown = false;
        Instance = this;
        //params 
        text = "Learn fourth grade math—arithmetic, measurement, _________, fractions, and more. This course is aligned with Common Core standards.";
        array = new char[] { 'g', 'e', 'o','m','e','t','r','y' }; 

        int i = 0;
        List<int> arraySeq = new List<int>();
        for (int j = 0; j < array.Length; j++)
        {
            arraySeq.Add(j);
        }
        Debug.Log("Pass1");
        List<int> gener = new List<int>();
        System.Random rand = new System.Random();
        Debug.Log("arraySeqCount" + arraySeq.Count);
        for (int g = arraySeq.Count - 1; g >= 0; g--)
        {
            int ind = rand.Next(0, g);
            Debug.Log("arraySeq[ind]" + arraySeq[ind]);
            gener.Add(arraySeq[ind]);
            arraySeq.RemoveAt(ind);
        }
        Debug.Log("Pass2");
        Debug.Log("!!!!" + gener.Count);
        for (int r = 0; r < gener.Count; r++)
        {
            Debug.Log("!!!!"+gener[r]);
        }
        GameObject gameOb;
        Debug.Log("Pass3");
        float xCoordTile = (float)cameraRect.x + (float)cameraRect.width / 2 - (float) array.Length * (float)2/2;
        float yCoordTile = (float)cameraRect.y + (float)cameraRect.height / 3;
        float yCoordWords = (float)cameraRect.y + (float) cameraRect.height / 7;


        //gameObject.GetComponentInChildren<Canvas>().gameObject.transform = new RectTransform(3, 3, 4)
        gameObject.GetComponentInChildren<Canvas>().gameObject.transform.position = new Vector3(cameraRect.x + cameraRect.width / 2, cameraRect.y + 3*cameraRect.height / 4, 0);

        Debug.Log($"{gameObject.GetComponentInChildren<Canvas>().gameObject.transform.position}");
        gameObject.GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Text>().text = text;


        foreach (var item in array)
        {
            gameOb = Instantiate(tilePrefab, new Vector3(xCoordTile + 2 * i, yCoordTile, 0), Quaternion.identity);
            Debug.Log(gameOb.transform.position.ToString());
            tiles.Add(gameOb);

            i++;

        }
        i = 0;

        foreach (var item in array)
        {
            gameOb = Instantiate(wordPrefab, new Vector3(xCoordTile + 2 * i, yCoordWords, 0), Quaternion.identity);
            Debug.Log("word was made");
            
            gameOb.GetComponentInChildren<Canvas>().gameObject.GetComponentInChildren<Text>().text = array[gener[i]].ToString();
            dict.Add(gameOb, gener[i]);
            words.Add(gameOb);
            i++;
        }
        
       
    }
    void Start()
    {
        
    }

    public bool IsFree(Collider2D collider)
    {
        foreach (var item in words)
        {
            if (item != collider.gameObject && item.transform.position == collider.gameObject.transform.position)
            {
                return false;
            }
        }
        return true;
    }
    void Clear()
    {
        int len = array.Length;
        for (int i = 0; i < len; i++)
        {
            words[i].SetActive(false);
            tiles[i].SetActive(false);
        }
    }
    bool HasWon()
    {
        int scores;
        for (int i = 0; i < array.Length; i++)
        {
            scores = 0;
            for (int k = 0; k < array.Length; k++)
            {
                if (tiles[i].transform.position == words[k].transform.position)
                {
                    if (!(array[dict[words[k]]]).Equals(array[i]))
                    {
                        return false;
                    }
                    else
                    {
                        scores++;
                    }
                }
                
            }
            if (scores == 0)
            {
                return false;
            }
        }
        return true;
        //for (int i = 0; i < array.Length; i++)
        //{
        //    if (words[i].transform.position != tiles[dict[words[i]]].transform.position)
        //    {
        //        return false;
        //    }
        //}
        //return true;
        //for (int i = 0; i < array.Length; i++)
        //{
        //    if (words[i].transform.position != tiles[i].transform.position)
        //    {
        //        return false;
        //    }
        //}      
        //return true;
    }
    void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }
    }
    // Update is called once per frame
    void Update()
    {

        
        time -= Time.deltaTime;
        if (time <= 0 )
        {
            TheEndTileScript.Instance.gameObject.GetComponentInChildren<Button>().onClick.AddListener(OnClick);
            if (HasWon())
            {
                TheEndTileScript.Instance.gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "You have won!";
                
            }
            else
            {
                TheEndTileScript.Instance.gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "You have lost!";
            }
            TheEndTileScript.Instance.gameObject.SetActive(true);
            Clear();
        }
    }
}
