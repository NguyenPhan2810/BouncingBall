using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public interface ISegmentGenerator
{
    void Register(GameObject segSample, Vector3 segSpawnLocation, Vector3 segVelocity, float segDistantTravel);
    
    List<GameObject> Get();

    int GetFirstSegment(); // Segment closest to spawning point
    int GetLastSegment(); // Segment farthest to spawning point
}

public interface IObstacleGenerator
{
    // maxObs is the max number of obstacles to be seen at once
    void Register(GameObject obsSample, GameObject endPoint, int maxObs);

    GameObject Get();
}

public class GroundManager : MonoBehaviour
{
    public float obstaclePeriod; // Average period between two obstacle being generated
    public float obstacleOffset; // Offset from the ground
    
    private ISegmentGenerator   segmentGenerator;
    private IObstacleGenerator  obstacleGenerator;
    private List<GameObject>    segmentList;

    public void Construct(ISegmentGenerator segGenerator, IObstacleGenerator obsGenerator)
    {
        segmentGenerator = segGenerator;
        segmentList = segmentGenerator.Get();
        obstacleGenerator = obsGenerator;
    }

    public void StartGenerateObstacle()
    {
        StartCoroutine("ObstacleGenerationCoroutine");
    }

    private IEnumerator ObstacleGenerationCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(obstaclePeriod * Random.Range(0.5f, 2f));

            var obs = obstacleGenerator.Get();
            var groundTransform = segmentList[segmentGenerator.GetFirstSegment()].transform;
            var groundPos = groundTransform.position;

            obs.transform.parent = groundTransform;

            Vector3 newPos = new Vector3(0, 0, groundPos.z);
            obs.transform.localPosition = newPos;

            var obsPos = obs.transform.position;
            obs.transform.position = new Vector3(obsPos.x, obsPos.y + obstacleOffset, obsPos.z);
        }
    }
}
