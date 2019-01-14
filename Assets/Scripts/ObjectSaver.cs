using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    public static ObjectSaver Instance = new ObjectSaver();

    public class SavedObject
    {
        public string spriteName;
        public Vector3 position;
        public float scale;

        public SavedObject(string sprite, Vector3 pos, float scale)
        {
            this.spriteName = sprite;
            this.position = pos;
            this.scale = scale;
        }
    }

    public List<SavedObject> platformObjects;

    public List<SavedObject> backgroundObjects;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Debug.LogError("More then one example of ObjectSaver.");
        }

        Instance = this;
        
        platformObjects = new List<SavedObject>();
    }

    public void SaveBackgroundObjects()
    {
        platformObjects = new List<SavedObject>();
        backgroundObjects = new List<SavedObject>();
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.GetComponent<SpriteRenderer>() != null && t.GetComponent<SpriteRenderer>().sprite.name.Contains("Platform"))
            {
                platformObjects.Add(new SavedObject(t.GetComponent<SpriteRenderer>().sprite.name, t.position, t.localScale.x));
            }
            else if (t.GetComponent<SpriteRenderer>() != null && t.GetComponent<SpriteRenderer>().sprite.name.Contains("Background"))
            {
                backgroundObjects.Add(new SavedObject(t.GetComponent<SpriteRenderer>().sprite.name, t.position, t.localScale.x));
            }
        }
    }
}
