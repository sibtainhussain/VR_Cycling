using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SplinePath : MonoBehaviour
{
    [SerializeField] List<Transform> controlPoints = new List<Transform>();
    [SerializeField] List<SplineSegment> segments = new List<SplineSegment>();
    [SerializeField] bool closeLoop;
    [Range(2, 32)] public int edgeRingCount = 8;
    public Mesh2D defaultShape2D;
    public Material defaultMaterial;
    [SerializeField] float controlPointRadius = 1f;
    [Range(0,0.999f)] [SerializeField] float tTest = 0;
    public float pathLength;

    void Start() {
        CreateSegments();
    }

    void Update() {
        GenerateMeshes();
    }

    void GenerateMeshes(){
        pathLength = 0f;
        foreach(SplineSegment s in segments) {;
            s.GenerateMesh();
            pathLength += s.SegmentLength();
        }
        Transform extraPath = this.gameObject.transform.Find("Segment " + -1);
        if(extraPath != null) {
            extraPath.gameObject.GetComponent<SplineSegment>().GenerateMesh();
            pathLength += extraPath.gameObject.GetComponent<SplineSegment>().SegmentLength();
        }
    }

    public void OnDrawGizmos(){
        int extraSegment = closeLoop ? 1 : 0;
        float dashSize = 4.0f;
        int segmentIndex = Mathf.FloorToInt(tTest * (controlPoints.Count - 1 + extraSegment));
        float tValue = tTest * (controlPoints.Count - 1 + extraSegment) - segmentIndex;
        for(int i = 0; i < controlPoints.Count - 1; i++) {
            Transform startPoint = controlPoints[i];
            Transform endPoint = controlPoints[i+1];
            Vector3 p0 = startPoint.position;
            Vector3 p1 = startPoint.TransformPoint(Vector3.forward * startPoint.localScale.z);
            Vector3 p2 = endPoint.TransformPoint(Vector3.back * endPoint.localScale.z);
            Vector3 p3 = endPoint.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p0, controlPointRadius);
            Gizmos.DrawSphere(p3, controlPointRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p1, controlPointRadius);
            Gizmos.DrawSphere(p2, controlPointRadius);
            Handles.DrawDottedLine(p0, p1, dashSize);
            Handles.DrawDottedLine(p3, p2, dashSize);

            Gizmos.color = Color.white;
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, EditorGUIUtility.whiteTexture, 1f); 

            if(i == segmentIndex) {
                OrientedPoint testPoint = GetBezierPoint(tValue, startPoint, endPoint, p0, p1, p2, p3);
                Handles.PositionHandle(testPoint.pos, testPoint.rot);
                Vector3[] verts = defaultShape2D.vertices.Select(v => testPoint.LocalToWorld(v.point)).ToArray();
                for(int j = 0; j < defaultShape2D.lineIndices.Length; j+=2){
                    Vector3 a = verts[defaultShape2D.lineIndices[j]];
                    Vector3 b = verts[defaultShape2D.lineIndices[j+1]];
                    Gizmos.DrawLine(a, b);
                }
            }
        }
        if (closeLoop) {
            Transform startPoint = controlPoints[controlPoints.Count-1];
            Transform endPoint = controlPoints[0];
            Vector3 p0 = startPoint.position;
            Vector3 p1 = startPoint.TransformPoint(Vector3.forward * startPoint.localScale.z);
            Vector3 p2 = endPoint.TransformPoint(Vector3.back * endPoint.localScale.z);
            Vector3 p3 = endPoint.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p0, controlPointRadius);
            Gizmos.DrawSphere(p3, controlPointRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p1, controlPointRadius);
            Gizmos.DrawSphere(p2, controlPointRadius);
            Handles.DrawDottedLine(p0, p1, dashSize);
            Handles.DrawDottedLine(p3, p2, dashSize);

            Gizmos.color = Color.white;
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, EditorGUIUtility.whiteTexture, 1f); 

            if(controlPoints.Count-1 == segmentIndex) {
                OrientedPoint testPoint = GetBezierPoint(tValue, startPoint, endPoint, p0, p1, p2, p3);
                Handles.PositionHandle(testPoint.pos, testPoint.rot);
                Vector3[] verts = defaultShape2D.vertices.Select(v => testPoint.LocalToWorld(v.point)).ToArray();
                for(int j = 0; j < defaultShape2D.lineIndices.Length; j+=2){
                    Vector3 a = verts[defaultShape2D.lineIndices[j]];
                    Vector3 b = verts[defaultShape2D.lineIndices[j+1]];
                    Gizmos.DrawLine(a, b);
                }
            }
        }
        Gizmos.color = Color.white;
        
    }

    public OrientedPoint GetPointAtPosition(float dist) {
        int extraSegment = closeLoop ? 1 : 0;
        float t = dist / pathLength;
        int segmentIndex = Mathf.FloorToInt(t * (controlPoints.Count - 1 + extraSegment));
        float tValue = t * (controlPoints.Count - 1 + extraSegment) - segmentIndex;
        if (segmentIndex < segments.Count) {
            SplineSegment s = segments[segmentIndex];
            return s.GetBezierPoint(tValue);
        }
        else{
            Transform extraPath = this.gameObject.transform.Find("Segment " + -1);
            if(extraPath != null) {
                return extraPath.gameObject.GetComponent<SplineSegment>().GetBezierPoint(tValue);
            }
        }
        Debug.Log("Invalid Position");
        return segments[0].GetBezierPoint(0f);
    }

    public void AddControlPoint() {
        GameObject newPoint = new GameObject("p" + controlPoints.Count);
        newPoint.transform.parent = this.gameObject.transform;
        if(controlPoints.Count > 0){
            newPoint.transform.position = controlPoints[^1].position + controlPoints[^1].forward * 30;
            newPoint.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1,1,3));
        }
        controlPoints.Add(newPoint.transform);
    }

    public void CreateSegments() {
        for(int i = segments.Count; i < controlPoints.Count - 1; i++) {
            Transform startPoint = controlPoints[i];
            Transform endPoint = controlPoints[i+1];
            GameObject pathSegment = new GameObject("Segment " + i);
            pathSegment.transform.parent = this.gameObject.transform;
            pathSegment.AddComponent<SplineSegment>();
            SplineSegment segmentComponent = pathSegment.GetComponent<SplineSegment>();
            segments.Add(segmentComponent);
            segmentComponent.startPoint = startPoint;
            segmentComponent.endPoint = endPoint;
            segmentComponent.shape2D = defaultShape2D;
            segmentComponent.path = this;
        }
        if(closeLoop && this.gameObject.transform.Find("Segment " + -1) == null) {
            Transform startPoint = controlPoints[controlPoints.Count-1];
            Transform endPoint = controlPoints[0];
            GameObject pathSegment = new GameObject("Segment " + -1);
            pathSegment.transform.parent = this.gameObject.transform;
            pathSegment.AddComponent<SplineSegment>();
            SplineSegment segmentComponent = pathSegment.GetComponent<SplineSegment>();
            segmentComponent.startPoint = startPoint;
            segmentComponent.endPoint = endPoint;
            segmentComponent.shape2D = defaultShape2D;
            segmentComponent.path = this;
        }
        GenerateMeshes();
    }

    public void ResetSegments() {
        for(int i = 0; i < segments.Count; i++) {
            DestroyImmediate(this.gameObject.transform.Find("Segment " + i).gameObject);
        }
        try
        {
            DestroyImmediate(this.gameObject.transform.Find("Segment " + -1).gameObject);
        }
        catch (System.Exception)
        {
            Debug.Log("No Segment -1");
        }
        segments = new List<SplineSegment>();
        pathLength = 0f;
    }

    OrientedPoint GetBezierPoint(float t, Transform startPoint, Transform endPoint, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3){
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;
        Vector3 up = Vector3.Lerp(startPoint.up, endPoint.up, t).normalized;
        Quaternion rot = Quaternion.LookRotation(tangent, up);

        return new OrientedPoint(pos, rot);
    }

}
