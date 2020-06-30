using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleObstacleGenerator : MonoBehaviour, IObstacleGenerator
{
    private GameObject          triangleSample;
    private GameObject          endPoint;
    private int                 maxTriangles;

    private List<GameObject>    triangleList;
    private Stack<GameObject>   inativeTriangles;

    public GameObject Get()
    {
        var top = inativeTriangles.Pop();
        top.SetActive(true);
        return top;
    }

    public void Register(GameObject obsSample, GameObject endPoint, int maxObs)
    {
        triangleSample = obsSample;
        this.endPoint = endPoint;
        maxTriangles = maxObs;

        inativeTriangles = new Stack<GameObject>();
        triangleList = new List<GameObject>(maxTriangles);

        triangleSample.SetActive(false);
        Setup();
    }

    private void Setup()
    {
        for(int i = 0; i < maxTriangles; i++)
        {
            var newTriangle = Instantiate(triangleSample);
            newTriangle.GetComponent<TriangleObstacleScript>().collideBoundary += CollideBoundaryEvent;
            newTriangle.SetActive(false);
            triangleList.Add(newTriangle);
            inativeTriangles.Push(newTriangle);
        }

    }

    private void CollideBoundaryEvent(GameObject triangle)
    {
        triangle.SetActive(false);
        inativeTriangles.Push(triangle);
    }
}
