using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum NetworkInputButtons
{
    LeftArrow,
    RightArrow,
    Space
}
public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;
    public NetworkButtons Buttons;

}
