using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ArrayList = System.Collections.ArrayList;

public class SpellCasting : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject particlePlane;
    public Wand wand;
    public float maxDistance = 40.0f;
    public float spawnDelayMilliseconds = 50;

    private List<Vector2> points;
    private float lastSpawnTime;
    private GameObject activePlane;
    private Vector3? centerpoint;
    private Transform wandTip;
    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
        points = new List<Vector2>();
        wandTip = transform.GetChild(0).GetChild(0);
        wand = transform.GetChild(0).GetComponent<Wand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.1f) // Detect right trigger press
        {
            Debug.Log("Trigger down");
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = new Ray(transform.position, wandTip.forward);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, maxDistance) && !activePlane)
            {
                activePlane = Instantiate(particlePlane);
                activePlane.transform.position = ray.origin + ray.direction * maxDistance;
                activePlane.transform.forward = ray.direction;
                return;
            }
            if (Physics.Raycast(ray, out hit) && Time.time - lastSpawnTime >= spawnDelayMilliseconds / 1000)
            {
                //spawn the prefab at the end of the ray
                if (hit.transform.gameObject == activePlane)
                {
                    Vector3 spawnPosition = hit.point;
                    GameObject myParticle = Instantiate(particlePrefab, spawnPosition, Quaternion.identity, activePlane.transform);
                    Destroy(myParticle, 30.0f);
                    Vector2 point = ProjectPointToPlane(hit.point, hit.normal, activePlane.transform.up, activePlane.transform.right, centerpoint);
                    if (!centerpoint.HasValue)
                    {
                        centerpoint = point;
                        points.Add(new Vector2(0, 0)); //gotta manually add it cause we don't know offset yet
                    }
                    else
                    {
                        points.Add(point);
                    }
                }
            }
        }
        else if (activePlane)
        {
            Destroy(activePlane);
            centerpoint = null;
            doSpells(points);
            points.Clear();
        }
    }

    Vector2 ProjectPointToPlane(Vector3 point, Vector3 normal, Vector3 up, Vector3 right, Vector3? center)
    {
        // Create basis vectors
        Vector3 tangent = right;
        Vector3 bitangent = up;

        // Project onto the 2D plane
        float u = Vector3.Dot(point, tangent);
        float v = Vector3.Dot(point, bitangent);

        if (center.HasValue)
        {
            u -= center.Value.x;
            v -= center.Value.y;
        }

        return new Vector2(u, v);
    }

    void doSpells(List<Vector2> pointList)
    {
        //for now, just a check whether the line is north, south, east, or west
        if (pointList.Count < 2)
        {
            return;
        }

        int n = points.Count;
        float sumX = 0f, sumY = 0f, sumXY = 0f, sumX2 = 0f;
        foreach (Vector2 p in pointList)
        {
            sumX += p.x;
            sumY += p.y;
            sumXY += p.x * p.y;
            sumX2 += p.x * p.x;
        }

        float angleTotal = Mathf.Atan((n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX)) * Mathf.Rad2Deg;
        float angleFirstLast = Mathf.Atan((pointList.Last().y - pointList[0].y) / (pointList.Last().x - pointList[0].x)) * Mathf.Rad2Deg;
        if (pointList.Last().x - pointList[0].x < 0)
        {
            angleTotal = (angleTotal + 180) % 360; //TODO: this doesn't work right - debug it.
            angleFirstLast = (angleFirstLast + 180) % 360;
        }
        if (angleTotal < 0)
        {
            angleTotal += 360;
        }
        if (angleFirstLast < 0)
        {
            angleFirstLast += 360;
        }

        print($"Best-fit slope is {angleTotal} and first-to-last slope is {angleFirstLast}");
        float degreeLeeway = 7.5f;
        if (
            (angleTotal < degreeLeeway || angleTotal > 360 - degreeLeeway)
            && (angleFirstLast < degreeLeeway || angleFirstLast > 360 - degreeLeeway)
            )
        {
            //0 degrees +- degreeLeeway
            print("Found a right-facing line");
            wand.Cast(0);
        }
        else if (angleTotal < 180 + degreeLeeway && angleTotal > 180 - degreeLeeway && angleFirstLast < 180 + degreeLeeway &&
            angleFirstLast > 180 - degreeLeeway)
        {
            //180 degrees +- degreeLeeway
            print("Found a left-facing line");
        }
        else if (angleTotal < 90 + degreeLeeway && angleTotal > 90 - degreeLeeway && angleFirstLast < 90 + degreeLeeway &&
                 angleFirstLast > 90 - degreeLeeway)
        {
            //90 degrees +- degreeLeeway
            print("Found an upwards-facing line");
        }
        else if (angleTotal < 270 + degreeLeeway && angleTotal > 270 - degreeLeeway && angleFirstLast < 270 + degreeLeeway &&
                 angleFirstLast > 270 - degreeLeeway)
        {
            //270 degrees +- degreeLeeway
            print("Found a downwards-facing line");
        }
        else
        {
            print("No valid shape found.");
        }

    }
}