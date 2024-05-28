using System.Text.RegularExpressions;

public class RegexConst
{
    public const string password = @"[A-Za-z]+[0-9]";  //密码格式

    public const string number = @"^[0-9]*$";  //验证是否为数字

    public const string pwdLen = @"^\d{6,18}$";  //密码长度6-18

    public const string phone = @"^1[0-9]{10}$";   //电话格式

    public const string email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";  //邮箱格式

    public const string json = "^(?:\".*?\")|(?:true|false|null)|(?:[+-]?(\\d+(?:\\.?\\d*)?|\\d*(?:\\.\\d*))(?:[eE][+-]?\\d+)?)|(?:(?:\\s*\\[\\s*(?:(?:"
      + "(?:\".*?\")|(?:true|false|null)|(?:[+-]?(\\d+(?:\\.?\\d*)?|\\d*(?:\\.\\d*))(?:[eE][+-]?\\d+)?)|(?<json1>(?:\\[.*?\\])|(?:\\{.*?\\})))\\s*,\\s*)*(?:"
      + "(?:\".*?\")|(?:true|false|null)|(?:[+-]?(\\d+(?:\\.?\\d*)?|\\d*(?:\\.\\d*))(?:[eE][+-]?\\d+)?)|(?<json2>(?:\\[.*?\\])|(?:\\{.*?\\})))\\s*\\]\\s*)"
      + "|(?:\\s*\\{\\s*"
      + "(?:\".*?\"\\s*:\\s*(?:(?:\".*?\")|(?:true|false|null)|(?:[+-]?(\\d+(?:\\.?\\d*)?|\\d*(?:\\.\\d*))(?:[eE][+-]?\\d+)?)|(?<json3>(?:\\[.*?\\])|(?:\\{.*?\\})))\\s*,\\s*)*"
      + "(?:\".*?\"\\s*:\\s*(?:(?:\".*?\")|(?:true|false|null)|(?:[+-]?(\\d+(?:\\.?\\d*)?|\\d*(?:\\.\\d*))(?:[eE][+-]?\\d+)?)|(?<json4>(?:\\[.*?\\])|(?:\\{.*?\\}))))\\s*\\}\\s*))$";
}

public static class RegexExtension
{
    public static bool IsRegexMatch(this string str, string regex)
    {
        if (string.IsNullOrEmpty(str))
            return false;
        return Regex.IsMatch(str, regex);
    }

    public static bool IsJson(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;
        return Regex.IsMatch(str, RegexConst.json);
    }
}
