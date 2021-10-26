using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoryControl
{
    bool ActionNow { get; }
    void Skip();
}
