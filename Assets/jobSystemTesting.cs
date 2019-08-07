using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class jobSystemTesting : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> spawnPoints;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            ReallyToughTask(spawnPoints[i].transform.position);
        }

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    private JobHandle ReallyToughTask(Vector3 spwanPoint)
    {
        ReallyToughJob job = new ReallyToughJob();
        job.spawn = spwanPoint;
        return job.Schedule();
    }
}

[BurstCompile]
public struct ReallyToughJob : IJob{
    public Vector3 spawn;
    public void Execute()
    {
        for(int i = 0; i < 100; i++)
        {
            MonoBehaviour.Instantiate(new GameObject(), spawn, new Quaternion());
        }
    }
}
