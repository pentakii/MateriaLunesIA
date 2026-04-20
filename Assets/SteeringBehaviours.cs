using UnityEngine;

public static class SteeringBehaviours
{
    public static Vector3 Seek(Transform self, Vector3 target)
    {
        Vector3 dir = target - self.position;
        dir.y = 0;
        return dir.normalized;
    }

    public static Vector3 Flee(Transform self, Vector3 target)
    {
        Vector3 dir = self.position - target;
        dir.y = 0;
        return dir.normalized;
    }

    public static Vector3 Wander(Vector3 currentDirection, float maxAngleChange)
    {
        float randomAngle = UnityEngine.Random.Range(-maxAngleChange, maxAngleChange);

        Quaternion rotation = Quaternion.Euler(0f, randomAngle, 0f);

        Vector3 newDirection = rotation * currentDirection;
        newDirection.y = 0f; 

        return newDirection.normalized;
    }
}