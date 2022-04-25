using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            this.GetComponent<Canvas>().enabled = false;
        }
        
        if(Keyboard.current.tabKey.wasReleasedThisFrame)
        {
            this.GetComponent<Canvas>().enabled = true;
        }
    }
}
