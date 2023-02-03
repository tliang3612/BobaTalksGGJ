using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpable
{
    void OnPlayerJumpedOn();
    bool CanBeJumpedOn { get; }
}
