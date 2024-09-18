namespace HT.Core.Commons.Utils;

public static class StringUtils
{
    public static string SomenteNumeros(this string str)
    {
        return new string(str.Where(char.IsDigit).ToArray());
    }
}