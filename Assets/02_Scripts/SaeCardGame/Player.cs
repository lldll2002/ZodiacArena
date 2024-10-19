using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> Hand { get; private set; }
    public int LossCount { get; set; }

    public Player()
    {
        Hand = new List<Card>();
        LossCount = 0;
    }

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }

    public void RemoveCard(Card card)
    {
        Hand.Remove(card);
    }
}
