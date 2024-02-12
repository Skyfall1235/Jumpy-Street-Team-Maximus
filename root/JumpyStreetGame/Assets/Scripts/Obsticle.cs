using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsticle : MonoBehaviour
{
    public Obstruction obstructionType;
    
}

public enum Obstruction
{
    /// <summary>
    /// and object that does not hurt the player but doesnt allow the player to move into itself
    /// </summary>
    StaticObstruction,
    /// <summary>
    /// an object that can kill the player that does not move
    /// </summary>
    StaticHazard,
    /// <summary>
    /// an object that can kill the player  which moves on/across a tile
    /// </summary>
    DynamicHazard
}
