using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Vector2 dPostion;  //deltaPosition

    public BodyPart following = null;

    

    private SpriteRenderer spriteRenderer = null;

    const int PARTSREMEMBERED = 10;
    public Vector3[] previousPositions = new Vector3[PARTSREMEMBERED];

    // circular buffers
    public int setIndex = 0;
    public int getIndex = -(PARTSREMEMBERED - 1);


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (!GameController.instance.alive) return;

        Vector3 followPosition;
        if (following != null)
        {
            if (following.getIndex > -1)
                followPosition = following.previousPositions[following.getIndex];
            else
                followPosition = following.transform.position;

        }
        else
            followPosition = gameObject.transform.position;


        // we are not creating any new values, therefore the garbage collector does not kick in
        previousPositions[setIndex].x = gameObject.transform.position.x;
        previousPositions[setIndex].y = gameObject.transform.position.y;
        previousPositions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if (setIndex >= PARTSREMEMBERED) setIndex = 0;

        getIndex++;
        if (getIndex >= PARTSREMEMBERED) getIndex = 0;


        if (following != null) // not the head
        {
            Vector3 newPosition;
            if (following.getIndex > -1)
            {
                newPosition = followPosition;
            }
            else
            {
                newPosition = following.transform.position;
            }

            // for rendering purpose
            // we display each sprite with a slightly changed z coordinate so that the images look overlaped
            newPosition.z = newPosition.z + 0.01f;

            SetMovement(newPosition - gameObject.transform.position); // delta position = newPosition - currentPosition
            UpdateDirection();
            UpdatePosition();
        }



    }

    public void SetMovement(Vector2 movement)
    {
        dPostion = movement;
    }

    public void UpdatePosition()
    {
        gameObject.transform.position += (Vector3)dPostion;
    }

    public void UpdateDirection()
    {
        // up
        if (dPostion.y > 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0); // rotations are done along z axis, which is also normal of our game screen

        // down
        else if (dPostion.y < 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);


        // left
        else if (dPostion.x < 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);


        // right
        else if (dPostion.x > 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90); 

    } 

    public void TurnIntoTail()
    {
        
        spriteRenderer.sprite = GameController.instance.tailSprite;
    }

    public void TurnIntoBodyPart()
    {
        
        spriteRenderer.sprite = GameController.instance.bodySprite;
    }

    public void ResetMemory()
    {
        setIndex = 0;
        getIndex = -(PARTSREMEMBERED - 1) ;
    }
}
