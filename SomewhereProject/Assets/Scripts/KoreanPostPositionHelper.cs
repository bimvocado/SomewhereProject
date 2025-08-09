using System;

public static class KoreanPostpositionHelper
{
    public static string Josa(string name, string josaFormat)
    {
        if (string.IsNullOrEmpty(name)) return "";

        char lastChar = name[name.Length - 1];
        string[] josas = josaFormat.Split('/');

        if (lastChar < '가' || lastChar > '힣' || josas.Length != 2)
        {
            return name;
        }

        bool hasBatchim = (lastChar - 0xAC00) % 28 > 0;

        return name + (hasBatchim ? josas[0] : josas[1]);
    }
}