using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    // Text m_debugTip;
    public bool canRotation_X = true;
    public bool canRotation_Y = true;
    public bool canScale = true;
    public bool Win;
    #region Field and Property
    // 旋转中心
    public Transform target;
    private Touch[] Touchs;
    private int TouchCount;
    // 鼠标
    public MouseSettings mouseSettings = new MouseSettings(1, 10, 10);

    // 角度限制
    public Range angleRange = new Range(15, 70);

    // 距离限制
    public Range distanceRange = new Range(3, 7);

    // 移动和旋转阻力
    [Range(0, 10)]
    public float damper = 2;

    public Vector2 CurrentAngles { protected set; get; }
    public float CurrentDistance { protected set; get; }

    protected Vector2 targetAngles;

    protected float targetDistance;
    #endregion

    #region Protected Method
    protected virtual void Start()
    {
        Touchs = new Touch[2];
        CurrentAngles = targetAngles = transform.eulerAngles;
        // CurrentDistance = targetDistance = Vector3.Distance(transform.position, target.position);
        targetDistance = distanceRange.max;
        MessageManager.Instance.MainCharShowWin += IsWin;
        MessageManager.Instance.ChangeCamera += CameraChange;
    }
    bool DistanceTrigger=true;
    private void CameraChange()
    {
        if (DistanceTrigger)
        {
            targetDistance = distanceRange.min;
        }
        else
        {
            targetDistance = distanceRange.max;
        }
        DistanceTrigger = !DistanceTrigger;
    }
    private void IsWin()
    {
        Win = true;
    }

    public void Update()
    {

    }
    protected virtual void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
#if UNITY_EDITOR
        AroundByMouseInput();
#elif UNITY_ANDROID || UNITY_IPHONE
                AroundByMobileInput();
#endif

    }

    private void CameraCollisionUpdate()
    {

    }

    /*private void OnDrawGizmos()
    {
        if (transform==null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);
    }*/

    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势  
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

    private bool m_IsSingleFinger;
    /*
    private void ScaleCamera()
    {
        //计算出当前两点触摸点的位置  
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;
        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);
        //计算上次和这次双指触摸之间的距离差距  
        //然后去更改摄像机的距离  
        distance -= ( currentTouchDistance - lastTouchDistance ) * scaleFactor * Time.deltaTime;
        //把距离限制住在min和max之间  
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        //备份上一次触摸点的位置，用于对比  
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }
    */
    //private bool 
    private void GetTouchOp()
    {
        TouchCount = 0;
        int touchNum = Input.touchCount;
        if (touchNum == 0)
        {
            return;
        }
        //获取一个非UI触点
        if (touchNum > 0)
        {
            for (int i = 0; i < touchNum; i++)
            {
                Touch touch = Input.GetTouch(i);
                float TouchX = touch.position.x;

#if UNITY_ANDROID || UNITY_IPHONE
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
#endif
                {
                    if (TouchX > Screen.width / 3f)
                    {
                        TouchCount++;
                        if (TouchCount > 2)
                        {
                            break;
                        }
                        Touchs[TouchCount - 1] = touch;
                    }
                }
            }
        }
    }
    protected void AroundByMobileInput()
    {
        GetTouchOp();

        if (TouchCount == 1)
        {
            if (!Win&&!UIManager.Instance.IsTouchUI)
            {
                if (Touchs[0].phase == TouchPhase.Moved)
                {
                    targetAngles.y += Touchs[0] .deltaPosition.x/3f;
                    targetAngles.x -= Touchs[0].deltaPosition.y/3f;

                    //Range.
                    targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
                }
                //Mouse pointer.
                m_IsSingleFinger = true;
            }
        }
        
        //Mouse scrollwheel.
        if (canScale)
        {
            if (TouchCount > 1)
            {
                //计算出当前两点触摸点的位置  
                if (m_IsSingleFinger)
                {
                    oldPosition1 = Touchs[0].position;
                    oldPosition2 = Touchs[1].position;
                }

                if (Touchs[0].phase == TouchPhase.Moved && Touchs[1].phase == TouchPhase.Moved)
                {
                    var tempPosition1 = Touchs[0].position;
                    var tempPosition2 = Touchs[1].position;
                    float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
                    float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

                    //计算上次和这次双指触摸之间的距离差距  
                    //然后去更改摄像机的距离  
                    targetDistance -= (currentTouchDistance - lastTouchDistance) * Time.deltaTime * mouseSettings.wheelSensitivity;
                    //  m_debugTip.text = ( currentTouchDistance - lastTouchDistance ).ToString() + " + " + targetDistance.ToString();


                    //把距离限制住在min和max之间
                    //备份上一次触摸点的位置，用于对比  
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                    m_IsSingleFinger = false;
                }
            }
        }


        targetDistance = Mathf.Clamp(targetDistance, distanceRange.min, distanceRange.max);

        //Lerp.
        CurrentAngles = Vector2.Lerp(CurrentAngles, targetAngles, damper * Time.deltaTime);
        CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);


        ///撞墙处理
        Ray m_ray = new Ray(target.position, -transform.forward);
        RaycastHit m_hit;
        LayerMask layerMask = 1 << 10;
        if (Physics.Raycast(m_ray, out m_hit, targetDistance, layerMask))
        {

            float colDis = (target.position - m_hit.point).magnitude - 0.1f;
            CurrentDistance = Mathf.Lerp(CurrentDistance, colDis, 2 * damper * Time.deltaTime);
        }
        else
        {

            //Lerp.

            CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);

        }

        if (!canRotation_X) targetAngles.y = 0;
        if (!canRotation_Y) targetAngles.x = 0;


        //Update transform position and rotation.
        transform.rotation = Quaternion.Euler(CurrentAngles);

        transform.position = target.position - transform.forward * CurrentDistance;
        // transform.position = target.position - Vector3.forward * CurrentDistance;

    }

    /// <summary>
    /// Camera around target by mouse input.
    /// </summary>
    protected void AroundByMouseInput()
    {

        if (Input.GetMouseButton(mouseSettings.mouseButtonID))
        {
            if (!Win)
            {
                //Mouse pointer.
                targetAngles.y += Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity;
                targetAngles.x -= Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;
                //Range.
                targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
            }
        }

        //Mouse scrollwheel.
        if (canScale)
        {
            targetDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseSettings.wheelSensitivity;
        }
        // m_debugTip.text = Input.GetAxis("Mouse ScrollWheel").ToString() + " + " + targetDistance.ToString();
        targetDistance = Mathf.Clamp(targetDistance, distanceRange.min, distanceRange.max);
        CurrentAngles = Vector2.Lerp(CurrentAngles, targetAngles, damper * Time.deltaTime);

        ///撞墙处理
        Ray m_ray = new Ray(target.position, -transform.forward);
        RaycastHit m_hit;
        LayerMask layerMask = 1 << 10;
        if (Physics.Raycast(m_ray, out m_hit, targetDistance, layerMask))
        {

            float colDis = (target.position - m_hit.point).magnitude - 0.1f;
            CurrentDistance = Mathf.Lerp(CurrentDistance, colDis, 2 * damper * Time.deltaTime);
        }
        else
        {

            //Lerp.

            CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);

        }

        if (!canRotation_X) targetAngles.y = 0;
        if (!canRotation_Y) targetAngles.x = 0;


        //Update transform position and rotation.
        transform.rotation = Quaternion.Euler(CurrentAngles);

        Vector3 m_rayDirection = transform.position - target.position;

        m_rayDirection.Normalize();


        transform.position = target.position - transform.forward * CurrentDistance;
        // transform.position = target.position - Vector3.forward * CurrentDistance;


    }
    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("xaing");
    }*/
    #endregion
}

[Serializable]
public struct MouseSettings
{
    /// <summary>
    /// ID of mouse button.
    /// </summary>
    public int mouseButtonID;

    /// <summary>
    /// Sensitivity of mouse pointer.
    /// </summary>
    public float pointerSensitivity;

    /// <summary>
    /// Sensitivity of mouse ScrollWheel.
    /// </summary>
    public float wheelSensitivity;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mouseButtonID">ID of mouse button.</param>
    /// <param name="pointerSensitivity">Sensitivity of mouse pointer.</param>
    /// <param name="wheelSensitivity">Sensitivity of mouse ScrollWheel.</param>
    public MouseSettings(int mouseButtonID, float pointerSensitivity, float wheelSensitivity)
    {
        this.mouseButtonID = mouseButtonID;
        this.pointerSensitivity = pointerSensitivity;
        this.wheelSensitivity = wheelSensitivity;
    }

}

/// <summary>
/// Range form min to max.
/// </summary>
[Serializable]
public struct Range
{
    /// <summary>
    /// Min value of range.
    /// </summary>
    public float min;

    /// <summary>
    /// Max value of range.
    /// </summary>
    public float max;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="min">Min value of range.</param>
    /// <param name="max">Max value of range.</param>
    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

/// <summary>
/// Rectangle area on plane.
/// </summary>
[Serializable]
public struct PlaneArea
{
    /// <summary>
    /// Center of area.
    /// </summary>
    public Transform center;

    /// <summary>
    /// Width of area.
    /// </summary>
    public float width;

    /// <summary>
    /// Length of area.
    /// </summary>
    public float length;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="center">Center of area.</param>
    /// <param name="width">Width of area.</param>
    /// <param name="length">Length of area.</param>
    public PlaneArea(Transform center, float width, float length)
    {
        this.center = center;
        this.width = width;
        this.length = length;
    }
}

/// <summary>
/// Target of camera align.
/// </summary>
[Serializable]
public struct AlignTarget
{
    /// <summary>
    /// Center of align target.
    /// </summary>
    public Transform center;

    /// <summary>
    /// Angles of align.
    /// </summary>
    public Vector2 angles;

    /// <summary>
    /// Distance from camera to target center.
    /// </summary>
    public float distance;

    /// <summary>
    /// Range limit of angle.
    /// </summary>
    public Range angleRange;

    /// <summary>
    /// Range limit of distance.
    /// </summary>
    public Range distanceRange;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="center">Center of align target.</param>
    /// <param name="angles">Angles of align.</param>
    /// <param name="distance">Distance from camera to target center.</param>
    /// <param name="angleRange">Range limit of angle.</param>
    /// <param name="distanceRange">Range limit of distance.</param>
    public AlignTarget(Transform center, Vector2 angles, float distance, Range angleRange, Range distanceRange)
    {
        this.center = center;
        this.angles = angles;
        this.distance = distance;
        this.angleRange = angleRange;
        this.distanceRange = distanceRange;
    }

}