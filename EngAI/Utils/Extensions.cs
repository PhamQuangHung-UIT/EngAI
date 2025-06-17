namespace EngAI.Utils;

public static class Extensions
{
    public static string Ellipsize(this string str, int maxLength)
    {
        if (str.Length <= maxLength)
            return str;
        return string.Concat(str.AsSpan(0, maxLength), "...");
    }
}
