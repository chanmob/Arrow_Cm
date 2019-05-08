using UnityEngine;
using System.Collections;

public class RightButton : MonoBehaviour
{
    public Player player;

    private bool IsPressed;
    private bool RightSide;
    private bool LeftSide;

    private int StoredTouchID;

    void Start()
    {
        IsPressed = false;
        RightSide = false;
        LeftSide = false;
    }

    void Update()
    {
        if (IsPressed == true)
        {
            if (RightSide == true && TouchCamera.DragPos[StoredTouchID].x < this.transform.position.x)
            {
                RightSide = false;
                LeftSide = true;

                player.JumpButtonRelease();
                player.SitButtonPress();
            }

            else if (RightSide == false && TouchCamera.DragPos[StoredTouchID].x > this.transform.position.x)
            {
                RightSide = true;
                LeftSide = false;

                player.SitButtonRelease();
                player.JumpButtonPress();
            }
        }
    }



    public void OnPress_IE(int TouchID)
    {
        StoredTouchID = TouchID;

        IsPressed = true;

        if (TouchCamera.inputHitPos[StoredTouchID].x < this.transform.position.x)
        {
            LeftSide = true;
            player.SitButtonPress();
        }
        else
        {
            RightSide = true;
            player.JumpButtonPress();
        }

    }

    public void OnRelease_IE(int TouchID)
    {
        if (TouchCamera.inputHitPos[StoredTouchID].x < this.transform.position.x)
        {
            LeftSide = false;
            player.SitButtonRelease();
        }

        else
        {
            RightSide = false;
            player.JumpButtonRelease();
        }

        if (RightSide == false && LeftSide == false)
        {
            IsPressed = false;
        }
    }

}
