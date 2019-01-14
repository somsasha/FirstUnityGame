using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundObjectSpawner : MonoBehaviour
{
    // Count of background object
    public int countOfEachplatformObjects;

    // Background object prefab
    public List<Transform> platformObjectsPrefabs;

    // List of childs with render
    private List<Transform> backgroundPart;

    // Start is called before the first frame update
    void Start()
    {
        backgroundPart = new List<Transform>();

        if (ObjectSaver.Instance.platformObjects != null && ObjectSaver.Instance.platformObjects.Count > 0)
        {
            foreach (Transform prefab in platformObjectsPrefabs)
            {
                foreach (ObjectSaver.SavedObject savedObject in ObjectSaver.Instance.platformObjects)
                {
                    if (savedObject.spriteName == prefab.GetComponent<SpriteRenderer>().sprite.name && savedObject.scale == prefab.transform.localScale.x)
                    {
                        Transform backgroundObject = Instantiate(prefab, savedObject.position, transform.rotation);
                        backgroundObject.parent = transform;
                    }
                }
            }
        }

        int childCount = transform.childCount;

        if (childCount < countOfEachplatformObjects)
        {
            for(int i = 0; i < (countOfEachplatformObjects - childCount); i++)
            {
                // Spawn background objects
                foreach (Transform t in platformObjectsPrefabs)
                {
                    Vector3 position = new Vector3(Camera.main.transform.position.x + Camera.main.orthographicSize + (Random.Range(3.0f, 15.0f) * Random.Range(1.0f, 2.0f)), Random.Range(-9.0f, 9.0f), transform.position.z);
                    Transform backgroundObject = Instantiate(t, position, transform.rotation);
                    backgroundObject.parent = transform;
                }
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Only Visible
            if (child.GetComponent<Renderer>() != null)
            {
                backgroundPart.Add(child);
            }

            // Sort by position (from left to right)
            backgroundPart = backgroundPart.OrderBy(
                t => t.position.x
                ).ToList();
        }
    }

    // Update is called once per frame
    void Update()
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

                    float startPosition;
                    // Start position to move
                    if (lastPosition.x > (Camera.main.transform.position.x + Camera.main.orthographicSize))
                    {
                        startPosition = lastPosition.x + lastSize.x;
                    }
                    else
                    {
                        startPosition = Camera.main.transform.position.x + Camera.main.orthographicSize;
                    }
                    // Move reusebale platform
                    firstChild.position = new Vector3(startPosition + Random.Range(5.0f, 15.0f), Random.Range(-9.0f, 9.0f), firstChild.position.z);

                    // Move reuseble item to end of backgroundPart
                    backgroundPart.Remove(firstChild);
                    backgroundPart.Add(firstChild);
                }
            }
        }
    }
}