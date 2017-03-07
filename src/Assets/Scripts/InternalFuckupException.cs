using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalFuckupException : UnityException
{
    public InternalFuckupException() : base() { }
    public InternalFuckupException(string message) : base(message)
    {
    }
}
