using UnityEngine;

public class CalibanAI : MonoBehaviour
{
    // The speed at which Caliban moves
    private float moveSpeed = 5f;

    // The range at which Caliban can detect objects in its path
    public float detectRange = 5f;
    ,
    // The layer mask for objects that Caliban can detect
    public LayerMask detectionLayer;

    // The distance Caliban will stop from an obstacle
    public float stoppingDistance = 2f;

    // The target object that Caliban will move towards
    private Transform target;

    // The direction Caliban is facing
    private Vector3 forward;

    public global::System.Single moveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // Update is called once per frame
    void Update()
    {
        // Check if Caliban has a target
        if (target != null)
        {
            // Calculate the distance to the target
            float distance = Vector3.Distance(transform.position, target.position);

            // If Caliban is within range of the target, stop moving
            if (distance < stoppingDistance)
            {
                forward = Vector3.zero;
            }
            else
            {
                // Calculate the direction to the target and move towards it
                forward = Vector3.Normalize(target.position - transform.position);
                transform.position += forward * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            // Search for objects in Caliban's path
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRange, detectionLayer);

            // If an object is found, move towards it
            if (hitColliders.Length > 0)
            {
                target = hitColliders[0].transform;
            }
        }

        // Rotate Caliban to face its movement direction
        if (forward != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * 5f);
        }
    }

    // Draw the detection range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
