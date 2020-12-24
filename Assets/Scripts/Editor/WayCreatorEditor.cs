using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace CurvesWay.Tools.Way
{
    [CustomEditor(typeof(WaySplineCreator))]
    public class WayCreatorEditor : Editor
    {
        private const int stepsPerCurve = 10;
        private const float directionScale = 0.5f;
        private const float handleSize = 0.04f;
        private const float pickSize = 0.06f;

        private WaySplineCreator way;
        private Transform handleTransform;
        private Quaternion handleRotation;

        private int currentLength;

        private int selectedIndex = -1;

        public override void OnInspectorGUI()
        {
            way = target as WaySplineCreator;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", way.Loop);
            float endprogress = EditorGUILayout.FloatField("End progress", way.EndProgress);
            float startprogress = EditorGUILayout.FloatField("Start progress", way.StartProgress);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(way, "Toggle Loop");
                EditorUtility.SetDirty(way);
                way.Loop = loop;
                way.EndProgress = endprogress;
                way.StartProgress = startprogress;
            }

            if (selectedIndex >= 0 && selectedIndex < way.ControlPointCount)
            {
                DrawSelectedPointInspector();
            }
            if (GUILayout.Button("Create point"))
            {
                Undo.RecordObject(way, "Add Curve");
                way.AddCurve();
                EditorUtility.SetDirty(way);
            }
        }

        private void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", way.GetControlPoint(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(way, "Move Point");
                EditorUtility.SetDirty(way);
                way.SetControlPoint(selectedIndex, point);
            }

            EditorGUI.BeginChangeCheck();
            BezierControlPointMode mode = (BezierControlPointMode)
                EditorGUILayout.EnumPopup("Mode", way.GetControlPointMode(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(way, "Change Point Mode");
                way.SetControlPointMode(selectedIndex, mode);
                EditorUtility.SetDirty(way);
            }
        }

        private void OnSceneGUI()
        {
            way = target as WaySplineCreator;
            handleTransform = way.transform;
            handleRotation = UnityEditor.Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

            DrawLines();
        }

        private void DrawLines()
        {
            
            Vector3 p0 = ShowPoint(0);
            Handles.color = Color.green;

            for (int i = 1; i < way.ControlPointCount; i+=3)
            {
                Vector3 p1 = ShowPoint(i);
                Vector3 p2 = ShowPoint(i+1);
                Vector3 p3 = ShowPoint(i+2);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2,p3);

                Handles.DrawBezier(p0,p3,p1,p2, Color.white, null, 2f);
                p0 = p3;
            }
            ShowDirection();
        }

        private void ShowDirection()
        {
            Handles.color = Color.green;
            Vector3 point = way.GetPoint(0f);
            Handles.DrawLine(point, point + way.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * way.CurveCount;
            for (int i = 1; i <= steps; i++)
            {
                point = way.GetPoint(i / (float)steps);
                Handles.DrawLine(point, point + way.GetDirection(i / (float)steps) * directionScale);
            }
        }

        private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
        };

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = handleTransform.TransformPoint(way.GetControlPoint(index));
            float size = HandleUtility.GetHandleSize(point);
            Handles.color = modeColors[(int)way.GetControlPointMode(index)];
            if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
            {
                selectedIndex = index;
                Repaint();
            }
            if (selectedIndex == index)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(way, "Move Point");
                    EditorUtility.SetDirty(way);
                    way.SetControlPoint(index,handleTransform.InverseTransformPoint(point));
                }
            }
            return point;
        }        
    }
}

