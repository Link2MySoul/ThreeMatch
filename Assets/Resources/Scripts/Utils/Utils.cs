using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Core;

public class Utils : MonoBehaviour 
{
    public class Timer
    {
        bool m_isRunning;
        float m_startTime;
        float m_duration;
        float m_pauseTime;

        public override string ToString() 
        {
            return GetRemainingTime().ToString();
        }

        public float GetDuration()
        {
            return m_duration;
        }

        public float GetRemainingTime()
        {
            if (m_isRunning)
            {
                float currentTime = Time.time;
                float remainingTime = m_duration - (currentTime - m_startTime);
                return Mathf.Max(remainingTime, 0.0f);
            }
            return 0.0f;
        }
		
		public float GetProgress()
		{
			if(m_isRunning)
				return Mathf.Clamp((Time.time - m_startTime)/m_duration, 0.0f, 1.0f);
			else
				return 0.0f;
		}

        public void Start(float duration)
        {
            m_duration = duration;
            m_startTime = Time.time;
            m_isRunning = true;
        }

        public void Restart()
        {
            m_startTime = Time.time;
            m_isRunning = true;
        }

        public void Stop()
        {
            if (m_isRunning)
            {
                m_pauseTime = Time.time;
                m_isRunning = false;
            }
        }

        public void Resume()
        {
            if (!m_isRunning)
            {
                float remainingTime = m_duration - (m_pauseTime - m_startTime);
                Start(remainingTime);
            }
        }

        public bool IsRunning()
        {
            return m_isRunning;
        }

        public bool IsElapsed()
        {
            if (m_isRunning)
            {
                float currentTime = Time.time;
                bool isElapsed = (currentTime - m_startTime) > m_duration;
                if (isElapsed)
                {
                    Stop();
                }
                return isElapsed;
            }
            else
            {
                return false;
            }
        }
		
		public void Jump(float _percent)
		{
			if(m_isRunning)
			{
				float newProgress = Mathf.Clamp(GetProgress() + _percent, 0.0f, 1.0f);
				m_startTime = Time.time - newProgress * m_duration;
			}
		}
		
		public void Extend(float _extra)
		{
			if(m_isRunning)
			{
				m_duration += _extra;
			}
		}
    }

    
    public struct Pouch
    {
        int m_maxValue;
        int m_currentValue;
		int m_baseMaxValue;
        public void Init(int maxValue)
        {
            m_maxValue = maxValue;
            m_currentValue = m_maxValue;
			m_baseMaxValue = maxValue;
        }

        public void Init(int currentValue, int maxValue)
        {
            m_maxValue = maxValue;
            m_currentValue = currentValue;
			m_baseMaxValue = maxValue;
        }

        public void ReFill()
        {
            if (m_currentValue != m_maxValue)
            {
                m_currentValue = m_maxValue;
            }
        }

        public void Add(int value)
        {
            m_currentValue = Mathf.Min(m_maxValue, m_currentValue + value);
        }

        public int GetMaxValue() { return m_maxValue; }
        public int GetCurrentValue() { return m_currentValue; }
		public int GetBaseMaxValue() {return m_baseMaxValue; }

        public void SetMaxValue(int value)
        {
            if (m_maxValue != value)
            {
                m_maxValue = value;
            }
        }

        public void SetCurrentValue(int value)
        {
            value = Mathf.Min(value, m_maxValue);
            value = Mathf.Max(value, 0);
            if (m_currentValue != value)
            {
                m_currentValue = value;
            }
        }
		
		public void SetBaseMaxValue(int value)
		{
			m_baseMaxValue = value;
		}
    }


    public class PouchNetwork
    {
        bool authority = true;

        Pouch m_authorityPouch;
        Pouch m_localPouch;

        public void Init(int maxValue)
        {
            m_authorityPouch.Init(maxValue);
            m_localPouch.Init(maxValue);
        }

        public void Init(int currentValue, int maxValue)
        {
            m_authorityPouch.Init(currentValue, maxValue);
            m_localPouch.Init(currentValue, maxValue);
        }        

        public void ReFill()
        {
            if (authority)
                m_authorityPouch.ReFill();
            else
                m_localPouch.ReFill();
        }

        public void Add(int value)
        {
            if (authority)
                m_authorityPouch.Add(value);
            else
                m_localPouch.Add(value);
        }
        public int GetMaxValue() 
        {
            if (authority)
                return m_authorityPouch.GetMaxValue();
            else
                return m_localPouch.GetMaxValue();
        }
        public int GetCurrentValue() 
        {
            if (authority)
                return m_authorityPouch.GetCurrentValue();
            else
                return m_localPouch.GetCurrentValue();
        }
        public int GetBaseMaxValue()
        {
            if (authority)
                return m_authorityPouch.GetBaseMaxValue();
            else
                return m_localPouch.GetBaseMaxValue();
        }

        public void SetMaxValue(int value)
        {
            if (authority)
                m_authorityPouch.SetMaxValue(value);
            else
                m_localPouch.SetMaxValue(value);
        }

        public void SetCurrentValue(int value)
        {
            if (authority)
                m_authorityPouch.SetCurrentValue(value);
            else
                m_localPouch.SetCurrentValue(value);
        }

        public void SetBaseMaxValue(int value)
        {
            if (authority)
                m_authorityPouch.SetBaseMaxValue(value);
            else
                m_localPouch.SetBaseMaxValue(value);
        }
        public void TurnAuthorityOn()
        {
            authority = true;
        }
        public void TurnAuthorityOff()
        {
            authority = false;
            m_localPouch = m_authorityPouch;
        }
    }

	

   
	
	
	

    
    
	
	// Important: we are using this function instead of CopyFrom, for cameras attached to animated bones, as their rotation can become invalid and the CopyFrom will spam errors.
	static public void CopyCamera(ref Camera cameraIn, ref Camera cameraOut)
	{
		cameraOut.fieldOfView = cameraIn.fieldOfView;
		cameraOut.nearClipPlane = cameraIn.nearClipPlane;
		cameraOut.farClipPlane = cameraIn.farClipPlane;
		cameraOut.renderingPath = cameraIn.renderingPath;
		cameraOut.hdr = cameraIn.hdr;
		cameraOut.orthographicSize = cameraIn.orthographicSize;
		cameraOut.orthographic = cameraIn.orthographic;
		cameraOut.transparencySortMode = cameraIn.transparencySortMode;
		cameraOut.depth = cameraIn.depth;
		cameraOut.aspect = cameraIn.aspect;
		cameraOut.cullingMask = cameraIn.cullingMask;
		cameraOut.eventMask = cameraIn.eventMask;
		cameraOut.backgroundColor = cameraIn.backgroundColor;
		cameraOut.rect = cameraIn.rect;
		cameraOut.pixelRect = cameraIn.pixelRect;
		cameraOut.targetTexture = cameraIn.targetTexture;
		cameraOut.worldToCameraMatrix = cameraIn.worldToCameraMatrix;
		cameraOut.projectionMatrix = cameraIn.projectionMatrix;
		cameraOut.clearFlags = cameraIn.clearFlags;
		cameraOut.useOcclusionCulling = cameraIn.useOcclusionCulling;
		cameraOut.layerCullDistances = cameraIn.layerCullDistances;
		cameraOut.layerCullSpherical = cameraIn.layerCullSpherical;
		cameraOut.depthTextureMode = cameraIn.depthTextureMode;
		
		cameraOut.transform.position = cameraIn.transform.position;
		cameraOut.transform.rotation = cameraIn.transform.rotation;
	}

    public static float DistancePointToRay(Ray ray, Vector3 pos)
    {
        Vector3 AtoB = pos - ray.origin;
        float proj = Vector3.Dot(AtoB, ray.direction.normalized);
        Vector3 projPos = ray.origin;
        if(proj>0)
            projPos +=ray.direction.normalized * proj;
        return Vector3.Distance(projPos, pos);
    }
	
	public struct CacheFloatToInt
	{
		float m_sum;
		
		public int Add(float _delta)
		{
			m_sum += _delta;
			int integer = Mathf.FloorToInt(m_sum);
			if(integer > 0)
				m_sum -= (float)integer;
			return integer;
		}
	}
	
	public class CacheSeconds
	{
		private float m_sum = 0.0f;
		private float m_duration = 0.0f;
		private float m_startTime = 0.0f;
		
		public CacheSeconds(float _duration)
		{
			m_duration = _duration;	
		}
		
		public int Add(float _delta)
		{
			int integer = 0;
			m_sum += _delta;
			if(Time.time - m_startTime >= m_duration)
			{
				m_startTime = Time.time;
				integer = Mathf.FloorToInt(m_sum);
				if(integer > 0)
					m_sum -= (float)integer;
			}
			return integer;
		}
	}
	
	public class StaticRandom
	{
		private static float[] randomList = new float[1000]
		{
			0.139f, 0.598f, 0.485f, 0.682f, 0.814f, 0.191f, 0.935f, 0.287f, 0.450f, 0.097f, 
			0.683f, 0.976f, 0.805f, 0.957f, 0.764f, 0.214f, 0.202f, 0.608f, 0.261f, 0.929f, 
			0.745f, 0.050f, 0.133f, 0.369f, 0.535f, 0.490f, 0.991f, 0.130f, 0.366f, 0.501f, 
			0.315f, 0.005f, 0.852f, 0.913f, 0.974f, 0.061f, 0.045f, 0.640f, 0.718f, 0.432f, 
			0.785f, 0.690f, 0.902f, 0.779f, 0.604f, 0.308f, 0.092f, 0.786f, 0.942f, 0.191f, 
			0.975f, 0.408f, 0.079f, 0.019f, 0.726f, 0.088f, 0.465f, 0.116f, 0.147f, 0.324f, 
			0.199f, 0.806f, 0.921f, 0.949f, 0.783f, 0.679f, 0.124f, 0.891f, 0.983f, 0.704f, 
			0.169f, 0.562f, 0.889f, 0.829f, 0.144f, 0.056f, 0.148f, 0.354f, 0.766f, 0.228f, 
			0.398f, 0.206f, 0.188f, 0.175f, 0.526f, 0.907f, 0.925f, 0.833f, 0.187f, 0.886f, 
			0.596f, 0.929f, 0.493f, 0.395f, 0.233f, 0.979f, 0.769f, 0.953f, 0.996f, 0.925f, 
			0.184f, 0.200f, 0.802f, 0.239f, 0.022f, 0.413f, 0.519f, 0.250f, 0.875f, 0.670f, 
			0.940f, 0.110f, 0.350f, 0.919f, 0.926f, 0.993f, 0.618f, 0.746f, 0.423f, 0.876f, 
			0.753f, 0.291f, 0.704f, 0.098f, 0.107f, 0.240f, 0.069f, 0.495f, 0.596f, 0.165f, 
			0.287f, 0.196f, 0.358f, 0.840f, 0.044f, 0.198f, 0.403f, 0.949f, 0.152f, 0.114f, 
			0.388f, 0.675f, 0.957f, 0.517f, 0.946f, 0.486f, 0.579f, 0.574f, 0.469f, 0.240f, 
			0.361f, 0.023f, 0.530f, 0.120f, 0.172f, 0.893f, 0.496f, 0.705f, 0.131f, 0.782f, 
			0.344f, 0.024f, 0.654f, 0.711f, 0.383f, 0.744f, 0.752f, 0.510f, 0.876f, 0.728f, 
			0.590f, 0.669f, 0.594f, 0.309f, 0.488f, 0.078f, 0.334f, 0.187f, 0.152f, 0.621f, 
			0.661f, 0.495f, 0.026f, 0.357f, 0.804f, 0.264f, 0.666f, 0.238f, 0.668f, 0.151f, 
			0.702f, 0.698f, 0.818f, 0.370f, 0.770f, 0.364f, 0.021f, 0.246f, 0.134f, 0.202f, 
			0.734f, 0.069f, 0.698f, 0.741f, 0.649f, 0.692f, 0.429f, 0.220f, 0.632f, 0.094f, 
			0.559f, 0.309f, 0.553f, 0.510f, 0.121f, 0.848f, 0.998f, 0.297f, 0.907f, 0.119f, 
			0.078f, 0.514f, 0.842f, 0.725f, 0.026f, 0.892f, 0.852f, 0.224f, 0.539f, 0.124f, 
			0.057f, 0.087f, 0.053f, 0.950f, 0.887f, 0.593f, 0.797f, 0.284f, 0.707f, 0.112f, 
			0.835f, 0.446f, 0.606f, 0.574f, 0.592f, 0.393f, 0.146f, 0.308f, 0.249f, 0.922f, 
			0.366f, 0.724f, 0.339f, 0.014f, 0.124f, 0.220f, 0.227f, 0.973f, 0.716f, 0.758f, 
			0.840f, 0.206f, 0.681f, 0.508f, 0.037f, 0.471f, 0.255f, 0.603f, 0.623f, 0.528f, 
			0.051f, 0.714f, 0.590f, 0.796f, 0.285f, 0.632f, 0.908f, 0.267f, 0.133f, 0.633f, 
			0.746f, 0.544f, 0.975f, 0.617f, 0.758f, 0.333f, 0.748f, 0.039f, 0.826f, 0.667f, 
			0.366f, 0.824f, 0.569f, 0.949f, 0.271f, 0.629f, 0.765f, 0.097f, 0.538f, 0.121f, 
			0.138f, 0.543f, 0.685f, 0.721f, 0.871f, 0.652f, 0.133f, 0.544f, 0.730f, 0.492f, 
			0.312f, 0.960f, 0.840f, 0.083f, 0.012f, 0.297f, 0.231f, 0.064f, 0.750f, 0.703f, 
			0.723f, 0.216f, 0.776f, 0.955f, 0.163f, 0.227f, 0.145f, 0.641f, 0.524f, 0.656f, 
			0.867f, 0.811f, 0.729f, 0.012f, 0.495f, 0.203f, 0.859f, 0.254f, 0.747f, 0.106f, 
			0.200f, 0.205f, 0.310f, 0.334f, 0.356f, 0.406f, 0.984f, 0.486f, 0.937f, 0.975f, 
			0.398f, 0.701f, 0.347f, 0.835f, 0.398f, 0.921f, 0.767f, 0.730f, 0.558f, 0.644f, 
			0.507f, 0.493f, 0.020f, 0.074f, 0.625f, 0.500f, 0.230f, 0.828f, 0.588f, 0.809f, 
			0.438f, 0.977f, 0.317f, 0.644f, 0.503f, 0.518f, 0.906f, 0.442f, 0.600f, 0.757f, 
			0.243f, 0.659f, 0.703f, 0.549f, 0.257f, 0.166f, 0.464f, 0.895f, 0.614f, 0.574f, 
			0.436f, 0.129f, 0.696f, 0.103f, 0.599f, 0.707f, 0.185f, 0.236f, 0.710f, 0.856f, 
			0.175f, 0.155f, 0.897f, 0.128f, 0.537f, 0.421f, 0.384f, 0.703f, 0.332f, 0.879f, 
			0.015f, 0.325f, 0.130f, 0.801f, 0.400f, 0.848f, 0.789f, 0.470f, 0.817f, 0.459f, 
			0.036f, 0.336f, 0.214f, 0.014f, 0.373f, 0.309f, 0.281f, 0.214f, 0.045f, 0.796f, 
			0.333f, 0.671f, 0.732f, 0.744f, 0.582f, 0.088f, 0.762f, 0.575f, 0.012f, 0.836f, 
			0.782f, 0.762f, 0.563f, 0.735f, 0.559f, 0.110f, 0.999f, 0.895f, 0.703f, 0.751f, 
			0.918f, 0.715f, 0.419f, 0.251f, 0.820f, 0.840f, 0.941f, 0.883f, 0.755f, 0.228f, 
			0.140f, 0.170f, 0.657f, 0.536f, 0.546f, 0.344f, 0.594f, 0.386f, 0.219f, 0.312f, 
			0.086f, 0.214f, 0.458f, 0.981f, 0.676f, 0.274f, 0.772f, 0.479f, 0.456f, 0.263f, 
			0.310f, 0.387f, 0.511f, 0.294f, 0.877f, 0.618f, 0.252f, 0.481f, 0.924f, 0.742f, 
			0.147f, 0.879f, 0.820f, 0.784f, 0.697f, 0.612f, 0.104f, 0.027f, 0.189f, 0.215f, 
			0.517f, 0.613f, 0.675f, 0.391f, 0.666f, 0.704f, 0.257f, 0.438f, 0.575f, 0.546f, 
			0.067f, 0.462f, 0.427f, 0.492f, 0.918f, 0.753f, 0.659f, 0.364f, 0.658f, 0.928f, 
			0.092f, 0.113f, 0.461f, 0.478f, 0.165f, 0.207f, 0.394f, 0.674f, 0.284f, 0.064f, 
			0.170f, 0.745f, 0.291f, 0.241f, 0.747f, 0.696f, 0.711f, 0.120f, 0.628f, 0.173f, 
			0.362f, 0.813f, 0.906f, 0.379f, 0.674f, 0.685f, 0.452f, 0.328f, 0.866f, 0.944f, 
			0.953f, 0.741f, 0.369f, 0.261f, 0.122f, 0.404f, 0.152f, 0.198f, 0.225f, 0.459f, 
			0.310f, 0.583f, 0.154f, 0.511f, 0.376f, 0.322f, 0.171f, 0.186f, 0.745f, 0.222f, 
			0.042f, 0.539f, 0.735f, 0.485f, 0.566f, 0.279f, 0.207f, 0.082f, 0.803f, 0.480f, 
			0.755f, 0.066f, 0.808f, 0.340f, 0.111f, 0.429f, 0.376f, 0.029f, 0.119f, 0.343f, 
			0.544f, 0.514f, 0.895f, 0.169f, 0.863f, 0.779f, 0.659f, 0.395f, 0.002f, 0.352f, 
			0.514f, 0.265f, 0.583f, 0.644f, 0.902f, 0.414f, 0.005f, 0.884f, 0.695f, 0.843f, 
			0.197f, 0.070f, 0.394f, 0.746f, 0.704f, 0.244f, 0.775f, 0.814f, 0.399f, 0.948f, 
			0.946f, 0.142f, 0.068f, 0.403f, 0.678f, 0.528f, 0.644f, 0.566f, 0.622f, 0.787f, 
			0.620f, 0.812f, 0.080f, 0.451f, 0.966f, 0.490f, 0.483f, 0.164f, 0.439f, 0.698f, 
			0.423f, 0.493f, 0.334f, 0.275f, 0.146f, 0.474f, 0.119f, 0.149f, 0.465f, 0.619f, 
			0.004f, 0.845f, 0.996f, 0.055f, 0.406f, 0.372f, 0.197f, 0.181f, 0.967f, 0.374f, 
			0.770f, 0.439f, 0.261f, 0.908f, 0.223f, 0.385f, 0.990f, 0.943f, 0.780f, 0.214f, 
			0.762f, 0.611f, 0.375f, 0.702f, 0.524f, 0.453f, 0.808f, 0.792f, 0.257f, 0.203f, 
			0.409f, 0.495f, 0.101f, 0.004f, 0.783f, 0.776f, 0.816f, 0.025f, 0.045f, 0.594f, 
			0.292f, 0.302f, 0.197f, 0.065f, 0.928f, 0.984f, 0.231f, 0.426f, 0.053f, 0.761f, 
			0.485f, 0.176f, 0.370f, 0.251f, 0.857f, 0.393f, 0.469f, 0.294f, 0.531f, 0.953f, 
			0.564f, 0.405f, 0.924f, 0.763f, 0.965f, 0.275f, 0.338f, 0.258f, 0.126f, 0.447f, 
			0.569f, 0.964f, 0.898f, 0.186f, 0.922f, 0.460f, 0.415f, 0.310f, 0.774f, 0.540f, 
			0.222f, 0.382f, 0.739f, 0.496f, 0.719f, 0.233f, 0.125f, 0.850f, 0.147f, 0.390f, 
			0.306f, 0.522f, 0.495f, 0.092f, 0.029f, 0.887f, 0.378f, 0.434f, 0.100f, 0.653f, 
			0.647f, 0.973f, 0.300f, 0.606f, 0.192f, 0.520f, 0.278f, 0.184f, 0.329f, 0.397f, 
			0.978f, 0.159f, 0.520f, 0.991f, 0.731f, 0.182f, 0.666f, 0.428f, 0.044f, 0.732f, 
			0.150f, 0.487f, 0.111f, 0.735f, 0.141f, 0.047f, 0.469f, 0.806f, 0.157f, 0.035f, 
			0.081f, 0.981f, 0.210f, 0.121f, 0.893f, 0.248f, 0.082f, 0.134f, 0.305f, 0.198f, 
			0.954f, 0.941f, 0.521f, 0.750f, 0.412f, 0.147f, 0.002f, 0.504f, 0.361f, 0.847f, 
			0.221f, 0.951f, 0.389f, 0.368f, 0.503f, 0.160f, 0.862f, 0.564f, 0.465f, 0.144f, 
			0.051f, 0.473f, 0.027f, 0.294f, 0.316f, 0.997f, 0.949f, 0.400f, 0.098f, 0.483f, 
			0.100f, 0.532f, 0.123f, 0.846f, 0.778f, 0.209f, 0.966f, 0.362f, 0.323f, 0.993f, 
			0.046f, 0.965f, 0.445f, 0.310f, 0.813f, 0.131f, 0.592f, 0.842f, 0.618f, 0.733f, 
			0.386f, 0.518f, 0.669f, 0.160f, 0.147f, 0.481f, 0.915f, 0.227f, 0.434f, 0.415f, 
			0.199f, 0.602f, 0.406f, 0.877f, 0.747f, 0.689f, 0.828f, 0.543f, 0.590f, 0.295f, 
			0.194f, 0.501f, 0.241f, 0.472f, 0.099f, 0.086f, 0.027f, 0.717f, 0.481f, 0.185f, 
			0.405f, 0.106f, 0.782f, 0.365f, 0.097f, 0.582f, 0.863f, 0.963f, 0.699f, 0.297f, 
			0.294f, 0.739f, 0.927f, 0.908f, 0.794f, 0.444f, 0.560f, 0.568f, 0.563f, 0.124f, 
			0.303f, 0.049f, 0.253f, 0.323f, 0.539f, 0.885f, 0.276f, 0.197f, 0.061f, 0.062f, 
			0.454f, 0.198f, 0.746f, 0.249f, 0.011f, 0.093f, 0.820f, 0.041f, 0.780f, 0.165f, 
			0.826f, 0.865f, 0.845f, 0.293f, 0.001f, 0.455f, 0.573f, 0.334f, 0.678f, 0.655f, 
			0.526f, 0.459f, 0.154f, 0.357f, 0.882f, 0.356f, 0.853f, 0.458f, 0.826f, 0.212f, 
			0.306f, 0.667f, 0.902f, 0.637f, 0.843f, 0.096f, 0.992f, 0.678f, 0.897f, 0.702f, 
			0.623f, 0.519f, 0.788f, 0.398f, 0.298f, 0.438f, 0.204f, 0.960f, 0.323f, 0.084f, 
			0.312f, 0.943f, 0.519f, 0.406f, 0.536f, 0.995f, 0.044f, 0.389f, 0.368f, 0.655f, 
			0.239f, 0.362f, 0.215f, 0.568f, 0.859f, 0.159f, 0.963f, 0.208f, 0.450f, 0.905f, 
			0.463f, 0.895f, 0.437f, 0.976f, 0.032f, 0.007f, 0.672f, 0.755f, 0.059f, 0.444f, 
			0.152f, 0.894f, 0.136f, 0.742f, 0.900f, 0.363f, 0.148f, 0.495f, 0.101f, 0.848f, 
			0.052f, 0.603f, 0.310f, 0.242f, 0.463f, 0.262f, 0.304f, 0.220f, 0.539f, 0.139f
		};

		
		public static float Get(int _index)
		{
			return randomList[_index % randomList.Length];
		}
	}

    public static string GetPath(GameObject go)
    {
        var path = "null";
        if (null != go)
        {
            path = go.name;
            while (null != go.transform.parent)
            {
                go = go.transform.parent.gameObject;
                path = go.name + "/" + path;
            }
        }
        return path;
    }


    private const string CLONE_MARKER = "(Clone)";
    private static string GetWithoutCloneMarker(string s)
    {
        string result = null;
        if (null != s && s.EndsWith(CLONE_MARKER))
        {
            result = s.Substring(0, s.Length - CLONE_MARKER.Length);
        }
        return result;
    }

    public static string RemoveCloneMarkerFrom(string s)
    {
        var removedCloneMarker = GetWithoutCloneMarker(s);
        return removedCloneMarker ?? s;
    }

    public static bool RemoveCloneMarkerFromName(Object obj)
    {
        var replacement = RemoveCloneMarkerFrom(obj.name);
        bool replace = (null != replacement);
        if (replace)
        {
            obj.name = replacement;
        }
        return replace;
    }

    public static void Reparent(GameObject child, GameObject parent)
    {
       
        var transform = child.transform;
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    

    public static bool InCameraView(Camera _camera, Vector3 _point)
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(_point);
        if (screenPos.x < 0.0f || screenPos.x > Screen.width || screenPos.y < 0.0f || screenPos.y > Screen.height || screenPos.z < 0.0f)
            return false;
        return true;
    }

    
}

