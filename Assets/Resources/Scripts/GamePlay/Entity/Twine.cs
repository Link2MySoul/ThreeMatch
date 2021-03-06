﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Twine : Entity {
    private Transform left;
    private Transform right;
    private Transform verticle;

	private Transform up_left;
	private Transform up_right;
	private Transform up_verticle;

	private Transform down_left;
	private Transform down_right;
	private Transform down_verticle;

    public int life = 3;
    public Piece piece;
    public int state = 0;
	private List<FadeIn> fadeIns;
	private List<FadeAway> fadeAways;
    void Awake()
    {
		Init ();
    }
    public override void Init()
	{
		fadeIns = new List<FadeIn> ();
		fadeAways = new List<global::FadeAway> ();
		Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);
		foreach (var child in children)
		{
			
			if (child.name.Contains("up_left")) up_left = child;
			if (child.name.Contains("up_right")) up_right = child;
			if (child.name.Contains("up_verticle")) up_verticle = child;
			if (child.name.Contains("down_left")) down_left = child;
			if (child.name.Contains("down_right")) down_right = child;
			if (child.name.Contains("down_verticle")) down_verticle = child;
			
			//Debug.Log("up_left "+up_left);
			
		}
	}
    public override void Reset()
    {
        base.Reset();
		ResetRope ();
        life = 3;
    }

    public void SetState(int s)
    {
        state = s;
        life = 0;
        ResetRope();
        if ((state & (int)HexagonEdget.UpperLeft) != 0)
        {
            up_left.gameObject.SetActive(true);
            life++;
        }
        if ((state & (int)HexagonEdget.UpperRight) != 0)
        {
            up_right.gameObject.SetActive(true);
            life++;
        }
        if ((state & (int)HexagonEdget.UpperDown) != 0)
        {
            up_verticle.gameObject.SetActive(true);
            life++;
        }
        if ((state & (int)HexagonEdget.DownLeft) != 0)
        {
            down_left.gameObject.SetActive(true);
            life++;
        }
        if ((state & (int)HexagonEdget.DownRight) != 0)
        {
            down_right.gameObject.SetActive(true);
            life++;
        }
        if ((state & (int)HexagonEdget.DownUp) != 0)
        {
            down_verticle.gameObject.SetActive(true);
            life++;
        }

    }

	private void ResetRope()
	{

		if (up_left != null)
		{

			up_left.gameObject.SetActive(false);
			up_right.gameObject.SetActive(false);
			up_verticle.gameObject.SetActive(false);
			
			down_left.gameObject.SetActive(false);
			down_right.gameObject.SetActive(false);
			down_verticle.gameObject.SetActive(false);

            Color opacity = new Color(1f, 1f, 1f, 1f);
            up_left.GetComponent<SpriteRenderer>().color = opacity;
            up_right.GetComponent<SpriteRenderer>().color = opacity;
            up_verticle.GetComponent<SpriteRenderer>().color = opacity;
            down_left.GetComponent<SpriteRenderer>().color = opacity;
            down_right.GetComponent<SpriteRenderer>().color = opacity;
            down_verticle.GetComponent<SpriteRenderer>().color = opacity;
		}
	}
	private void SetupRope()
	{
        if (left != null) left.gameObject.SetActive(true);
        if (right != null) right.gameObject.SetActive(true);
        if (verticle != null) verticle.gameObject.SetActive(true);
		fadeIns.Clear ();
		fadeIns.Add (new FadeIn ().Init (left.gameObject, .3f, null));
		fadeIns.Add (new FadeIn ().Init (right.gameObject, .3f, null));
		fadeIns.Add (new FadeIn ().Init (verticle.gameObject, .3f, null));

	}
    public void ShutDown()
    {
        EntityPool.Instance.Reclaim(this.gameObject, "Twine");
		foreach (var i in fadeAways) {
			i.Cancel();
		}
		fadeAways.Clear ();
        life = 0;
        piece = null;
		ResetRope ();
    }
	private void OnFadeAway(object target)
	{
		GameObject g = target as GameObject;
		if (g != null) {
			g.SetActive(false);
		}
	}
	public void FadeAway(object target)
	{
		GameObject rope = target as GameObject;
		    
		fadeAways.Add(new FadeAway().Init(rope,.2f,OnFadeAway));
		
	}
    public void OnPass(BoardDirection direction, float time)
    {
		//Debug.LogWarning ("OnPass " + direction);
	    
        switch (direction)
        {
            case BoardDirection.BottomLeft:
            case BoardDirection.TopRight:
				if(piece.isUpper)
				{
					if (left.gameObject.activeInHierarchy)
					{
						life--;
						//new FadeAway().Init(left.gameObject,.2f,OnFadeAway);
						new DelayCall().Init(time,left.gameObject,FadeAway);
					}
				}
				else
				{
					if (right.gameObject.activeInHierarchy)
					{
						life--;
						//new FadeAway().Init(right.gameObject,.2f,OnFadeAway);
						new DelayCall().Init(time,right.gameObject,FadeAway);
					}
				}
                break;
            case BoardDirection.BottomRight:
            case BoardDirection.TopLeft:
				if(piece.isUpper)
				{
					if (right.gameObject.activeInHierarchy)
					{
						life--;
						//new FadeAway().Init(right.gameObject,.2f,OnFadeAway);
						new DelayCall().Init(time,right.gameObject,FadeAway);
					}
				}
			else
			{
				if (left.gameObject.activeInHierarchy)
				{
					life--;
					//new FadeAway().Init(left.gameObject,.2f,OnFadeAway);
					new DelayCall().Init(time,left.gameObject,FadeAway);
				}
			}
               
                break;
            case BoardDirection.Left:
            case BoardDirection.Right:
                if (verticle.gameObject.activeInHierarchy)
                {
                    life--;
					//new FadeAway().Init(verticle.gameObject,.2f,OnFadeAway);
					new DelayCall().Init(time,verticle.gameObject,FadeAway);
                }
                break;

        }

						
        
    }
	private void ResetAnimation()
	{
		foreach (var i in fadeIns) {
			i.Cancel();
		}
		fadeIns.Clear ();
		foreach (var i in fadeAways) {
			i.Cancel();
		}
		fadeAways.Clear ();
	}
    public void SetUp(Piece p)
    {
        piece = p;
        piece.twine = this;
		ResetRope ();
        this.transform.parent = piece.transform.parent;
        this.transform.localPosition = piece.transform.localPosition;
        this.transform.localScale = new Vector3(piece.scale, piece.scale, piece.scale);
        state = 0;
		ResetAnimation ();
        if (!piece.isUpper)
        {
			left = down_left;
			right = down_right;
			verticle = down_verticle;
            state |= (int)HexagonEdget.DownLeft;
            state |= (int)HexagonEdget.DownRight;
            state |= (int)HexagonEdget.DownUp;

        }
        else
        {
			left = up_left;
			right = up_right;
			verticle = up_verticle;
            state |= (int)HexagonEdget.UpperLeft;
            state |= (int)HexagonEdget.UpperRight;
            state |= (int)HexagonEdget.UpperDown;
            
        }
		    
		SetupRope ();
        this.transform.localPosition -= Vector3.forward;
    }
    void Update()
    {
        if (piece != null)
        {
            this.transform.localPosition = piece.transform.localPosition;
            this.transform.localPosition -= Vector3.forward;
        }
        
    }
    
}
