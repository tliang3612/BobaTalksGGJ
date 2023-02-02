using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContextable
{
    Sprite ContextSprite { get; }
    void ShowContext();
    void HideContext();

}
