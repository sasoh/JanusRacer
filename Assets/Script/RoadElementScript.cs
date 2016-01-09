using UnityEngine;
using System.Collections;

public class RoadElementScript : MonoBehaviour
{
    public GameObject StraightPrefab;
    public GameObject CurvePrefab;
    public RoadElementScript Previous;
    public RoadElementScript Next;

    public void UpdateShape()
    {
        // calculate type by difference of prev & next's positions
        Difference(Previous.gameObject, Next.gameObject);
    }

    void Difference(GameObject prev, GameObject next)
    {
        Vector3 difference = next.transform.position - prev.transform.position;
        Vector3 forwardVector3 = transform.position - prev.transform.position;
        transform.forward = difference;

        float angle = Vector3.Angle(difference, forwardVector3);
        if (Mathf.Abs(angle) > 0.0f)
        {
            // curve
            GameObject shape = Instantiate(CurvePrefab);
            shape.transform.position = transform.position;
            shape.transform.parent = transform;

            Vector3 directionVector3 = next.transform.position - transform.position;
            shape.transform.forward = directionVector3;
        }
        else
        {
            // straight line
            GameObject shape = Instantiate(StraightPrefab);
            shape.transform.position = transform.position;
            shape.transform.parent = transform;
            shape.transform.rotation = transform.rotation;
        }
    }
}
