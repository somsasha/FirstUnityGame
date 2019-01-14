using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    // speed
    public Vector2 speed = new Vector2(15, 15);

    // movment direction
    public Vector2 direction = new Vector2(-1, 0);

    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(
            speed.x * direction.x,
            speed.y * direction.y);
    }

    private void FixedUpdate()
    {
        //object movment
        GetComponent<Rigidbody2D>().velocity = movement;
    }
}
