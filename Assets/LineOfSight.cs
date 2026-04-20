using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private float angle = 90f;
    [SerializeField] private LayerMask obstacleMask;

    public bool isInRange(Transform self, Transform target)
    {
        return Vector3.Distance(self.position, target.position) <= distance;
    }

    public bool isInAngle(Transform self, Transform target)
    {
        Vector3 dir = (target.position - self.position).normalized;
        return Vector3.Angle(self.forward, dir) <= angle * 0.5f;
    }

    public bool hasLineOfSight(Transform self, Transform target)
    {
        Vector3 origin = self.position + Vector3.up * 0.5f;
        Vector3 dir = (target.position - origin);

        if (Physics.Raycast(origin, dir.normalized, dir.magnitude, obstacleMask))
        {
            return false;
        }

        return true;
    }

    public bool CanSeeTarget(Transform self, Transform target)
    {
        return isInRange(self, target) &&
               isInAngle(self, target) &&
               hasLineOfSight(self, target);
    }
}