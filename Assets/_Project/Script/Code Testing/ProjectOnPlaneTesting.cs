using UnityEngine;

[ExecuteAlways]
public class ProjectOnPlaneTesting : MonoBehaviour
{
    [SerializeField, Range(1f,10f)]
    private float rayLength;

    public Vector3 direction;
    private void ProjectOnPlane()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, Vector3.down); // this one checks the ground
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            direction = Vector3.ProjectOnPlane(transform.forward, hit.normal); // changes the projection of movement based on plane
            Debug.DrawRay(transform.position,direction * rayLength, Color.red);
        }


    }

    private void Update()
    {
        ProjectOnPlane();
    }
}
