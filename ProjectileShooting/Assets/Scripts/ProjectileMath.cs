using UnityEngine;

public static class ProjectileMath
{
    /// <summary>
    /// Calculates the initial launch speed required to hit a target at distance with elevation yOffset.
    /// </summary>
    /// <param name="distance">Planar distance from origin to the target</param>
    /// <param name="yOffset">Elevation of the origin with respect to the target</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <param name="angle">Initial launch angle in radians</param>
    /// <returns>Initial launch speed</returns>
    public static float LaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        float speed = (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));

        return speed;
    }

    /// <summary>
    /// Samples a series of points along a projectile arc
    /// </summary>
    /// <param name="iterations">Number of points to sample</param>
    /// <param name="speed">Initial speed of the projectile</param>
    /// <param name="distance">Distance the projectile will travel along the horizontal axis</param>
    /// <param name="gravity">Downward acceleration in m/s^2</param>
    /// <param name="angle">Initial launch angle in radians</param>
    /// <returns>Array of sampled points with the length of the supplied iterations</returns>
    public static Vector2[] ProjectileArcPoints(int iterations, float speed, float distance, float gravity, float angle)
    {
        float iterationSize = distance / iterations;

        float radians = angle;

        Vector2[] points = new Vector2[iterations + 1];

        for (int i = 0; i <= iterations; i++)
        {
            float x = iterationSize * i;
            float t = x / (speed * Mathf.Cos(radians));
            float y = -0.5f * gravity * (t * t) + speed * Mathf.Sin(radians) * t;

            Vector2 p = new Vector2(x, y);

            points[i] = p;
        }

        return points;
    }
}
