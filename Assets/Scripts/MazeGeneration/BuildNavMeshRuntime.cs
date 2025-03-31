using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class BuildNavMeshRuntime : MonoBehaviour
{
    NavMeshSurface _surface;

    // Start is called before the first frame update
    void Start()
    {
        _surface = GetComponent<NavMeshSurface>();

    }

    public void BuildNavMesh()
    {
        StartCoroutine(BuildNavMeshCoroutine());
    }

    IEnumerator BuildNavMeshCoroutine()
    {
        yield return new WaitForEndOfFrame();
        _surface.BuildNavMesh();

    }
}
