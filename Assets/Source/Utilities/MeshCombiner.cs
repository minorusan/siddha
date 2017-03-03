using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
public class Combiner:Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var mapGenerator = (MeshCombiner)target;
        if (GUILayout.Button("Combine"))
        {
            mapGenerator.CombineMeshes();
        }
    }
}

#endif
public class MeshCombiner : MonoBehaviour
{


    public void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.active = false;
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.active = true;

    }
}
