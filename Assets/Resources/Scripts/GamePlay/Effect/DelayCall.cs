using UnityEngine;
using System.Collections;
using System;

public class DelayCall : TimeEffect {
	public object data;
    public void Init(float delay, object p, Action<object> callback = null)
	{

        TimeEffect.effects += DelayCallUpdate;
		data = p;
		onCompleteCallbackWithParam = callback;
		progress = new Counter (delay);
	}
    public void Init(float delay, Action callback = null)
	{
        TimeEffect.effects += DelayCallUpdate;
		onCompleteCallback = callback;
		progress = new Counter (delay);
	}
    public void Stop()
    {
        TimeEffect.effects -= DelayCallUpdate;
        progress.Reset();
    }
	public void DelayCallUpdate()
	{
		progress.Tick (Time.deltaTime);
		if (progress.Expired ()) {
            TimeEffect.effects -= DelayCallUpdate;
			if(onCompleteCallbackWithParam!=null)onCompleteCallbackWithParam(data);
			if(onCompleteCallback!=null)onCompleteCallback();
            data = null;
		} 
	}
}
