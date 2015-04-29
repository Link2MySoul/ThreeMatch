using UnityEngine;

public static class MathExtensionMethods
{
	public static void SetInvalid(this Vector3 v)
	{
		v.x=float.NaN;
		v.y=float.NaN;
		v.z=float.NaN;
	}
	
	public static bool IsValid(this Vector3 v)
	{
		if(float.IsNaN(v.x)==true)
			return false;
		
		if(float.IsNaN(v.y)==true)
			return false;
		
		if(float.IsNaN(v.z)==true)
			return false;
		
		return true;
	}
	
	public static Vector3 ClosestPointOnLine2D(Vector3 p0, Vector3 p1, Vector3 pos)
	{
		Vector3	dir=pos-p0;dir.y=0.0f;
		if(dir.sqrMagnitude<Mathf.Epsilon)
			return p0;
		
		Vector3	lineDir=(p1-p0);
		lineDir.y=0.0f;
		if(lineDir.sqrMagnitude<Mathf.Epsilon)
			return p0;
		
		lineDir.Normalize();
 
    	float t = Vector3.Dot(lineDir, dir);
 
    	if (t <= Mathf.Epsilon)
        	return p0;
 
    	float d = Vector3.Distance(p0, p1);
    	if (t >= d)
            return p1;
 
		return (p0+lineDir*t);
	}
}