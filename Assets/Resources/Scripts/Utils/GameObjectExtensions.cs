using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Remoting;

//You can add extension methods to an existing class, but they cannot override existing methods.
//so you can't re-write GetComponent<>() for GameObject class

public static class GameObjectExtensions
{
	public static T GetComponentInParents<T>(this GameObject gameObject) where T : Component
	{
	    for(Transform t = gameObject.transform; t != null; t = t.parent)
	    {
	        T result = t.GetComponent<T>();
	        if(result != null)
	            return result;
	    }
	
	    return null;
	}
	
	public static T[] GetComponentsInParents<T>(this GameObject gameObject) where T: Component
	{
	    List<T> results = new List<T>();
	    for(Transform t = gameObject.transform; t != null; t = t.parent)
	    {
	        T result = t.GetComponent<T>();
	        if(result != null)
	            results.Add(result);
	    }
	
	    return results.ToArray();
	}
	
	public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag) where T: Component
	{		
	    List<T> results = new List<T>();	
	    if(gameObject.CompareTag(tag))
		{
			if(gameObject.GetComponent<T>()!=null)
	        	results.Add(gameObject.GetComponent<T>());
		}
	 	foreach(Transform t in gameObject.transform)
		{
			results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));
		}
	    return results.ToArray();	    
	}
	
	public static T[] GetComponentsInChildrenWithLayer<T>(this GameObject gameObject, int layerIndex) where T: Component
	{		
	    List<T> results = new List<T>();	
	    if(gameObject.layer == layerIndex)
		{
			if(gameObject.GetComponent<T>()!=null)
				results.Add(gameObject.GetComponent<T>());
		}
	 	foreach(Transform t in gameObject.transform)
		{
			results.AddRange(t.gameObject.GetComponentsInChildrenWithLayer<T>(layerIndex));
		}
	    return results.ToArray();
	}
	
	public static void SetComponentsRecursively<T>(this GameObject gameObject, bool enableComponent)where T: MonoBehaviour
	{	
	    if(gameObject.GetComponent<T>()!=null)
		{	
			gameObject.GetComponent<T>().enabled = enableComponent;
		}
	 	foreach(Transform t in gameObject.transform)
		{
			t.gameObject.SetComponentsRecursively<T>(enableComponent);
		}
	}
	
    public static void SetLayerRecursively(this GameObject gameObject, int layerIndex)
    {
        gameObject.layer = layerIndex;
	    foreach(Transform trans in gameObject.transform)
		{
			trans.gameObject.SetLayerRecursively(layerIndex);
		}
    }
	
	public static void SetColliderRecursively(this GameObject gameObject, bool enableCollider)
	{
	    Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
	    foreach(Collider collider in colliders)
		{
			collider.enabled = enableCollider;
		}		
	}
	// return the layer mask that can collider with layerToCollider 
	public static int GetCollisionMask(this GameObject gameObject, int layerToCollider = -1)
	{
	    if(layerToCollider == -1)
	        layerToCollider = gameObject.layer;
	
	    int mask = 0;
	    for(int i = 0; i < 32; i++)
	        mask |= (Physics.GetIgnoreLayerCollision(layerToCollider, i) ? 0 : 1) << i;
	
	    return mask;
	}
}
