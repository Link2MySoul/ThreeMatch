﻿using UnityEngine;
using System.Collections;
using System;
public class TurnColor : TimeEffect {
	
	private SpriteRenderer render;
	private byte r;
	private byte g;
	private byte b;
	private byte a;
	private byte sr;
	private byte sg;
	private byte sb;
	private byte sa;
	public void Init(GameObject target,float time, Color32 color,Action<object> callback )
	{
		render = target.GetComponent<SpriteRenderer> ();
		if (render != null) {
			progress = new Counter(time);
			r = color.r;
			g = color.g;
			b = color.b;
			a = color.a;
			Color32 e = render.color;
			sr =  e.r;
			sg =  e.g;
			sb =  e.b;
			sa = e.a;
			onCompleteCallbackWithParam = callback;
            TimeEffect.effects += TurnColorUpdate;
		}
	}
	
	void TurnColorUpdate ()
	{
		progress.Tick (Time.deltaTime);
		if (progress.Expired ()) {
            render.color = new Color32(r, g, b, a);
            TimeEffect.effects -= TurnColorUpdate;
			if (onCompleteCallbackWithParam != null)onCompleteCallbackWithParam (render.gameObject);
            render = null;
		} else {
			render.color = new Color32(Utility.LerpColorChannel(sr,r,progress.percent),Utility.LerpColorChannel(sg,g,progress.percent),Utility.LerpColorChannel(sb,b,progress.percent), Utility.LerpColorChannel(sa,a,progress.percent));
		}
	}

}