﻿using UnityEngine;
using System.Collections;
using System;
public class FadeIn : TimeEffect {
	
	private SpriteRenderer render;
	private byte r;
	private byte g;
	private byte b;
    private byte a;
	public FadeIn Init(GameObject target,float time, Action<object> callback )
	{
        //Debug.LogError("Init " + target.name);
		render = target.GetComponent<SpriteRenderer> ();
        if (render != null )
        {
            progress = new Counter(time);
            a = (byte)(render.color.a*255);

            r = (byte)(render.color.r * 255);
            g = (byte)(render.color.g * 255);
            b = (byte)(render.color.b * 255);

			render.color = new Color32(r,g,b, 0);
			onCompleteCallbackWithParam = callback;
            TimeEffect.effects += FadeInUpdate;
		}
		return this;
	}
	public void Cancel()
	{
		if(render!=null)render.color = new Color32(r, g, b, 255);
		Stop ();
	}
	public void Stop()
	{
        TimeEffect.effects -= FadeInUpdate;
	}
	void FadeInUpdate ()
	{
		progress.Tick (Time.deltaTime);
		if (progress.Expired ()) {
            TimeEffect.effects -= FadeInUpdate;
			if (onCompleteCallbackWithParam != null)onCompleteCallbackWithParam (render.gameObject);
            render.color = new Color32(r, g, b, a);
            render = null;

		} else {
            float percent = Mathf.Min(Mathf.Max(0, progress.percent), 1f);
            r = (byte)(render.color.r * 255);
            g = (byte)(render.color.g * 255);
            b = (byte)(render.color.b * 255);
            render.color = new Color32(r, g, b, (byte)(percent * a));
		}
	}
}
