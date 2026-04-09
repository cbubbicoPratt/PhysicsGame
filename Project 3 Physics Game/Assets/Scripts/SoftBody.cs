using System;
using UnityEngine;

[RequireComponent (typeof(SkinnedMeshRenderer))]

public class SoftBody : MonoBehaviour
{
    [Range(0, 2f)]
    public float softness = 1f; //how far verticies can move (higher = more floppy)
    //how much motion slows down (like friction)
    [Range(0.01f, 1f)]
    public float damping = 0.1f;
    //how resistant it is to bending
    public float stiffness = 1f;

    void Start()
    {
        CreateSoftBodyPhysics();
    }

    void CreateSoftBodyPhysics()
    {
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer> ();
        if (smr == null) return;

        //add unity cloth physics component to obj at runtime
        Cloth cloth = gameObject.AddComponent<Cloth>();
        cloth.damping = damping;
        cloth.bendingStiffness = stiffness;

        //every vertex in mesh gets physics rule
        //we generate rules w/ our funcion
        cloth.coefficients = GenerateClothCoefficients(smr.sharedMesh.vertices.Length);
    }

    //making array so we have multiple coefficients for all verticies
    //ex: mesh may have 500 verts. so cloth needs 500 coefficients (one per vertex)
    private ClothSkinningCoefficient[] GenerateClothCoefficients(int vertexCount)
    {
        //[] creates array; one entry per vertex
        //make a list w/ vertexcount slots
        ClothSkinningCoefficient[] coefficients = new ClothSkinningCoefficient[vertexCount];

        //loop thru every vertex
        //set rules for each vertex 1 by 1
        for (int i = 0; i < vertexCount; i++)
        {
            //how far vertex can move
            coefficients[i].maxDistance = softness;
            //collision buffer; 0 = tight
            coefficients[i].collisionSphereDistance = 0f;
            //so basically every vertex can move up to softness distance
        }

        return coefficients; //send it back to cloth component
    }
}
