﻿using UnityEngine;
using System.Collections;

public class Maze : Entity {

    private SpriteRenderer render;
	private Counter life = new Counter(5f);
    private Counter blink = new Counter(5f);
	private FadeAway fadeAway;
	private FadeIn fadeIn;
	void Awake () {
        Init();
	}
    public override void Init()
    {
        render = this.GetComponent<SpriteRenderer>();
    }
    public Maze SetUp(Hexagon hexagon, bool isUpper)
    {
		if (fadeAway!=null)fadeAway.Cancel ();
        this.transform.parent = hexagon.transform.parent;
        this.transform.localPosition = isUpper ? hexagon.upPosition : hexagon.lowPosition;
        this.transform.localScale = new Vector3( .85f,  .85f, 1f);
        this.transform.localPosition += Vector3.forward;
        this.transform.localEulerAngles = isUpper ? Vector3.zero : new Vector3(0, 0, 180f);
        render.color = new Color32(255, 255, 255, 255);
		life.Reset ();
		if (fadeIn != null)fadeIn.Cancel ();
						
		fadeIn = new FadeIn ();
		fadeIn.Init (this.gameObject, .3f, null);

            
        return this;
    }
    void Update()
    {
        blink.Tick(Time.deltaTime);
        if (blink.Expired()) blink.Reset();
        float ratio = Mathf.Sin(Mathf.PI * blink.percent);
        int alpha = (byte)128 + (byte)(127f * ratio);
        render.color = new Color32(255, 255, 255, (byte)alpha);
    }
    public void ShutDown()
    {
		fadeAway = new FadeAway ();
		fadeAway.Init(this.gameObject, .2f, Dispose);
    }

    private void Dispose(object obj)
    {
		if (fadeAway!=null)fadeAway.Cancel ();
		EntityPool.Instance.Reclaim(this.gameObject, "Maze");
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
