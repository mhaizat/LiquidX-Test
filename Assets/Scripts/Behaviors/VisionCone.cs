using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    [SerializeField] private AIbehaviorManager behaviorManager;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private int rayCount = 100;
    [SerializeField] private float viewDistance;

    private MeshFilter meshFilter;
    private Mesh mesh;
    [SerializeField] private Material material;

    [SerializeField] private LayerMask playerMask;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    private void LateUpdate()
    {
        CreateVisionCone();
    }

    void CreateVisionCone()
    {
        Vector3[] vertices = new Vector3[rayCount + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        float angleIncrement = viewAngle / rayCount;
        float angle = -viewAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, viewDistance, behaviorManager.GetObstacleLayerMask() | playerMask))
            {
                vertices[i + 1] = transform.InverseTransformPoint(hit.point);
            }
            else
            {
                vertices[i + 1] = transform.InverseTransformPoint(transform.position + dir * viewDistance);
            }

            if (i > 0)
            {
                triangles[(i - 1) * 3] = 0;
                triangles[(i - 1) * 3 + 1] = i;
                triangles[(i - 1) * 3 + 2] = i + 1;
            }

            angle += angleIncrement;
        }

        triangles[rayCount * 3 - 3] = 0;
        triangles[rayCount * 3 - 2] = rayCount;
        triangles[rayCount * 3 - 1] = 1;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        if (!behaviorManager.GetIsLooking())
        {
            Vector3 playerDirection = behaviorManager.GetPlayerTransform().position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, playerDirection);
            if (angleToPlayer <= viewAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, playerDirection, out hit, viewDistance, behaviorManager.GetObstacleLayerMask() | playerMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        behaviorManager.SetLastKnownPlayerLocation();

                        if (!behaviorManager.GetBehaviorState("Chase"))
                        { 
                            behaviorManager.SetBehaviorState("Chase");
                        }
                    }
                }
            }
            else
            {
                if (!behaviorManager.GetBehaviorState("Patrol"))
                {
                    behaviorManager.SetBehaviorState("Patrol");
                }
            }
        }
    }
}
