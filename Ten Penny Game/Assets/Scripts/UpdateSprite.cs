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
        this.tenPenny = FindObjectOfType<TenPenny>();

        int i = 0;
        foreach (string card in sampleDeck.cardList)
        {
            if (this.name == card)
            {
                this.cardFace = this.tenPenny.cardFaces[i];
                break;
            }
            i++;
        }
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.selectable.faceUp == true)
        {
            this.spriteRenderer.sprite = this.cardFace;
        }
        else
        {
            this.spriteRenderer.sprite = this.cardBack;
        }
    }

    public void SetColour(Color colour)
    {
        this.spriteRenderer.color = colour;
    }

    public Color GetColour()
    {
        return this.spriteRenderer.color;
    }
}
