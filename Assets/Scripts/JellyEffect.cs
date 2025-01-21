using UnityEngine;
using DG.Tweening; // Import DoTween namespace

[RequireComponent(typeof(MeshFilter))]
public class SoftBodyJelly : MonoBehaviour
{
    public class JellyVertex
    {
        public int ID;
        public Vector3 position;
        public Vector3 velocity, force;

        public JellyVertex(int _id, Vector3 _pos)
        {
            ID = _id;
            position = _pos;
        }

        public void Shake(Vector3 target, float mass, float stiffness, float damping)
        {
            force = (target - position) * stiffness;
            velocity = (velocity + force / mass) * damping;
            position += velocity;

            if ((velocity + force / mass).magnitude < 0.001f)
                position = target;
        }
    }

    public float Intensity = 1f;
    public float Mass = 1f;
    public float Stiffness = 1f;
    public float Damping = 0.75f;

    private Mesh originalMesh, meshClone;
    private JellyVertex[] jellyVertices;
    private Vector3[] vertexArray;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        // Duplicate the mesh to allow runtime modification
        originalMesh = GetComponent<MeshFilter>().sharedMesh;
        meshClone = Instantiate(originalMesh);
        GetComponent<MeshFilter>().sharedMesh = meshClone;
        meshRenderer = GetComponent<MeshRenderer>();

        // Initialize jelly vertices
        jellyVertices = new JellyVertex[meshClone.vertices.Length];
        for (int i = 0; i < meshClone.vertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(i, transform.TransformPoint(meshClone.vertices[i]));
        }
    }

    private void FixedUpdate()
    {
        vertexArray = originalMesh.vertices;

        for (int i = 0; i < jellyVertices.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jellyVertices[i].ID]);
            float intensity = (1 - (meshRenderer.bounds.max.y - target.y) / meshRenderer.bounds.size.y) * Intensity;

            // Apply soft body physics
            jellyVertices[i].Shake(target, Mass, Stiffness, Damping);

            // Update vertex positions
            target = transform.InverseTransformPoint(jellyVertices[i].position);
            vertexArray[jellyVertices[i].ID] = Vector3.Lerp(vertexArray[jellyVertices[i].ID], target, intensity);
        }

        meshClone.vertices = vertexArray;

        // Optionally recalculate normals for lighting effects
        meshClone.RecalculateNormals();
    }

    public void AnimateJelly(Vector3 impactPoint, float duration)
    {
        // Use DoTween to simulate a global "bounce" effect
        transform.DOPunchScale(Vector3.one * 0.1f, duration, 10, 1);
    }
}
