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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Socket"))
        {
            isOnSocket = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Socket"))
        {
            isOnSocket = false;
        }
    }

    public bool IsOnSocketRange() { return isOnSocket;}
}
