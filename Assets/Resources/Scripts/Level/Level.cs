using UnityEngine;
using System.Collections;

public enum LevelObjective
{
    Occupy,
    Eliminate
}

public class Level:MonoBehaviour  {

    public int step;
    [HideInInspector]
    public int id;
    public int[] pieceIndex;
    public BoardDirection[] moveDirection;
}

public class LevelGuide
{
    public BoardDirection direction;
    public int PieceIndex;
}
