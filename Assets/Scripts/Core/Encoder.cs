using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class Encoder
{
    public static byte[] EncodeString(string s)
    {
        byte[] tmpSource = Encoding.ASCII.GetBytes(s);
        byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
        return tmpHash;
    }

    public static string ByteToString(byte[] data)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(data.Length);
        for (i = 0; i < data.Length - 1; i++)
        {
            sOutput.Append(data[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

    public static bool CompareStringToHash(string s, byte[] hash)
    {
        byte[] stringHash = EncodeString(s);
        bool equal = false;
        if (stringHash.Length == hash.Length)
        {
            int i = 0;
            while ((i < stringHash.Length) && (stringHash[i] == hash[i]))
            {
                i += 1;
            }
            if (i == stringHash.Length)
            {
                equal = true;
            }
        }
        return equal;
    }
}
