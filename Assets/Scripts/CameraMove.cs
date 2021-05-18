using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Tilemap tilemap;
    public Camera cameraObj;
    
    private Rect cameraBoundDefault;
    private Rect cameraBound;
    private Vector3 cameraTarget;

    private Vector3 CameraTarget
    {
        get => cameraTarget;
        set
        {
            cameraTarget = new Vector3(Mathf.Clamp(value.x, cameraBound.xMin, cameraBound.xMax),
                Mathf.Clamp(value.y, cameraBound.yMin, cameraBound.yMax),
                transform.position.z);
        }
    }
    public float cameraSpeedDefaultX;
    public float cameraSpeedDefaultY;
    private float cameraSpeedX;
    private float cameraSpeedY;
    public float cameraSmoothing = 5;
    
    private Vector3 vecZeroStart;
    private Vector3 vecOneStart;
    private Vector3 vecCenterStart;
    public float zoomOutMin;
    public float zoomOutMax;
    public float zoomOutSensitivity;
    private float zoomCurrent;
    private float zoomOutDefault;

    void Start()
    {
        if (cameraObj == null)
            cameraObj = Camera.main;
        
        cameraBoundDefault.xMin = tilemap.GetComponent<TilemapRenderer>().bounds.min.x + cameraObj.orthographicSize * 2; //TODO: Why -orthSize * 2?
        cameraBoundDefault.xMax = tilemap.GetComponent<TilemapRenderer>().bounds.max.x - cameraObj.orthographicSize * 2;
        cameraBoundDefault.yMin = tilemap.GetComponent<TilemapRenderer>().bounds.min.y + cameraObj.orthographicSize;
        cameraBoundDefault.yMax = tilemap.GetComponent<TilemapRenderer>().bounds.max.y * 3; //TODO: set it properly
        Debug.Log(cameraBoundDefault.xMin + " " + cameraBoundDefault.xMax + " " + cameraBoundDefault.yMin + " " + cameraBoundDefault.yMax);
        Debug.Log(tilemap.GetComponent<TilemapRenderer>().bounds.min.x + " " +
                  tilemap.GetComponent<TilemapRenderer>().bounds.max.x + " " +
                  tilemap.GetComponent<TilemapRenderer>().bounds.min.y + " " +
                  tilemap.GetComponent<TilemapRenderer>().bounds.max.y);

        cameraBound = cameraBoundDefault;
        zoomOutDefault = cameraObj.orthographicSize;
        zoomCurrent = zoomOutDefault;
        zoomOutMax = Mathf.Min((tilemap.GetComponent<TilemapRenderer>().bounds.max.x - tilemap.GetComponent<TilemapRenderer>().bounds.min.x) / cameraObj.aspect / 2, zoomOutMax);

        cameraTarget = transform.position;
        cameraSpeedX = cameraSpeedDefaultX;
        cameraSpeedY = cameraSpeedDefaultY;
    }

    // Update is called once per frame
    void Update()
    {
    #if UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                var touchMove = Input.GetTouch(0).deltaPosition;
                var newX = CameraTarget.x - touchMove.x * Time.deltaTime * cameraSpeedX;
                var newY = CameraTarget.y - touchMove.y * Time.deltaTime * cameraSpeedY;
                CameraTarget = new Vector3(newX,newY, transform.position.z);
            }
            vecZeroStart = cameraObj.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                vecOneStart = cameraObj.ScreenToWorldPoint(Input.GetTouch(1).position);
                vecCenterStart = (vecZeroStart + vecOneStart) / 2;
            }
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            
            Zoom(difference * zoomOutSensitivity);
            CameraTarget = Vector3.MoveTowards(CameraTarget, vecCenterStart, difference / 3);
        }
    #endif

        if (transform.position != cameraTarget)
        {
            float _x = Mathf.SmoothStep(transform.position.x, cameraTarget.x, Time.time / cameraSmoothing);
            float _y = Mathf.SmoothStep(transform.position.y, cameraTarget.y, Time.time / cameraSmoothing);
            transform.position = new Vector3(_x, _y, transform.position.z);
        }

    }
    
    void Zoom(float increment)
    {
        float zoomPrev = zoomCurrent;
        zoomCurrent = Mathf.Clamp(zoomCurrent - increment, zoomOutMin, zoomOutMax);
        cameraObj.orthographicSize = zoomCurrent;

        float ratio = zoomCurrent / zoomOutDefault;
        float diff = zoomCurrent - zoomPrev;
        cameraSpeedX = cameraSpeedDefaultX * Mathf.Sqrt(ratio);
        cameraSpeedY = cameraSpeedDefaultY * Mathf.Sqrt(ratio);
        cameraBound.xMin += diff * cameraObj.aspect;
        cameraBound.xMax -= diff * cameraObj.aspect;
        cameraBound.yMin += diff;
        cameraBound.yMax -= diff;
    }
}
