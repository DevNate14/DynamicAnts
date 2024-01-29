using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [SerializeField] bool PointMode;
    [SerializeField] Transform Point;
    [SerializeField] float Speed;
    private void OnTriggerStay(Collider other)
    {
        if (PointMode && Point != null)
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, Point.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, other.transform.position + 1.2f * transform.localScale.x * transform.forward, Speed * Time.deltaTime);
        }
        // the treadmill now correctly moves the player toward the front instead of a single point, now toggleable by a bool
        // Major issue: this does not work for the non-rigidbody player controller unless there is a bullet in scene (why? no clue)
    }
}
