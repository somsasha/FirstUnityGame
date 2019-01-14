using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Scrolling script for layer
public class ScrollingScript : MonoBehaviour
{
    // Scrolling speed
    public Vector2 speed = new Vector2(2, 2);

    // Moving direction
    public Vector2 direction = new Vector2(-1, 0);

    // Need to link to camera
    public bool IsLinkedToCamera = false;

    // Infinity background
    public bool isLooping = false;

    // List of childs with render
    private List<Transform> backgroundPart;

    // Start is called before the first frame update
    void Start()
    {
        // Take all childs
        if (isLooping)
        {
            // Take all
            backgroundPart = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                // Only Visible
                if (child.GetComponent<Renderer>() != null)
                {
                    if (ObjectSaver.Instance.backgroundObjects != null && ObjectSaver.Instance.backgroundObjects.Count > 0)
                    {
                        child.position = ObjectSaver.Instance.backgroundObjects[i].position;
                    }
                    backgroundPart.Add(child);
                }

                // Sort by position (from left to right)
                backgroundPart = backgroundPart.OrderBy(
                    t => t.position.x
                    ).ToList();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movment
        Vector3 movement = new Vector3(
            speed.x * direction.x,
            speed.y * direction.y,
            0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Camera movement
        if (IsLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        // Loop
        if (isLooping)
        {
            // Get First Child
            Transform firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                // Check if child (particaly) before camera
                if (firstChild.position.x < Camera.main.transform.position.x)
                {
                    // Check if child go from frame to reuse it
                    if (firstChild.GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
                    {
                        Transform lastChild = backgroundPart.LastOrDefault();
                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.GetComponent<Renderer>().bounds.max - lastChild.GetComponent<Renderer>().bounds.min);

                        // Move reusebale item next to last child
                        firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

                        // Move reuseble item to end of backgroundPart
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);
                    }
                }
            }
        }
    }
}
