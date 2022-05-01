using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //the door from this object
    public GameObject Panel;  // Child of this
    public bool isOpened = false;

    // this is the movement rate (if movemnt is applied to the door)
    public float moveSpeed = 3;
    // this is the rotation rate (if rotation is applied to the door)
    public float rotationSpeed = 90;

    private Quaternion closeRotation;
    private Quaternion openRotation;


    void Start()
    {
        closeRotation = Panel.transform.localRotation;
        openRotation = closeRotation;
        openRotation.y += 90;
    }

    void Update()
    {
        float currentY = Panel.transform.localEulerAngles.y;
        if (isOpened)
        {
            Panel.transform.localRotation = Quaternion.Euler(0, Mathf.Min(90, currentY + rotationSpeed * Time.deltaTime), 0);
        }
        else
        {
            Panel.transform.localRotation = Quaternion.Euler(0, Mathf.Max(0, currentY - rotationSpeed * Time.deltaTime), 0);
        }
    }

    void OnTriggerEnter(Collider cube)
    {
        // whenever anything enters the trigger, open the door
        if(cube.gameObject.tag == "Player")
        {
            isOpened = true;
        }
    }

    void OnTriggerExit(Collider cube)
    {
        // whenever anything exits the trigger, close the door.
        if (cube.gameObject.tag == "Player")
        {
            isOpened = false;
        }
    }
}
