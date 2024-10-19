using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int Value {get; private set;}

    public Card(int value)
    {
        Value = value;
    }
}

