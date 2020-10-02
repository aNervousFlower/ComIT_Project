using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public RaycastHit2D mouseDownHit;
    public RaycastHit2D mouseUpHit;
    private TenPenny tenPenny;
    // Start is called before the first frame update
    void Start()
    {
        tenPenny = FindObjectOfType<TenPenny>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.mouseDownHit = UserInput.GetHit();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.mouseUpHit = UserInput.GetHit();
            if (this.mouseUpHit && this.mouseDownHit)
            {
                if (this.mouseUpHit.collider.ToString() == this.mouseDownHit.collider.ToString())
                {
                    if (this.mouseUpHit.collider.CompareTag("Deck"))
                    {
                        Deck();
                    }
                    else if (this.mouseUpHit.collider.CompareTag("PlayerHand"))
                    {
                        PlayerHand();
                    }
                }
                else
                {
                    DragClick();
                }
            }
        }
    }

    private static RaycastHit2D GetHit()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
        return Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    }

    void Deck()
    {
        print("clicked on Deck");

        tenPenny.DrawCardToPlayerHand();
    }

    void PlayerHand()
    {
        print("clicked on PlayerHand");
    }

    private void DragClick()
    {
        print("Dragging Click");
        print("Down: " + this.mouseDownHit.collider.ToString());
        print("Up: " + this.mouseUpHit.collider.ToString());
    }
}
