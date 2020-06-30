using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundSegment
{
    float Length();
    float Width();
}

public class SegmentsGenerator : MonoBehaviour, ISegmentGenerator
{
    public Vector3 spawnPoint;

    private List<GameObject>    segmentsList;
    private GameObject          segmentSample;

    [SerializeField]
    private Vector3 velocity;
    private float distantTravel;
    private float segmentLength = 1f;

    private int lastSegment;
    private int firstSegment;

    public Vector3 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            // resetup for all segments
            for (int i = 0; i < segmentsList.Count; i++)
            {
                var segScript = segmentsList[i].GetComponent<GroundSegmentScript>();
                segScript.velocity = velocity;
            }
        }
    }

    List<GameObject> ISegmentGenerator.Get()
    {
        return segmentsList;
    }

    public int GetFirstSegment()
    {
        return firstSegment;
    }

    public int GetLastSegment()
    {
        return lastSegment;
    }

    // segSample must implements IGroundSegment
    public void Register(GameObject segSample, Vector3 segSpawnLocation, Vector3 segVelocity, float segDistantTravel)
    {
        segmentSample = segSample;
        spawnPoint = segSpawnLocation;
        velocity = segVelocity;
        distantTravel = segDistantTravel;

        Setup();

        // Keep track of the first/last segment being checked to loop
        lastSegment = segmentsList.Count - 1;
        firstSegment = 0;

        StartCoroutine("LoopCoroutine");
    }


    private void Setup()
    {
        segmentSample.SetActive(false);

        IGroundSegment iGroundSegment;
        if (segmentSample.TryGetComponent<IGroundSegment>(out iGroundSegment))
        {
            segmentLength = iGroundSegment.Length();
        }
        else
        {
            Debug.LogError("IGroundSegment component not found!");
        }

        segmentsList = new List<GameObject>();

        int numberOfSegments = (int)(distantTravel / segmentLength) + 1;

        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 pos = spawnPoint + velocity.normalized * segmentLength * i;
            GenerateSegment(pos);
        }

        Velocity = velocity;
    }

    private IEnumerator LoopCoroutine()
    {
        while (true)
        {
            var firstPos = segmentsList[firstSegment].transform.position;
            var lastPos = segmentsList[lastSegment].transform.position;

            // Projected to velocity direction
            var distFromFirst = Vector3.Dot(firstPos - spawnPoint, velocity) / velocity.magnitude;
            var distFromLast = Vector3.Dot(lastPos - spawnPoint, velocity) / velocity.magnitude;

            if (distFromFirst > distFromLast)
                yield return new WaitForSeconds(distFromFirst / velocity.magnitude);
            else
            {
                int numOfLoops = (int)Math.Ceiling((double)distFromFirst / segmentLength);
                for (int i = 0; i < numOfLoops; i++)
                {
                    var ds = distFromFirst - segmentLength;

                    segmentsList[lastSegment].transform.position = spawnPoint + velocity * ds / velocity.magnitude;

                    firstSegment = lastSegment;
                    lastSegment--;
                    if (lastSegment < 0)
                        lastSegment = segmentsList.Count - 1;
                }

                yield return null;
            }
        }
    }

    private void GenerateSegment(Vector3? pos = null)
    {
        var newSegment = Instantiate(segmentSample);
        newSegment.transform.position = (Vector3)(pos == null ? Vector3.zero : pos);
        newSegment.SetActive(true);
        segmentsList.Add(newSegment);
    }
}
