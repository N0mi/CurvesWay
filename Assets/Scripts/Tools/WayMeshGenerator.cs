using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvesWay.Tools.Way
{
    
    public class WayMeshGenerator : MonoBehaviour
    {
        private const int stepsPerCurve = 100;

        public bool changeInRuntime;

        [SerializeField] private float RoadWidth = 2f;
        [SerializeField] private GameObject flagsPref;

        private WaySplineCreator way;
        private Mesh mesh;
        List<Vector3> points = new List<Vector3>();
        List<int> triangles = new List<int>();

        private void Start()
        {
            way = FindObjectOfType<WaySplineCreator>();
            mesh = new Mesh();
            mesh.name = "Way quad";

            GenerateVertices();   

            for (int i = 0; i < points.Count - 3; i += 2)
            {              

                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + 2);

                triangles.Add(i + 1);
                triangles.Add(i + 3);
                triangles.Add(i + 2);

            }

            mesh.SetVertices(points);
            mesh.SetTriangles(triangles,0);
            mesh.RecalculateNormals();

            Instantiate(flagsPref, way.GetPoint(way.EndProgress), Quaternion.LookRotation(way.GetDirection(way.EndProgress)));


            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        private void GenerateVertices()
        {
            points.Add(way.GetPoint(0) + Quaternion.LookRotation(way.GetDirection(0)) * Vector3.right * RoadWidth + Quaternion.LookRotation(way.GetDirection(0)) * Vector3.forward * -10f);
            points.Add(way.GetPoint(0) + Quaternion.LookRotation(way.GetDirection(0)) * Vector3.right * -RoadWidth + Quaternion.LookRotation(way.GetDirection(0)) * Vector3.forward * -10f);

            for (float i = 0; i <= 1; i += 1f / stepsPerCurve)
            {
                points.Add(way.GetPoint(i) + Quaternion.LookRotation(way.GetDirection(i)) * Vector3.right * RoadWidth);
                points.Add(way.GetPoint(i) + Quaternion.LookRotation(way.GetDirection(i)) * Vector3.right * -RoadWidth);
            }

            points.Add(way.GetPoint(1) + Quaternion.LookRotation(way.GetDirection(1)) * Vector3.right * RoadWidth + Quaternion.LookRotation(way.GetDirection(1)) * Vector3.forward * 10f);
            points.Add(way.GetPoint(1) + Quaternion.LookRotation(way.GetDirection(1)) * Vector3.right * -RoadWidth + Quaternion.LookRotation(way.GetDirection(1)) * Vector3.forward * 10f);
        }

        private void Update()
        {
            if(changeInRuntime)
            {
                points.Clear();
                GenerateVertices();
                mesh.SetVertices(points);
            }

        }
    }
}

