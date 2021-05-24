using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] private InputReader input = null;
    [SerializeField] private GameStateController gameState = null;
    [HideInInspector] public GameObject target = null;

    private void Awake()
    {
        input.OnTouch += Open;
        gameObject.SetActive(false);
    }
    
    private void Open(GameObject obj)
    {
        if (obj == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (ObjectIsTower(obj) == false && ObjectIsBuildPlaceAndDontHaveTower(obj) == false){
            return;
        }     
        if (obj.transform.childCount == 1)
            target = obj.transform.GetChild(0).gameObject;
        else
            target = obj;
        
        
        gameObject.SetActive(true);
        transform.position = target.transform.position;
    }

    private bool ObjectIsTower(GameObject obj) => obj.CompareTag("Tower");
    private bool ObjectIsBuildPlaceAndDontHaveTower(GameObject obj) => obj.transform.childCount >= 1 && obj.CompareTag("Build Place");

    public void OnTowerSell()
    {
        gameState.Sell(target.GetComponent<Tower>().SellPrice());
        Destroy(target);
        gameObject.SetActive(false);
    }
}
