using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

sealed class TransformSerializationSurrogate : ISerializationSurrogate
{

    // Serialize the Employee object to save the object�s name and address fields.
    public void GetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context)
    {
        Transform tf = (Transform)obj;
        info.AddValue("name", tf.name);
        info.AddValue("tag", tf.tag);
        // Orientation
        info.AddValue("forwardX", tf.forward.x);
        info.AddValue("forwardY", tf.forward.y);
        info.AddValue("forwardZ", tf.forward.z);
        info.AddValue("rightX", tf.right.x);
        info.AddValue("rightY", tf.right.y);
        info.AddValue("rightZ", tf.right.z);
        info.AddValue("upX", tf.up.x);
        info.AddValue("upY", tf.up.y);
        info.AddValue("upZ", tf.up.z);
        // Position Rotation and Scale
        info.AddValue("positionX", tf.position.x);
        info.AddValue("positionY", tf.position.y);
        info.AddValue("positionZ", tf.position.z);
        info.AddValue("scaleX", tf.localScale.x);
        info.AddValue("scaleY", tf.localScale.y);
        info.AddValue("scaleZ", tf.localScale.z);
        info.AddValue("rotationW", tf.rotation.w);
        info.AddValue("rotationX", tf.rotation.x);
        info.AddValue("rotationY", tf.rotation.y);
        info.AddValue("rotationZ", tf.rotation.z);
    }

    // Deserialize the Employee object to set the object�s name and address fields.
    public System.Object SetObjectData(System.Object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {

        Transform tf = (Transform)obj;
        tf.name = info.GetString("name");
        tf.tag = info.GetString("tag");
        // Orientation
        tf.forward = new Vector3((float)info.GetDecimal("forwardX"), (float)info.GetDecimal("forwardY"), (float)info.GetDecimal("forwardZ"));
        tf.right = new Vector3((float)info.GetDecimal("rightX"), (float)info.GetDecimal("rightY"), (float)info.GetDecimal("rightZ"));
        tf.up = new Vector3((float)info.GetDecimal("upX"), (float)info.GetDecimal("upY"), (float)info.GetDecimal("upZ"));
        // Position Rotation and Scale
        tf.position = new Vector3((float)info.GetDecimal("positionX"), (float)info.GetDecimal("positionY"), (float)info.GetDecimal("positionZ"));
        tf.localScale = new Vector3((float)info.GetDecimal("scaleX"), (float)info.GetDecimal("scaleY"), (float)info.GetDecimal("scaleZ"));
        tf.rotation = new Quaternion((float)info.GetDecimal("rotationX"), (float)info.GetDecimal("rotationY"), (float)info.GetDecimal("rotationZ"), (float)info.GetDecimal("rotationW"));
        return tf;
    }
}

