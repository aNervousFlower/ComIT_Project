using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    private TenPenny tenPenny;
    // Start is called before the first frame update
    void Start()
    {
        tenPenny = FindObjectOfType<TenPenny>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SortHand()
    {
        tenPenny.SortPlayerHand();
    }

    public void PlaySelectedCards()
    {
        tenPenny.PlaySelectedCards();
    }
}
