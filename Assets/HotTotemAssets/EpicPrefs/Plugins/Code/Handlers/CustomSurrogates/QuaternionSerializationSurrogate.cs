using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

sealed class QuaternionSerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Quaternion quat = (Quaternion)obj;
        info.AddValue("x", quat.x);
        info.AddValue("y", quat.y);
        info.AddValue("z", quat.z);
        info.AddValue("w", quat.w);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Quaternion quat = (Quaternion)obj;
        quat = new Quaternion((float)info.GetDecimal("x"), (float)info.GetDecimal("y"), (float)info.GetDecimal("z"), (float)info.GetDecimal("w"));
        return quat;
    }
}
sealed class QuaternionDictSerializationSurrogate : ISerializationSurrogate
{

	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context)
	{
		var dict = (Dictionary<string,Quaternion>)obj;
		info.AddValue("count", dict.Count);
		int i = 0;
		foreach(var pair in dict){
			var content = pair.Value;
			info.AddValue(i.ToString() + "key", pair.Key);
			info.AddValue(i.ToString() + "x", content.x);
			info.AddValue(i.ToString() + "y", content.y);
			info.AddValue(i.ToString() + "z", content.z);
			info.AddValue(i.ToString() + "w", content.w);
			i++;
		}
	}
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector)
	{
		var dict = new Dictionary<string,Quaternion> ();
		var count = (int)info.GetValue ("count", typeof(int));
		for (int i = 0; i < count; i++) {
			var key = info.GetString (i.ToString () + "key");
			dict[key] = new Quaternion((float)info.GetDecimal(i.ToString() + "x"), (float)info.GetDecimal(i.ToString() + "y"), (float)info.GetDecimal(i.ToString() + "z"), (float)info.GetDecimal(i.ToString() + "w"));
		}
		return dict;
	}
}