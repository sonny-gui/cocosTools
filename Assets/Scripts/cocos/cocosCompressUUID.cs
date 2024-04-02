using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class cocosCompressUUID : MonoBehaviour
{
    private static string Base64KeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    private static string Reg_Dash = @"-";
    private static string Reg_NormalizedUuid = @"^[0-9a-fA-F]{32}$";

    public static string CompressHex(string uuid)
    {
        string hexString = "";
        if (Regex.IsMatch(uuid, Reg_Dash))
            hexString = uuid.Replace(Reg_Dash, "");
        else if (!Regex.IsMatch(uuid, Reg_NormalizedUuid))
            return uuid;

        int length = hexString.Length;
        string head = hexString.Substring(0,5);
        
        List<char> base64Chars = new List<char>();
        int i = 5;
        while (i < length)
        {
            int hexVal1 = Convert.ToInt32(hexString[i].ToString(), 16);
            int hexVal2 = Convert.ToInt32(hexString[i+1].ToString(), 16);
            int hexVal3 = Convert.ToInt32(hexString[i+2].ToString(), 16);
            base64Chars.Add(Base64KeyChars[(hexVal1 << 2)|(hexVal2 >> 2)]);
            base64Chars.Add(Base64KeyChars[((hexVal2 & 3) << 4) | hexVal3]);
            i += 3;
        }
        string content = "";
        for (int j = 0; j < base64Chars.Count; j++)
            content += base64Chars[j];
        return head + content;
    }
}
