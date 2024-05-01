using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshRefresher : MonoBehaviour
{
    private int ij = 0;
    private NavMeshSurface2d m_Surface;
    public bool ActiveRefresh = true;


    // Start is called before the first frame update
    void OnEnable()
    {
        m_Surface = GetComponent<NavMeshSurface2d>();
        BuildNavMesh();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ActiveRefresh)
        {
            ij++;
            if (ij >= 100)
            {
                BuildNavMesh();
                ij = 0;
            }
        }
    }
    public void BuildNavMesh()
    {
        m_Surface.BuildNavMesh();
    }

}
