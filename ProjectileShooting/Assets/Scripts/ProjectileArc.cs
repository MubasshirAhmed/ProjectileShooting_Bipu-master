using UnityEngine;

public class ProjectileArc : MonoBehaviour
{
    [SerializeField]
    int iterations = 20;

    [SerializeField]
    Color errorColor;

    [SerializeField]
    Cursor targetCursor;

    [SerializeField]
    Transform firePoint;

    private float currentSpeed;
    private float currentAngle;

    private Color initialColor;
    private LineRenderer lineRenderer;

    [SerializeField]
    Transform cannonBase;

    [SerializeField]
    Transform turret;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    float cooldown = 1;

    private float lastShotTime;

    public int cannonAngle = 45;

    Vector3 direction;

    

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialColor = lineRenderer.material.color;
    }

    private void Update()
    {
        SetTargetWithAngle(targetCursor.transform.position, cannonAngle);

        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    public void SetTargetWithAngle(Vector3 point, float angle)
    {
        currentAngle = angle;
        //point = new Vector3(-8, 0, 8);
        direction = point - firePoint.position;
        float yOffset = -direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);

        UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, currentAngle * Mathf.Deg2Rad, direction, true);
        SetTurret(direction, currentAngle);
    }

    public void Fire()
    {
        if (Time.time > lastShotTime + cooldown)
        {
            GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            p.GetComponent<Rigidbody>().velocity = turret.up * currentSpeed;
            Debug.Log(currentSpeed);
            Debug.Log(turret.up);
            Debug.Log(p.GetComponent<Rigidbody>().velocity);
            lastShotTime = Time.time;
        }
    }

    private void SetTurret(Vector3 planarDirection, float turretAngle)
    {
        cannonBase.rotation = Quaternion.LookRotation(planarDirection) * Quaternion.Euler(-90, -90, 0);
        turret.localRotation = Quaternion.Euler(90, 90, 0) * Quaternion.AngleAxis(turretAngle, Vector3.forward);
    }

    public void UpdateArc(float speed, float distance, float gravity, float angle, Vector3 direction, bool valid)
    {
        Vector2[] arcPoints = ProjectileMath.ProjectileArcPoints(iterations, speed, distance, gravity, angle);
        Vector3[] points3d = new Vector3[arcPoints.Length];

        for (int i = 0; i < arcPoints.Length; i++)
        {
            points3d[i] = new Vector3(0, arcPoints[i].y, arcPoints[i].x);
        }

        lineRenderer.positionCount = arcPoints.Length;
        lineRenderer.SetPositions(points3d);

        transform.rotation = Quaternion.LookRotation(direction);

        lineRenderer.material.color = valid ? initialColor : errorColor;
    }
}
