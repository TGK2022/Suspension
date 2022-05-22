using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor : MonoBehaviour
{
    //the door from this object
    // Child of this
    public bool isOpened = false;

    // this is the movement rate (if movemnt is applied to the door)
    public float moveSpeed = 3;
    // this is the rotation rate (if rotation is applied to the door)
    public float rotationSpeed = 90;

    private GameObject LeftPanel;
    private GameObject RightPanel;

    //private Quaternion closeRotation;
    //private Quaternion openRotation;


    void Start()
    {
        LeftPanel = this.transform.Find("LeftPanel").gameObject;
        RightPanel = this.transform.Find("RightPanel").gameObject;

        //closeRotation = LeftPanel.transform.localRotation;
        //openRotation = closeRotation;
        //openRotation.y += 90;
    }

    void Update()
    {
        float currentLeftY = LeftPanel.transform.localEulerAngles.y;

        if (currentLeftY < 15)
        {
            LeftPanel.GetComponent<Collider>().enabled = true;
            RightPanel.GetComponent<Collider>().enabled = true;
        }
        else
        {
            LeftPanel.GetComponent<Collider>().enabled = false;
            RightPanel.GetComponent<Collider>().enabled = false;
        }

        if (isOpened)
        {
            LeftPanel.transform.localRotation = Quaternion.Euler(0, Mathf.Min(90, currentLeftY + rotationSpeed * Time.deltaTime), 0);
            RightPanel.transform.localRotation = Quaternion.Euler(0, Mathf.Max(-90, -currentLeftY - rotationSpeed * Time.deltaTime), 0);
        }
        else
        {
            LeftPanel.transform.localRotation = Quaternion.Euler(0, Mathf.Max(0, currentLeftY - rotationSpeed * Time.deltaTime), 0);
            RightPanel.transform.localRotation = Quaternion.Euler(0, Mathf.Min(0, -currentLeftY + rotationSpeed * Time.deltaTime), 0);
        }
    }

    void OnTriggerEnter(Collider cube)
    {
        // whenever anything enters the trigger, open the door
        if (cube.gameObject.tag == "Player")
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
