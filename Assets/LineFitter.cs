using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineFitter : MonoBehaviour
{
    [SerializeField] private PointGenerator pointGenerator;
    [SerializeField] private double learningRate;
    [SerializeField] private double normalize;
    [SerializeField] private double epochs;
    
    public void FitLine()
    {
        if (!pointGenerator.Points.Any())
            return;
        StopAllCoroutines();
        StartCoroutine(FitLineCoR());
    }

    private IEnumerator FitLineCoR()
    {
        double a = 0;
        double b = 0;
        double N = pointGenerator.Points.Count;
        var X = pointGenerator.Points.Select(p => p.transform.localPosition.x);
        var Y = pointGenerator.Points.Select(p => p.transform.localPosition.y);
        for (int i = 0; i < epochs; i++)
        {
            var Y_pred = X.Select(x => x * a + b);
            double D_a = (-2.0/N) * Y
                .Zip(Y_pred, (y, y_pred) => y - y_pred)
                .Zip(X, (y_p, x) => y_p * x )
                .Sum() ;
            double D_b = (-2.0/N) * Y
                .Zip(Y_pred, (y, y_pred) => y - y_pred)
                .Sum();
            a -= learningRate * D_a;
            b -= learningRate * D_b * normalize;
            Debug.Log($"a: {a}, b: {b}");
            SetLine(a, b);
            yield return null;
        }
    }

    private void SetLine(double a, double b)
    {
        double rad = Math.Atan(a);
        double deg = rad * 180 / Math.PI;
        transform.localPosition = new Vector3(0, (float) b, 0);
        transform.localRotation = Quaternion.Euler(0, 0, (float) deg);
    }
}
