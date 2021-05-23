using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject backgroundSprite;
    public Camera cameraObj;
    
    private Rect cameraBoundDefault;
    private Rect cameraBound;
    private Vector3 cameraTarget;

    private Vector3 CameraTarget
    {
        get => cameraTarget;
        set =>
            cameraTarget = new Vector3(Mathf.Clamp(value.x, cameraBound.xMin, cameraBound.xMax),
                Mathf.Clamp(value.y, cameraBound.yMin, cameraBound.yMax),
                transform.position.z);
    }
    public float cameraSpeedDefaultX;
    public float cameraSpeedDefaultY;
    private float cameraSpeedX;
    private float cameraSpeedY;
    public float cameraSmoothing = 5;
    public float cameraAspectRatio;
    
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

        zoomOutDefault = cameraObj.orthographicSize;
        cameraAspectRatio = cameraObj.aspect;
        
        zoomCurrent = zoomOutDefault;
        
        var backgroundBounds = backgroundSprite.GetComponent<SpriteRenderer>().bounds;
        cameraBoundDefault.xMin = backgroundBounds.min.x + zoomOutDefault * cameraAspectRatio;
        cameraBoundDefault.xMax = backgroundBounds.max.x - zoomOutDefault * cameraAspectRatio;
        cameraBoundDefault.yMin = backgroundBounds.min.y + zoomOutDefault;
        cameraBoundDefault.yMax = backgroundBounds.max.y - zoomOutDefault;
        Debug.Log(cameraBoundDefault.xMin + " " + cameraBoundDefault.xMax + " " + cameraBoundDefault.yMin + " " + cameraBoundDefault.yMax);
        Debug.Log(backgroundBounds.min.x + " " +
                  backgroundBounds.max.x + " " +
                  backgroundBounds.min.y + " " +
                  backgroundBounds.max.y);

        cameraBound = cameraBoundDefault;
        zoomOutMax = Mathf.Min((backgroundBounds.max.x - backgroundBounds.min.x) / cameraAspectRatio / 2,
            (backgroundBounds.max.y - backgroundBounds.min.y) / 2,
            zoomOutMax);

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
                var touchMove = cameraObj.ScreenToViewportPoint(Input.GetTouch(0).deltaPosition);
                var newX = CameraTarget.x - touchMove.x * Time.deltaTime * cameraSpeedX * 5f;
                var newY = CameraTarget.y - touchMove.y * Time.deltaTime * cameraSpeedY * 5f;
                CameraTarget = new Vector3(newX,newY, transform.position.z);
            }
            vecZeroStart = Input.GetTouch(0).position;
        }
        else if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                vecOneStart = Input.GetTouch(1).position;
                vecCenterStart = (vecZeroStart + vecOneStart) / 2;
            }
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = cameraObj.ScreenToViewportPoint(touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = cameraObj.ScreenToViewportPoint(touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            
            Zoom(difference * Time.deltaTime * zoomOutSensitivity, vecCenterStart);
            //CameraTarget = Vector3.MoveTowards(CameraTarget, vecCenterStart, difference / 3);
        }
    #else
        if (Input.GetMouseButton(0))
        {
            var pos = CameraTarget;
            var axisX = Input.GetAxis("Mouse X");
            pos.x -= axisX * Time.deltaTime * cameraSpeedX;

            var axisY = Input.GetAxis("Mouse Y");
            pos.y -= axisY * Time.deltaTime * cameraSpeedY;

            CameraTarget = pos;
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) 
        {
            Zoom(scroll * Time.deltaTime * zoomOutSensitivity, Input.mousePosition);
        }
#endif

        if (transform.position != cameraTarget)
        {
            float x = Mathf.SmoothStep(transform.position.x, cameraTarget.x, Time.time / cameraSmoothing);
            float y = Mathf.SmoothStep(transform.position.y, cameraTarget.y, Time.time / cameraSmoothing);
            transform.position = new Vector3(x, y, transform.position.z);
        }

    }
    
    void Zoom(float increment, Vector3 zoomCenter /*In screen coord*/)
    {
        var oldCenterWorld = cameraObj.ScreenToWorldPoint(zoomCenter);
        float zoomPrev = zoomCurrent;
        zoomCurrent = Mathf.Clamp(zoomCurrent - increment, zoomOutMin, zoomOutMax);
        cameraObj.orthographicSize = zoomCurrent;
        var newCenterWorld = cameraObj.ScreenToWorldPoint(zoomCenter);

        float ratio = zoomCurrent / zoomOutDefault;
        float diff = zoomCurrent - zoomPrev;
        cameraSpeedX = cameraSpeedDefaultX * Mathf.Sqrt(ratio);
        cameraSpeedY = cameraSpeedDefaultY * Mathf.Sqrt(ratio);
        cameraBound.xMin += diff * cameraAspectRatio;
        cameraBound.xMax -= diff * cameraAspectRatio;
        cameraBound.yMin += diff;
        cameraBound.yMax -= diff;

        var diffCenterWorld = newCenterWorld - oldCenterWorld;
        CameraTarget -= diffCenterWorld;
    }
}
