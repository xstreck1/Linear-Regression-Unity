using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;


public class PointGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int pointCount;
    [SerializeField] private int max;
    [SerializeField] private float a;
    [SerializeField] private float b;
    [SerializeField] private float noise;

    public List<GameObject> Points { get; private set; }
    
    private void Start()
    {
        Points = new List<GameObject>();
    }

    public void GeneratePoints()
    {
        for (int i = 0; i < pointCount; i++)
        {
            float x = Random.Range(-max, max);
            float offset = SampleNormal(0f, noise);
            float y = x * a + b + offset;
            var point = Instantiate(pointPrefab, transform);
            point.transform.localPosition = new Vector3(x, y, 0f);
            Points.Add(point);
        }
    }

    private static float SampleNormal(float mean, float stddev)
    {
        float x1 = 1f - Random.Range(0f, 1f);
        float x2 = 1f - Random.Range(0f, 1f);
        float y1 = Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2);
        return y1 * stddev + mean;
    }

    public void ClearPoints()
    {
        Points.ForEach(Destroy);
        Points.Clear();
    }
}
