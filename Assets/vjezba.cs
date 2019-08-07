using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class vjezba : MonoBehaviour
{
    public Transform parentT;
    public Transform pointPrefab;
    public int size;
    Transform[] points;
    [Range(2f,30f)]
    public float wavelength;
    [Range(0.1f, 10f)]
    public float timelength;


    // Start is called before the first frame update
    void Start()
    {
        float step = 2f / size;
        Vector3 position;
        Vector3 scale = Vector3.one * step;
        position.y = 0f;
        position.z = 0f;
        points = new Transform[size * size];
        for(int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(parentT, false);
            points[i] = point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        float t = Time.time * timelength;
        GraphFunction f = Ripple;
        float step = 2f / size;
        for(int i = 0, z = 0; z < size; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for(int x = 0; x < size; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }
        //Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    const float pi = Mathf.PI;

    static float SineFunction(float x, float z, float t)
    {
        return Mathf.Sin(pi * (x + t));
    }

    static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        y *= 2f / 3f;
        return y;
    }
    static float Side2DFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }
    Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= wavelength + 10f * d;
        p.z = z;
        return p;
    }
    
}
