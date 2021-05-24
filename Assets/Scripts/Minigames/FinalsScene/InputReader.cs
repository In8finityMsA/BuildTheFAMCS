using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputReader : MonoBehaviour
{
    [SerializeField] private Camera mainCamera = null;
    
    private LayerMask layer = 1 << 8;

    public event Action<GameObject> OnTouch;
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
            
            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layer);

            OnTouch?.Invoke(hitInfo.collider == null ? null : hitInfo.collider.gameObject);
        }
    }
}
