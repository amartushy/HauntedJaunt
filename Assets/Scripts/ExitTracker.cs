using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Used this thread https://stackoverflow.com/questions/55922893/how-to-make-a-2d-arrow-on-the-screen-that-always-point-to-a-3d-object-inside-the
// as well as ChatGPT ai to make this script
// also found these videos very helpful https://www.youtube.com/watch?v=b4Xsv85Mkhg, https://www.youtube.com/watch?v=pYEAwiKFKeg

public class ExitTracker : MonoBehaviour
{
    public GameObject Target;

    // RectTransform rt;

    // Start is called before the first frame update
    //void Start()
    //{
    //rt = GetComponent<RectTransform>();
    //}

    // Update is called once per frame
    //void Update()
    //{
    //  Vector3 objScreenPos = Camera.main.WorldToScreenPoint(Target.transform.position);
    //
    //Vector3 dir = (objScreenPos - rt.position).normalized;
    //
    //  float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(dir, Vector3.up));

    //   Vector3 cross = Vector3.Cross(dir, Vector3.up);
    // angle = -Mathf.Sign(cross.y) * angle;

    //    rt.localEulerAngles = new Vector3(rt.localEulerAngles.x, angle, rt.localEulerAngles.z);
    //   }


    // abandoned the above method because it required the arrow
    // to be a child of the player, which was causing issues since the 
    // rotation was bound to player direction

    public Transform player;
    public Transform target;

    void Update()
    {
        // Calculate direction from player to target
        Vector3 direction = target.position - player.position;

        // Normalize the direction vector
        direction.Normalize();

        Vector3 projectedDirection = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;

        // Calculate the angle between forward direction and direction to target using dot product
        float dot = Vector3.Dot(Vector3.forward, projectedDirection);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // Check if the target is above or below the player
        if (direction.x < 0)
        {
            angle = 360 - angle;
        }

        // Set arrow's rotation
        transform.rotation = Quaternion.Euler(0, angle - 90f, 0);

        // Set arrow's position relative to player's position
        transform.position = player.position + new Vector3(-0.2f, 1.8f, 0);
    }
}
    
