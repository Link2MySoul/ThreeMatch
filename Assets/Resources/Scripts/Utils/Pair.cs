using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

[Serializable]
sealed public class Pair<TLeft, TRight>
{
    [SerializeField]
    private readonly TLeft _left;
    [SerializeField]
    private TRight _right;

    public Pair(TLeft left, TRight right)
    {
        _left = left;
        _right = right;
    }

    public TLeft Left
    {
        get { return _left; }
    }

    public TRight Right
    {
        get { return _right; }
        set { _right = value; }
    }

    public override int GetHashCode()
    {
        int leftHash = (_left == null ? 0 : _left.GetHashCode());
        int rightHash = (_right == null ? 0 : _right.GetHashCode());

        return leftHash ^ rightHash;
    }

    public override bool Equals(object obj)
    {
        var other = obj as Pair<TLeft, TRight>;

        if (other == null)
            return false;

        return Equals(_left, other._left) && Equals(_right, other._right);
    }
}