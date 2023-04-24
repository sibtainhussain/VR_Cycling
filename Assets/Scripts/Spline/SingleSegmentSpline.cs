using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class SingleSegmentSpline : MonoBehaviour
{
    [SerializeField] bool update = true;
    [SerializeField] Mesh2D shape2D;
    [Range(2, 32)] [SerializeField] int edgeRingCount = 8;
    [Range(0,1)] [SerializeField] float tTest = 0;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float controlPointRadius= 1f;
    Mesh mesh;
    
    Vector3 GetPos(int i) {
        if(i == 0){
            return startPoint.position;
        }
        else if(i == 1){
            return startPoint.TransformPoint(Vector3.forward * startPoint.localScale.z);
        }
        else if(i == 2){
            return endPoint.TransformPoint(Vector3.back * endPoint.localScale.z);
        }
        else if(i == 3){
            return endPoint.position;
        }
        return default;
    }

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void Start() 
    {
        GenerateMesh();
    }

    void Update() 
    {
        if(update){
            GenerateMesh();
        }
       
    }

    void GenerateMesh()
    {
        mesh.Clear();

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        for(int ring = 0; ring < edgeRingCount; ring++){
            float t = ring / (edgeRingCount - 1f);
            OrientedPoint op = GetBezierPoint(t);
            for(int i = 0; i < shape2D.VertexCount; i++){
                verts.Add(op.LocalToWorld(shape2D.vertices[i].point));
                normals.Add(op.LocalToWorldVector(shape2D.vertices[i].normal));
            }
        }

        List<int> triIndeces = new List<int>();
        for(int ring = 0; ring < edgeRingCount-1; ring++){
            int rootIndex = ring * shape2D.VertexCount;
            int rootIndexNext = (ring+1) * shape2D.VertexCount;
            for(int line = 0; line < shape2D.LineCount; line+=2){
                int currentA = shape2D.lineIndices[line] + rootIndex;
                int currentB = shape2D.lineIndices[line + 1] + rootIndex;
                int nextA = shape2D.lineIndices[line] + rootIndexNext;
                int nextB = shape2D.lineIndices[line + 1] + rootIndexNext;

                triIndeces.Add(currentA);
                triIndeces.Add(nextA);
                triIndeces.Add(nextB);

                triIndeces.Add(currentB);
                triIndeces.Add(currentA);
                triIndeces.Add(nextB);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetNormals(normals);
        mesh.SetTriangles(triIndeces, 0);

    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere( GetPos(1), controlPointRadius);
        Gizmos.DrawSphere( GetPos(2), controlPointRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere( GetPos(0), controlPointRadius);
        Gizmos.DrawSphere( GetPos(3), controlPointRadius);
        Gizmos.color = Color.white;

        Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f); 

        OrientedPoint testPoint = GetBezierPoint(tTest);
        Handles.PositionHandle(testPoint.pos, testPoint.rot);

        Vector3[] verts = shape2D.vertices.Select(v => testPoint.LocalToWorld(v.point)).ToArray();
        for(int i = 0; i < shape2D.lineIndices.Length; i+=2){
            Vector3 a = verts[shape2D.lineIndices[i]];
            Vector3 b = verts[shape2D.lineIndices[i+1]];
            Gizmos.DrawLine(a, b);
        }

    }

    OrientedPoint GetBezierPoint(float t){
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

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
