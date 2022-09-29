using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

sealed class Vector2SerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Vector2 vec = (Vector2)obj;
        info.AddValue("x", vec.x);
        info.AddValue("y", vec.y);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Vector2 vec = (Vector2)obj;
        vec = new Vector2((float)info.GetDecimal("x"), (float)info.GetDecimal("y"));
        return vec;
    }
}
sealed class Vector3SerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Vector3 vec = (Vector3)obj;
        info.AddValue("x", vec.x);
        info.AddValue("y", vec.y);
        info.AddValue("z", vec.z);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Vector3 vec = (Vector3)obj;
        vec = new Vector3((float)info.GetDecimal("x"), (float)info.GetDecimal("y"), (float)info.GetDecimal("z"));
        return vec;
    }
}
sealed class Vector4SerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Vector4 vec = (Vector4)obj;
        info.AddValue("x", vec.x);
        info.AddValue("y", vec.y);
        info.AddValue("z", vec.z);
        info.AddValue("w", vec.w);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Vector4 vec = (Vector4)obj;
        vec = new Vector4((float)info.GetDecimal("x"), (float)info.GetDecimal("y"), (float)info.GetDecimal("z"), (float)info.GetDecimal("w"));
        return vec;
    }
}
sealed class Vector2DictSerializationSurrogate : ISerializationSurrogate
{
	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context)
	{
		var dict = (Dictionary<string,Vector2>)obj;
		info.AddValue("count", dict.Count);
		int i = 0;
		foreach(var pair in dict){
			var content = pair.Value;
			info.AddValue(i.ToString() + "key", pair.Key);
			info.AddValue(i.ToString() + "x", content.x);
			info.AddValue(i.ToString() + "y", content.y);
			i++;
		}
	}
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector)
	{
		var dict = new Dictionary<string,Vector2> ();
		var count = (int)info.GetValue ("count", typeof(int));
		for (int i = 0; i < count; i++) {
			var key = info.GetString (i.ToString () + "key");
			dict[key] = new Vector2((float)info.GetDecimal(i.ToString() + "x"), (float)info.GetDecimal(i.ToString() + "y"));
		}
		return dict;
	}
}
sealed class Vector3DictSerializationSurrogate : ISerializationSurrogate
{
	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context)
	{
		var dict = (Dictionary<string,Vector3>)obj;
		info.AddValue("count", dict.Count);
		int i = 0;
		foreach(var pair in dict){
			var content = pair.Value;
			info.AddValue(i.ToString() + "key", pair.Key);
			info.AddValue(i.ToString() + "x", content.x);
			info.AddValue(i.ToString() + "y", content.y);
			info.AddValue(i.ToString() + "z", content.z);
			i++;
		}
	}
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector)
	{
		var dict = new Dictionary<string,Vector3> ();
		var count = (int)info.GetValue ("count", typeof(int));
		for (int i = 0; i < count; i++) {
			var key = info.GetString (i.ToString () + "key");
			dict[key] = new Vector3((float)info.GetDecimal(i.ToString() + "x"), (float)info.GetDecimal(i.ToString() + "y"), (float)info.GetDecimal(i.ToString() + "z"));
		}
		return dict;
	}
}
sealed class Vector4DictSerializationSurrogate : ISerializationSurrogate
{
	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context)
	{
		var dict = (Dictionary<string,Vector4>)obj;
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
		var dict = new Dictionary<string,Vector4> ();
		var count = (int)info.GetValue ("count", typeof(int));
		for (int i = 0; i < count; i++) {
			var key = info.GetString (i.ToString () + "key");
			dict[key] = new Vector4((float)info.GetDecimal(i.ToString() + "x"), (float)info.GetDecimal(i.ToString() + "y"), (float)info.GetDecimal(i.ToString() + "z"), (float)info.GetDecimal(i.ToString() + "w"));
		}
		return dict;
	}
}