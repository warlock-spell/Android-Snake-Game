using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// events enable classes or objects to notify other classes or objects when something interesting occurs
// publisher class - the class that raises the events
// subscriber classes - the classes that listens to raised events

public class SwipeControls : MonoBehaviour
{
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minimumDistance = 10;      // to differentiate between tap and swipe

    public static event System.Action<SwipeDirection> OnSwipe = delegate { };  // System.Action is a pre-made delegate, event specifies that only this class can envoke the Event method

    public enum SwipeDirection
    {
        Up, Down, Left, Right
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // storing the position of touches
                swipeStart = touch.position;

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                // after swipe has ended, we process the stored information
                ProcessSwipe();
            }
        }

        // mouse touch simulation
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            ProcessSwipe();
        }
    }


    void ProcessSwipe()
    {
        float distance = Vector2.Distance(swipeStart, swipeEnd);

        if (distance > minimumDistance)
        {
            if (IsVerticalSwipe())
            {
                // to check swipe is Up or Down
                if (swipeEnd.y > swipeStart.y)
                {
                    //up swipe
                    OnSwipe(SwipeDirection.Up);
                }
                else
                {
                    //down swipe
                    OnSwipe(SwipeDirection.Down);
                }

            }
            else // horizontal swipe
            {
                if (swipeEnd.x > swipeStart.x)
                {
                    //right swipe
                    OnSwipe(SwipeDirection.Right);
                }
                else
                {
                    //left swipe
                    OnSwipe(SwipeDirection.Left);
                }

            }

        }
    }


    bool IsVerticalSwipe()
    {
        // applying simple coordinate geometry
        float vertical = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float horizontal = Mathf.Abs(swipeEnd.x - swipeStart.x);
        if (vertical > horizontal)
            return true;
        return false;
    }
}
