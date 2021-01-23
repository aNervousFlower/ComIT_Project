using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public RaycastHit2D mouseDownHit;
    public RaycastHit2D mouseUpHit;
    public RaycastHit2D lastMouseUpHit;
    private TenPenny tenPenny;
    private float timer;
    private static float doubleClickTime = 0.3f;
    private int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.tenPenny = FindObjectOfType<TenPenny>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.clickCount == 1)
        {
            this.timer += Time.deltaTime;
        }
        else if (this.clickCount == 3)
        {
            this.timer = 0;
            this.clickCount = 1;
        }

        if (this.timer > doubleClickTime)
        {
            this.timer = 0;
            this.clickCount = 0;
        }

        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.clickCount++;
            this.mouseDownHit = UserInput.GetHit();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.lastMouseUpHit = this.mouseUpHit;
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
                    else if (this.mouseUpHit.collider.CompareTag("DiscardPile"))
                    {
                        DiscardPile();
                    }
                }
                else
                {
                    DragClick();
                }
            }
        }
    }

    private bool IsDoubleClick()
    {
        if (this.timer < doubleClickTime && this.clickCount == 2 &&
            this.mouseUpHit.collider.ToString() == this.lastMouseUpHit.collider.ToString())
        {
            // print("Double-Click");
            return true;
        }
        else
        {
            return false;
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
        // print("clicked on Deck");

        this.tenPenny.DrawCardToPlayerHand();
    }

    void PlayerHand()
    {
        // print("clicked on PlayerHand");

        if (IsDoubleClick())
        {
            this.tenPenny.DiscardCardFromPlayerHand(this.mouseUpHit.collider.name);
        }
        else
        {
            this.tenPenny.SelectPlayerCard(this.mouseUpHit.collider.gameObject);
        }
    }

    void DiscardPile()
    {
        // print("clicked on DiscardPile");

        if (IsDoubleClick())
        {
            this.tenPenny.BuyCard();
        }
    }

    private void DragClick()
    {
        // print("Dragging Click");
        // print("Down: " + this.mouseDownHit.collider.ToString());
        // print("Up: " + this.mouseUpHit.collider.ToString());

        if (this.mouseDownHit.collider.CompareTag("PlayerHand") &&
            this.mouseUpHit.collider.CompareTag("DiscardPile"))
            {
                this.tenPenny.DiscardCardFromPlayerHand(this.mouseDownHit.collider.name);
            }
    }
}
