using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 90f;
    public float speed = 2f;
    public Transform player;
    private bool isOpening = false;
    public float interactionDistance = 3f;
    private bool isOpen = false;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    void Start()
    {
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;
        leftOpenRot = leftClosedRot * Quaternion.Euler(0, -openAngle, 0);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            isOpening = true;
            isOpen = !isOpen;
        }

        if (isOpening)
        {
            Quaternion targetLeft = isOpen ? leftOpenRot : leftClosedRot;
            Quaternion targetRight = isOpen ? rightOpenRot : rightClosedRot;

            leftDoor.localRotation = Quaternion.Slerp(leftDoor.localRotation, targetLeft, Time.deltaTime * speed);
            rightDoor.localRotation = Quaternion.Slerp(rightDoor.localRotation, targetRight, Time.deltaTime * speed);

            if (Quaternion.Angle(leftDoor.localRotation, targetLeft) < 0.5f)
            {
                isOpening = false;
            }
        }
    }

    bool IsPlayerNear()
    {
        if (player == null) return true; 

        float dist = Vector3.Distance(player.position, transform.position);
        return dist <= interactionDistance;
    }
}