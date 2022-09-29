using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;
using HotTotemCrypto;
namespace EpicPrefsTools{
	public static class Cryptor {
	    private static SaltedAES aesKey;
	    static Cryptor()
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());      
	    }
	    #region Encryption
		public static string Encrypt(byte[] value)
		{
			aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
			return aesKey.Encrypt(value);
		}
	    public static string Encrypt(string value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value);
	    }
	    public static string Encrypt(float value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value.ToString());
	    }
	    public static string Encrypt(int value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value.ToString());
	    }
	    public static string Encrypt(bool value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value.ToString());
	    }
	    public static string Encrypt(long value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value.ToString());
	    }
	    public static string Encrypt(double value)
	    {
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        return aesKey.Encrypt(value.ToString());
	    }
	    #endregion
	    #region Decryption
	    public static object Decrypt(string value,Serializer.SerializationTypes type)
	    {
	        object decrpytedValue = null;
	        aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
	        switch (type)
	        {
	            case Serializer.SerializationTypes.String:
	                decrpytedValue = aesKey.Decrypt(value);
	                break;
	            case Serializer.SerializationTypes.Integer:
	                decrpytedValue = Int32.Parse(aesKey.Decrypt(value));
	                break;
	            case Serializer.SerializationTypes.Float:
	                decrpytedValue = (float)Convert.ToDecimal(aesKey.Decrypt(value));
	                break;
	            case Serializer.SerializationTypes.Double:
	                decrpytedValue = Convert.ToDouble(aesKey.Decrypt(value));
	                break;
	            case Serializer.SerializationTypes.Long:
	                decrpytedValue = (long)Convert.ToDouble(aesKey.Decrypt(value));
	                break;
	            case Serializer.SerializationTypes.Bool:
	                decrpytedValue = Convert.ToBoolean(aesKey.Decrypt(value));
	                break;
	            default:
	                Debug.LogError("Decryption Error - eC10 : This type cannot be decrypted");
	                break;
	        }
	        return decrpytedValue;
	    }
		public static byte[] Decrypt(string value){
			aesKey = new SaltedAES(EpicPrefs.getPassPhrase(), EpicPrefs.getInitVector());
			return aesKey.DecryptToBytes(value);
		}
	    #endregion
	    
	}
}

