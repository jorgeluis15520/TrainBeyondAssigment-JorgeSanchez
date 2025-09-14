using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GraspableNut : MonoBehaviour
{
    private Rigidbody rgbd;
    private bool isOnSocket;
    private void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }
    public void MoveToTransform(Transform target)
    {
        rgbd.linearVelocity = Vector3.zero;
        rgbd.MovePosition(target.position);
        rgbd.MoveRotation(target.rotation);
    }

    public void Grabbed()
    {
        GameEvents.InvokeNutGrabbed();
    }

    public void Released()
    {
        GameEvents.InvokeNutDropped();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Socket"))
        {
            isOnSocket = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Socket"))
        {
            isOnSocket = false;
        }
    }

    public void ResetIsOnRange() { isOnSocket = false; }
    public bool IsOnSocketRange() { return isOnSocket; }
}
