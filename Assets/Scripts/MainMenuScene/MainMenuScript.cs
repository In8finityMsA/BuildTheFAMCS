using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject newButton;
    public GameObject continueButton;
    public GameObject background;
    // Start is called before the first frame update
    void Start()
    {
        newButton.GetComponent<Button>().onClick.AddListener(OnNewClick);
        continueButton.GetComponent<Button>().onClick.AddListener(OnContinueClick);


        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        Debug.Log(spriteRenderer.ToString());
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Debug.Log("good");
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        scale.x *= cameraSize.x / spriteSize.x;
        scale.y *= cameraSize.y / spriteSize.y;
        background.transform.position = Vector2.zero; // Optional
        background.transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNewClick()
    {
        MainManager.newGame = true;
        SceneManager.LoadScene("MainScene");
    }

    void OnContinueClick()
    {
        MainManager.newGame = false;
        SceneManager.LoadScene("MainScene");
    }
}
