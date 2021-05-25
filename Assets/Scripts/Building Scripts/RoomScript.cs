using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomScript : MonoBehaviour
{
    private RoomScriptableObject roomInfo;
    private bool isInit = false;
    public GameObject characterPrefab;
    private DateTime time;

    // Shit code for fix missclicks
    private GameObject Camera;
    private Vector3 startDrag;
    private Vector3 endDrag;

    internal void Init(RoomScriptableObject room)
    {
        Camera = GameObject.Find("Main Camera");

        if (isInit) return;
        
        isInit = true;
        roomInfo = room;
        if (roomInfo == null)  {Debug.Log("Null room"); return;}

        roomInfo.script = this;
        SetSprite();

        foreach (var character in roomInfo.characters)
        {
            Debug.Log(character);
        }

        if (roomInfo.isUnlocked)
        {
            DrawCharacters();
        }
    }

    private void SetSprite()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = roomInfo.isUnlocked ? roomInfo.constructedSprite : roomInfo.underConstructionSprite;

        var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = boxCollider.transform.InverseTransformVector(spriteRenderer.bounds.size);
    }

    public void RoomUnlock()
    {
        //MainManager.Instance.Money -= roomInfo.costToBuild;
        roomInfo.isUnlocked = true;
        MainManager.Instance.SetRoomUnlocked(roomInfo, true);
        SetSprite();
        DrawCharacters();
    }

    private void DrawCharacters()
    {
        for (int i = 0; i < roomInfo.characters.Count; i++)
        {
            var characterObject = Instantiate(characterPrefab, new Vector2 (gameObject.transform.position.x,
                                                                            (float)(gameObject.transform.position.y - 0.8)),
                                                                            Quaternion.identity);
            characterObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
            characterObject.transform.SetParent(gameObject.transform);
            //characterObject.transform.localPosition =
            //  new Vector3(i < roomInfo.charactersPositions.Count ? roomInfo.charactersPositions[i].x : 0,
            //    i < roomInfo.charactersPositions.Count ? roomInfo.charactersPositions[i].y : 0, CharactersLayerZ);

            characterObject.GetComponent<CharacterScript>().Init(roomInfo.characters[i]);
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on UI");
            return;
        }
        startDrag = Camera.transform.position;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clicked on UI");
            return;
        }
    }
    
    void OnMouseUp()
    {
        endDrag = Camera.transform.position;
        if (startDrag == endDrag)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on UI");
                return;
            }
            
            time = System.DateTime.Now;


            if (System.DateTime.Now - time < new TimeSpan(0, 0, 1))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on UI");
                    return;
                }

                Debug.Log($"Room Clicked! floor: {roomInfo.floor}, index: {roomInfo.indexInFloor}.");
                if (roomInfo.isUnlocked == true)
                {
                    roomInfo.characters[0].script.StartDialog();
                }
                if (roomInfo.isUnlocked == false)
                {
                    var buyArray = JsonLoader.GetJsonArrayFromFile(roomInfo.jsonFilename);
                    MainManager.Instance.StartDialog(buyArray);
                }

            }
        }
    }
    
}
