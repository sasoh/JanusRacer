using UnityEngine;
using System.Collections;

public class LerpDemo : MonoBehaviour {
    float lerpTime = 1f;
    float currentLerpTime;

    float moveDistance = 10f;

    Vector3 startPos;
    Vector3 endPos;

    protected void Start()
    {
        startPos = transform.position;
        endPos = transform.position + transform.forward * moveDistance;
    }

    protected void Update()
    {
        //reset when we press spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }

        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);
    }
}
