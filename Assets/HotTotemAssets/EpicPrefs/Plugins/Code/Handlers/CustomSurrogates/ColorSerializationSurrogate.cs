using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

sealed class ColorSerializationSurrogate : ISerializationSurrogate
{

    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Color col = (Color)obj;
        info.AddValue("r", col.r);
        info.AddValue("g", col.g);
        info.AddValue("b", col.b);
        info.AddValue("a", col.a);
    }
	public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Color col = (Color)obj;
        col = new Color((float)info.GetDecimal("r"), (float)info.GetDecimal("g"), (float)info.GetDecimal("b"), (float)info.GetDecimal("a"));
        return col;
    }
}
sealed class ColorDictSerializationSurrogate : ISerializationSurrogate
{

	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context)
	{
		var dict = (Dictionary<string,Color>)obj;
		info.AddValue("count", dict.Count);
		int i = 0;
		foreach(var pair in dict){
			var content = pair.Value;
			info.AddValue(i.ToString() + "key", pair.Key);
			info.AddValue(i.ToString() + "r", content.r);
			info.AddValue(i.ToString() + "g", content.g);
			info.AddValue(i.ToString() + "b", content.b);
			info.AddValue(i.ToString() + "a", content.a);
			i++;
		}
	}
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector)
	{
		var dict = new Dictionary<string,Color> ();
		var count = (int)info.GetValue ("count", typeof(int));
		for (int i = 0; i < count; i++) {
			var key = info.GetString (i.ToString () + "key");
			dict[key] = new Color((float)info.GetDecimal(i.ToString() + "r"), (float)info.GetDecimal(i.ToString() + "g"), (float)info.GetDecimal(i.ToString() + "b"), (float)info.GetDecimal(i.ToString() + "a"));
		}
		return dict;
	}
}