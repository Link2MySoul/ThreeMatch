﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class MoveBy :TimeEffect {
	public Piece piece;
	private Vector3 initPosition;
	private Vector3 finalPosition;
	public Vector2 delta;
	public virtual void Init(Piece p, Vector3 targetPosition, float time, Action callback = null)
	{
        TimeEffect.effects += MoveByUpdate;
		piece = p;

		initPosition = piece.transform.localPosition;
		finalPosition = targetPosition;
		delta = finalPosition - initPosition;
		progress = new Counter (time);
		progress.Reset ();
		onCompleteCallback = callback;
	}
    public virtual void Init(Piece p, Vector3 targetPosition, float time, Action<object> callback = null)
	{
		Init (p, targetPosition, time, this.onCompleteCallback);
		onCompleteCallbackWithParam = callback;
	}
	public void MoveByUpdate()
	{
		progress.Tick (Time.deltaTime);
		if (progress.Expired ()) {
			if(piece!=null)piece.transform.localPosition = finalPosition;
            TimeEffect.effects -= MoveByUpdate;
			if (onCompleteCallback != null)onCompleteCallback ();
			if (onCompleteCallbackWithParam != null)onCompleteCallbackWithParam (this);
            piece = null;
		} else {
			
			if(piece!=null)
			{
				piece.transform.localPosition = initPosition+(finalPosition - initPosition)*progress.percent;
				if(piece.isDead||piece.isFadeAway)
				{
                    TimeEffect.effects -= MoveByUpdate;
					if(onCompleteCallback!=null)onCompleteCallback();
					if (onCompleteCallbackWithParam != null)onCompleteCallbackWithParam (this);
                    piece = null;
				}
			}
			else
			{
				progress.percent = 1f;
			}
		}
	}
}
