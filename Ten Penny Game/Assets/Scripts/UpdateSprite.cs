using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private TenPenny tenPenny;
    // Start is called before the first frame update
    void Start()
    {
        GameDeck sampleDeck = new GameDeck();
        tenPenny = FindObjectOfType<TenPenny>();

        int i = 0;
        foreach (string card in sampleDeck.cardList)
        {
            if (this.name == card)
            {
                cardFace = tenPenny.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable.faceUp == true)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    public void SetColour(Color colour)
    {
        spriteRenderer.color = colour;
    }
}
