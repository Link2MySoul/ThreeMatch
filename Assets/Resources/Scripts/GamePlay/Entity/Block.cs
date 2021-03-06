﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Block : Entity {

	private Transform up_left;
	private Transform up_right;
	private Transform up_down;
	
	private Transform down_left;
	private Transform down_right;
	private Transform down_up;
	private Counter life = new Counter(5f);
	private List<Transform> childs;
	private List<FadeAway> fadeAways;
	private List<FadeIn> fadeIns;
	private Color32 defaultColor;
	void Awake () {
        Init();
	}
    public override void Init()
    {
        Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);
        childs = new List<Transform>();
		fadeAways = new List<FadeAway> ();
		fadeIns = new List<FadeIn> ();
        foreach (var child in children)
        {

            if (child.name.Contains("UpLeft"))
            {
                up_left = child; 
				defaultColor = up_left.GetComponent<SpriteRenderer>().color;
                childs.Add(child);
            }
            if (child.name.Contains("UpRight"))
            {
                up_right = child; 
                childs.Add(child);
            }
            if (child.name.Contains("UpDown"))
            {
                up_down = child; 
                childs.Add(child);
            }
            if (child.name.Contains("DownLeft"))
            {
                down_left = child;
                childs.Add(child);
            }
            if (child.name.Contains("DownRight"))
            {
                down_right = child;
                childs.Add(child);
            }
            if (child.name.Contains("DownUp"))
            {
                down_up = child;
                childs.Add(child);
            }

        }
            
    }
	public void SetUp(Hexagon hexagon)
	{
		this.transform.parent = hexagon.transform.parent;
		this.transform.localPosition = hexagon.transform.localPosition;

		ResetBlock ();

        if (hexagon.blockState == 0)
        {
            ShutDown();
            return;
        }


		up_left.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.UpperLeft)!=0);
		up_right.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.UpperRight)!=0);
		up_down.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.UpperDown)!=0);
		down_left.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.DownLeft)!=0);
		down_right.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.DownRight)!=0);
		down_up.gameObject.SetActive ((hexagon.blockState&(int)HexagonEdget.DownUp)!=0);
      	/*
		foreach (var child in childs) {
			if(child.gameObject.activeInHierarchy)
			{

				FadeIn fadeIn = new FadeIn();
				fadeIn.Init(child.gameObject,.2f,null);
				fadeIns.Add(fadeIn);
			}
		}
		*/
		life.Reset ();

        
	}
	private void ResetBlock()
	{
		while (fadeAways.Count>0) {
			FadeAway fadeAway = fadeAways[0];
			fadeAway.Cancel();
			fadeAways.RemoveAt(0);
		}
		while (fadeIns.Count>0) {
			FadeIn fadeIn = fadeIns[0];
			fadeIn.Cancel();
			fadeIns.RemoveAt(0);
		}
		fadeAways.Clear ();
		fadeIns.Clear ();
		up_left.GetComponent<SpriteRenderer> ().color = defaultColor;
		up_right.GetComponent<SpriteRenderer> ().color = defaultColor;
		up_down.GetComponent<SpriteRenderer> ().color = defaultColor;
		down_left.GetComponent<SpriteRenderer> ().color = defaultColor;
		down_right.GetComponent<SpriteRenderer> ().color = defaultColor;
		down_up.GetComponent<SpriteRenderer> ().color = defaultColor;
	}
	public void OnFadeAway(object child)
	{

	}

	public void ShutDown()
	{
		EntityPool.Instance.Reclaim(this.gameObject, "Block");
	}

	public void Tick()
	{
		life.Tick (1f);
	}
	public bool Expired()
	{
		return life.Expired ();
	}
}
