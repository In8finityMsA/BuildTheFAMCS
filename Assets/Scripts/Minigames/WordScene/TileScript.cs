using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public bool onTriggerNow;
    List<Collider2D> collist = new List<Collider2D>();
    public Collider2D collision;
    public Collider2D colwelc;
    public Collider2D colbue;
    public bool mouseDown;
    public bool should;

    void Start()
    {
        onTriggerNow = false;
        should = false;
    }

 
    void Update()
    {
        if (GameObject.Find("ManagerEmpty").GetComponent<ManagerWordsScript>().IsPlaying)
        {
            if (collist.Count != 0)
            {
                foreach (var item in collist)
                {
                    if (item.gameObject.GetComponent<WordScript>().mouseDown)
                    {
                        return;
                    }
                }
                collist[collist.Count - 1].gameObject.transform.rotation = Quaternion.identity;
                collist[collist.Count - 1].gameObject.transform.position = transform.position;
            }
        }
        //if (onTriggerNow && !collision.gameObject.GetComponent<WordScript>().mouseDown)
        //{
        //    collision.gameObject.transform.rotation = Quaternion.identity;
        //    collision.gameObject.transform.position = transform.position;
        //}
        //if (collist.Count != 0 && !collist[collist.Count - 1].gameObject.GetComponent<WordScript>().mouseDown)
        //{
        //    collist[collist.Count - 1].gameObject.transform.rotation = Quaternion.identity;
        //    collist[collist.Count - 1].gameObject.transform.position = transform.position;
        //}

        //if (/*!should && */onTriggerNow && !collision.gameObject.GetComponent<WordScript>().mouseDown)
        //{
        //    collision.gameObject.transform.rotation = Quaternion.identity;
        //    collision.gameObject.transform.position = transform.position;
        //}
        //else if (should && !collision.gameObject.GetComponent<WordScript>().mouseDown)
        //{
        //    collision.gameObject.transform.rotation = Quaternion.identity;
        //    collision.gameObject.transform.position = transform.position;
        //}
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision != colwelc)
        //{
        //    should = true;
        //}
        //else
        //{
        //    should = false;
        //}
        collist.Remove(collision);
        onTriggerNow = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        this.collision = collision;
        onTriggerNow = true;
        collist.Add(collision);        
        Debug.Log("trigenter");
        colwelc = collision;
    }
}
