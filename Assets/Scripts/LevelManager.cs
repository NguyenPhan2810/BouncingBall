using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject    groundSegmentSample;
    public GameObject    triangleObstacleSample;
    public GameObject    obstacleBoundary;
    public GameObject    player;
    
    private GroundManager             groundManager;
    private TriangleObstacleGenerator triObstacleGenerator;
    private SegmentsGenerator         segGenerator;

    // Start is called before the first frame update
    private void Start()
    {
        segGenerator = gameObject.GetComponent<SegmentsGenerator>();
        segGenerator.Register(groundSegmentSample, new Vector3(1300, -350), new Vector3(-2000, 0), 3000);

        triObstacleGenerator = gameObject.GetComponent<TriangleObstacleGenerator>();
        triObstacleGenerator.Register(triangleObstacleSample, obstacleBoundary, 30);

        groundManager = gameObject.GetComponent<GroundManager>();
        groundManager.Construct(segGenerator, triObstacleGenerator);
        groundManager.obstaclePeriod = 1f;
        groundManager.obstacleOffset = 
            groundSegmentSample.GetComponent<SpriteRenderer>().bounds.extents.y + 
            triangleObstacleSample.GetComponent<SpriteRenderer>().bounds.extents.y;

        groundManager.StartGenerateObstacle();

        StartCoroutine("groundPumpCoroutine");
    }

    private void Update()
    {
        var dv = Input.GetAxis("Horizontal");
        if (dv != 0)
        {
            segGenerator.Velocity = segGenerator.Velocity + new Vector3(1000 * Time.deltaTime * dv, 0, 0);
            //groundManager.obstaclePeriod *= 1 + dv * Time.deltaTime;
        }
    }

    private IEnumerator groundPumpCoroutine()
    {
        var width = groundSegmentSample.GetComponent<IGroundSegment>().Width();
        while (true)
        {
            yield return new WaitForSeconds(Random.value * 3);
            segGenerator.spawnPoint.y += width;

            yield return new WaitForSeconds(Random.value * 3);
            segGenerator.spawnPoint.y -= width;
        }
    }
}
