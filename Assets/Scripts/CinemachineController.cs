using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Author: Thomas Wang
public class CinemachineController : MonoBehaviour {
    // Fields for checking whether the camera has reached a "battle scene" by using waypoints set by the Cinemachine dolly
    [SerializeField] private CinemachineVirtualCamera cm;
    [SerializeField] private CinemachineSmoothPath track;
    private bool snapped = false;
    private int counter = 1;

    // Fields for setting the left/right edges to the camera 
    private float frustumWidth;
    private float offset = 0.5f;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;

    private void Start() {
        //frustumWidth = cm.m_Lens.OrthographicSize * cm.m_Lens.Aspect;
    }

    // Update is called once per frame
    void Update() {
        frustumWidth = cm.m_Lens.OrthographicSize * cm.m_Lens.Aspect;
        // this is to update the borders to fit the size of the screen
        // frustumWidth is used in case the resolution changes and the screen doesn't scale
        leftEdge.transform.position = new Vector2(cm.gameObject.transform.position.x - (frustumWidth + offset), leftEdge.transform.position.y);
        rightEdge.transform.position = new Vector2(cm.gameObject.transform.position.x + (frustumWidth + offset), rightEdge.transform.position.y);

        // check if the camera has reached/gotten near a waypoint on the dolly and IS NOT SNAPPED
        // if so, set the camera to the waypoint,
        // then make some changes to ensure this condition doesn't keep getting checked and increment the counter
        if ((Mathf.Abs(track.m_Waypoints[counter].position.x - cm.gameObject.transform.position.x) <= 0.05f) && !snapped) {
            cm.gameObject.transform.position = new Vector2(track.m_Waypoints[counter].position.x, cm.gameObject.transform.position.y);
            Snap();
            counter++;
            StartCoroutine(Test());
            Unsnap();
        }
    }

    // placeholder coroutine function to simulate the "battle scenes" where the camera is lockec in place
    IEnumerator Test() {
        yield return new WaitForSeconds(5.0f);
    }
    void Snap() {
        cm.enabled = false;
        snapped = true;
    }
    void Unsnap() {

        cm.enabled = true;
        snapped = false;

    }



}