using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private RoomScriptableObject roomInfo;
    private bool isInit = false;
    public GameObject characterPrefab;

    internal void Init(RoomScriptableObject room)
    {
        if (isInit) return;
        
        isInit = true;
        roomInfo = room;
        if (roomInfo == null)  {Debug.Log("Null room"); return;}
        
        SetSprite();

        foreach (var character in roomInfo.characters)
        {
            Debug.Log(character);
        }
    }

    void SetSprite()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = roomInfo.isUnlocked ? roomInfo.constructedSprite : roomInfo.underConstructionSprite;

        var boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = boxCollider.transform.InverseTransformVector(spriteRenderer.bounds.size);
    }

    void OnMouseDown()
    {
        Debug.Log($"Room Clicked! floor: {roomInfo.floor}, index: {roomInfo.indexInFloor}.");
        roomInfo.isUnlocked = true;
        SetSprite();

        for (int i = 0; i < roomInfo.characters.Count; i++)
        {
            var characterObject = Instantiate(characterPrefab, gameObject.transform.position, Quaternion.identity);
            characterObject.GetComponent<SpriteRenderer>().sortingLayerName = "Characters";
            characterObject.transform.SetParent(gameObject.transform);
            //characterObject.transform.localPosition =
              //  new Vector3(i < roomInfo.charactersPositions.Count ? roomInfo.charactersPositions[i].x : 0,
                //    i < roomInfo.charactersPositions.Count ? roomInfo.charactersPositions[i].y : 0, CharactersLayerZ);
            
            characterObject.GetComponent<CharacterScript>().Init(roomInfo.characters[i]);
        }
    }
    
}
