using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerBugsScript : MonoBehaviour
{
    Rect cameraRect;
    public GameObject background;
    Canvas canv;
    public GameObject top;
    public float timer;
    public GameObject bottom;
    public GameObject right;
    public GameObject left;
    public static ManagerBugsScript Instance { get; private set; }
    // Start is called before the first frame update
    public GameObject bugPrefab;
    public List<GameObject> bugs = new List<GameObject>();
    public float speed;
    public int bugAmount;
    private bool isClicked = false;

    public float Parabola(float x)
    {
        return x * x;
    }
    public float Sq(float x)
    {
        return (float)System.Math.Sqrt(x);
    }
    private void Awake()
    {
        timer = 10;
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        bottom.transform.position = new Vector3(cameraRect.x, cameraRect.y, 0);
        top.transform.position = new Vector3(cameraRect.x, cameraRect.y + cameraRect.height, 0);
        left.transform.position = new Vector3(cameraRect.x, cameraRect.y, 0);
        right.transform.position = new Vector3(cameraRect.x + cameraRect.width, cameraRect.y + cameraRect.height, 0);

        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        scale.x *= cameraSize.x / spriteSize.x;
        scale.y *= cameraSize.y / spriteSize.y;
        background.transform.position = Vector2.zero; // Optional
        background.transform.localScale = scale;


        bugAmount = 5;
        speed = 2;
        Instance = this;
        System.Random rand = new System.Random();
        for (int i = 0; i < bugAmount; i++)
        {
            bugs.Add(Instantiate(bugPrefab, new Vector3(rand.Next((int)(cameraRect.x * 50), (int)((cameraRect.x + cameraRect.width) * 50))/100, rand.Next((int)(cameraRect.y * 50), (int)((cameraRect.y + cameraRect.height) * 50)) / 100 , 0),  Quaternion.identity));
        }
        canv = gameObject.GetComponentInChildren<Canvas>();

    }
    void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            SceneManager.LoadScene("MainScene");
        }

    }
    void Start()
    {
        
        gameObject.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        Debug.Log("set act false");
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && bugs.Count != 0)
        {
            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].SetActive(false);
            }
            canv.gameObject.GetComponentInChildren<Button>().onClick.AddListener(OnClick);
            canv.gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "You have lost!";
            canv.gameObject.SetActive(true);
        }
        if (bugs.Count == 0)
        {
            canv.gameObject.GetComponentInChildren<Button>().onClick.AddListener(OnClick);
            canv.gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "You have won!";
            canv.gameObject.SetActive(true);
        }
    }
}
