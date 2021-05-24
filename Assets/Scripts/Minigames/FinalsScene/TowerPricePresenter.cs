using TMPro;
using UnityEngine;

public class TowerPricePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text price = null;
    [SerializeField] private TowerSO tower = null;
    
    private void Awake()
    {
        price.SetText($"{tower.buildPrice}");
    }
}
