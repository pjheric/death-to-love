using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Author: Thomas Wang
public class CinemachineController : MonoBehaviour {
    // Fields for checking whether the camera has reached a "battle scene" by using waypoints set by the Cinemachine dolly
    [SerializeField] private CinemachineVirtualCamera cm;
    [SerializeField] private Transform target;
    [SerializeField] private CinemachineSmoothPath track;
    private bool snapped = false;
    private int counter = 1;

    // Fields for setting the left/right edges to the camera 
    private float frustumWidth;
    private float frustumHeight;
    private float offset = 0.5f;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;
    [SerializeField] private GameObject bottomEdge;
    [SerializeField] private GameObject topEdge;



    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    [SerializeField] private float intensity = 1f;
    [SerializeField] private float length = 1f;
    [SerializeField] private float SleepTime = 20f;

    private void Start() {
        //frustumWidth = cm.m_Lens.OrthographicSize * cm.m_Lens.Aspect;
        length *= 0.01f; //converts length and sleeptime from seconds to milliseconds
        SleepTime *= 0.01f;
    }

    // Update is called once per frame
    void Update() {
        frustumWidth = cm.m_Lens.OrthographicSize * cm.m_Lens.Aspect;
        frustumHeight = cm.m_Lens.OrthographicSize;
        // this is to update the borders to fit the size of the screen
        // frustumWidth is used in case the resolution changes and the screen doesn't scale

        leftEdge.transform.position = new Vector2(cm.gameObject.transform.position.x - (frustumWidth + offset), leftEdge.transform.position.y);
        rightEdge.transform.position = new Vector2(cm.gameObject.transform.position.x + (frustumWidth + offset), rightEdge.transform.position.y);
        bottomEdge.transform.position = new Vector2(cm.gameObject.transform.position.x, cm.gameObject.transform.position.y - (frustumHeight + offset));
        topEdge.transform.position = new Vector2(cm.gameObject.transform.position.x, cm.gameObject.transform.position.y + (frustumHeight - offset));

        // check if the camera has reached/gotten near a waypoint on the dolly and IS NOT SNAPPED
        // if so, set the camera to the waypoint,
        // then make some changes to ensure this condition doesn't keep getting checked and increment the counter
        if ((Mathf.Abs(track.m_Waypoints[counter].position.x - cm.gameObject.transform.position.x) <= 0.05f) && !snapped) {
            Debug.Log("camera hit waypoint");
            cm.gameObject.transform.position = new Vector2(track.m_Waypoints[counter].position.x, cm.gameObject.transform.position.y);
            Snap();
            counter++;
            StartCoroutine(Test());
        }

        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin Noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Noise.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1-shakeTimer/shakeTimerTotal));
        }
    }

    // placeholder coroutine function to simulate the "battle scenes" where the camera is lockec in place
    IEnumerator Test() {
        yield return new WaitForSeconds(5.0f);
        Unsnap();
    }
    public void Snap() {
        Debug.Log(cm.Follow);
        cm.Follow= null;
        Debug.Log(cm.Follow);
        //cm.enabled = false;
        snapped = true;
    }

    public void Unsnap() {
        Debug.Log("Unsnapped");
        cm.Follow = target;
        //cm.enabled = true;
        snapped = false;
    }

    public void shake()
    {
        CinemachineBasicMultiChannelPerlin Noise = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Noise.m_AmplitudeGain = intensity;
        shakeTimer = length;
        shakeTimerTotal = length;
        startingIntensity = intensity;
        StartCoroutine(Sleep());
    }

    private IEnumerator Sleep()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(Mathf.Abs(SleepTime));
        Time.timeScale = 1;
    }

    public void lockCam()
    {
        cm.enabled = false; //this locks the camera during combat, but the problem is that it disables screenshake as well
    }

    public void unlockCam()
    {
        cm.enabled = true;
    }

}