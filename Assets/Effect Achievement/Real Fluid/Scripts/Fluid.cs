using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using System.Threading;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Fluid : MonoBehaviour
{
    [Header("Mesh Settings")]
    [Min(0)] public float width = 20;
    [Min(0)] public float height = 5;
    [Min(2)] public int vertexCount = 40;

    private MeshRenderer render;
    private MeshFilter filter;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    [Header("Physics Settings")]
    public bool simulationPhysics = true;
    public bool useBuoyancy = true;
    public bool addCollider = true;
    public float springConstant = 0.02f; // ����ϵ��
    public float damping = 0.1f; // ����ϵ��
    public float spread = 0.1f; // ������Χ
    public float collisionVelocityFactor = 0.04f; //��ײ����ٶȶ�����Ӱ��
    public float time = 5;

    private BoxCollider2D boxCollider;
    private BuoyancyEffector2D buoyancy;
    private float[] velocities;
    private float[] accelerations;
    private float[] leftDeltas;
    private float[] rightDeltas;
    private float timer;

    [Header("Render Settings")]
    public string sortingLayer = "Default";
    public int orderInLayer;

    private void OnValidate()
    {
        Create();
    }

    private void Awake()
    {
        Create();
        InitSimulation();
    }

    private void Update()
    {
        Simulation();
    }

    private void Create()
    {
        // ���㶥��λ�ú�UV
        vertices = new Vector3[vertexCount * 2];
        uvs = new Vector2[vertexCount * 2];
        float interval = width / (vertexCount - 1);
        for (int i = 0; i < vertexCount; i++)
        {
            float x = i * interval - width * 0.5f;
            float uvX = (float)i / vertexCount;
            // ��
            vertices[i] = new Vector3(x, height * 0.5f, 0);
            uvs[i] = new Vector2(uvX, 1);
            // ��
            vertices[i + vertexCount] = new Vector3(x, -height * 0.5f, 0);
            uvs[i + vertexCount] = new Vector2(uvX, 0);
        }

        // ����
        triangles = new int[(vertexCount - 1) * 6];
        int triIndex = 0;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            triangles[triIndex] = i;
            triangles[triIndex + 1] = i + 1;
            triangles[triIndex + 2] = i + vertexCount;

            triangles[triIndex + 3] = i + vertexCount;
            triangles[triIndex + 4] = i + 1;
            triangles[triIndex + 5] = i + vertexCount + 1;

            triIndex += 6;
        }

        // Mesh
        mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);

        if (render == null)
            render = GetComponent<MeshRenderer>();
        render.sortingLayerName = sortingLayer;
        render.sortingOrder = orderInLayer;
        if (filter == null)
            filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;

        if (addCollider || useBuoyancy)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(width, height);
            boxCollider.isTrigger = true;

            if (useBuoyancy)
                boxCollider.usedByEffector = true;
        }

        if (useBuoyancy) 
        {
            buoyancy = GetComponent<BuoyancyEffector2D>();
            if (buoyancy == null)
                buoyancy = gameObject.AddComponent<BuoyancyEffector2D>();
            buoyancy.surfaceLevel = height * 0.5f;
        }
    }

    /// <summary> ��ʼ������ģ������ </summary>
    private void InitSimulation() 
    {
        velocities = new float[vertexCount];
        accelerations = new float[vertexCount];
        leftDeltas = new float[vertexCount];
        rightDeltas = new float[vertexCount];
    }

    /// <summary> ����ģ�� </summary>
    /// ԭ��https://gamedevelopment.tutsplus.com/tutorials/make-a-splash-with-dynamic-2d-water-effects--gamedev-236
    private void Simulation()
    {
        if (timer <= 0)
            return;
        timer -= Time.deltaTime;

        for (int i = 0; i < vertexCount; i++)
        {
            // ����λ��
            vertices[i] += new Vector3(0, velocities[i], 0);
            // ���¼����������ٶȡ��ٶ�
            float force = springConstant * (vertices[i].y - height * 0.5f) + velocities[i] * damping;
            accelerations[i] = -force;
            velocities[i] += accelerations[i];
        }

        for (int i = 0; i < vertexCount; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = spread * (vertices[i].y - vertices[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }
            if (i < vertexCount - 1)
            {
                rightDeltas[i] = spread * (vertices[i].y - vertices[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }

        mesh.SetVertices(vertices);
    }

    /// <summary> ��ˮ </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (simulationPhysics)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            Splash(col, rb.velocity.y * collisionVelocityFactor);
        }
    }

    /// <summary> ����ˮʱ���� </summary>
    public void Splash(Collider2D col, float force)
    {
        timer = time;
        float radius = col.bounds.max.x - col.bounds.min.x;
        Vector2 center = new Vector2(col.bounds.center.x, height * 0.5f);
        for (int i = 0; i < vertexCount; i++)
        {
            if (PointInsideCircle(vertices[i], center, radius))
            {
                velocities[i] = force;
            }
        }
    }

    /// <summary> ���Ƿ��ڷ�Χ�� </summary>
    private bool PointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        return Vector2.Distance(point, center) < radius;
    }
}
