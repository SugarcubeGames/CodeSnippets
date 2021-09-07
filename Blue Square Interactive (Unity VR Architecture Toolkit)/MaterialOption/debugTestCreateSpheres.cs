using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugTestCreateSpheres : MonoBehaviour {

    [Range(2,100)]
    public int numPoints = 5;
    private int oldNumPoints;

    public float radius = 0.25f;
    private float oldRadius;

    private List<GameObject> spheres;

    private void Start()
    {
        oldNumPoints = numPoints;
        oldRadius = radius;
        spheres = new List<GameObject>();

        generateSpheres();
    }

    private void Update()
    {
        if (numPoints != oldNumPoints || radius != oldRadius)
        {
            foreach (GameObject o in spheres)
            {
                Destroy(o);
            }

            spheres.Clear();

            oldNumPoints = numPoints;
            oldRadius = radius;

            generateSpheres();
        }
    }

    private void generateSpheres()
    {
        radius = 0.0375f + 0.0375f + (.011f * (float)numPoints);

        float angle = 2 * Mathf.PI / numPoints;

        for(int i = 0; i<numPoints; i++)
        {
            float x = radius * Mathf.Sin(i * angle);
            float y = radius * Mathf.Cos(i * angle);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Sphere " + i; 
            go.transform.parent = this.gameObject.transform;
            go.transform.localPosition = new Vector3(x, y, 0);
            go.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
            go.GetComponent<SphereCollider>().isTrigger = true;
            //Must add a rigid body to the sphere to make OnTriggerEntered work now...
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;  //isKinematic means physics won't affect it

            go.AddComponent<debugInteractWithTestSphere>();
  
            spheres.Add(go);
        }

        GameObject go2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go2.name = "Close";
        go2.transform.parent = this.gameObject.transform;
        go2.transform.localPosition = new Vector3(0, 0, 0);
        go2.transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);

        spheres.Add(go2);

    }
}
