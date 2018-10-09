using UnityEngine;

public enum LocLang
{
    English,
    SimplifiedChinese,
    TraditionalChinese,
    Thai,
    Vietnamese,
    Indonesia,
    BrazilianPortuguese,
    Spanish,
    Russian,
    Korean,
    French,
    German,
    Turkish,
    HINDI,
    Japanese,
    Romanian,
}

public static class LocLangConvert
{
    public static string GetCSVColumnName(LocLang lang)
    {
        switch (lang)
        {
            case LocLang.English: return "English";
            case LocLang.SimplifiedChinese: return "Chinese";
            case LocLang.TraditionalChinese: return "TraditionalChinese";
            case LocLang.Thai: return "";
            case LocLang.Vietnamese: return "";
            case LocLang.Indonesia: return "";
            case LocLang.BrazilianPortuguese: return "";
            case LocLang.Spanish: return "";
            case LocLang.Russian: return "";
            case LocLang.Korean: return "";
            case LocLang.French: return "";
            case LocLang.German: return "";
            case LocLang.Turkish: return "";
            case LocLang.HINDI: return "";
            case LocLang.Japanese: return "Japanese";
            case LocLang.Romanian: return "";
        }
        return "";
    }

    public static string GetAbbr(LocLang lang)
    {
        switch (lang)
        {
            case LocLang.English: return "en";
            case LocLang.SimplifiedChinese: return "zh-cn";
            case LocLang.TraditionalChinese: return "zh-tw";
            case LocLang.Thai: return "th";
            case LocLang.Vietnamese: return "vn";
            case LocLang.Indonesia: return "ind";
            case LocLang.BrazilianPortuguese: return "pt-br";
            case LocLang.Spanish: return "es";
            case LocLang.Russian: return "ru";
            case LocLang.Korean: return "ko";
            case LocLang.French: return "fr";
            case LocLang.German: return "de";
            case LocLang.Turkish: return "tr";
            case LocLang.HINDI: return "hi";
            case LocLang.Japanese: return "ja";
            case LocLang.Romanian: return "rou";
        }
        return "none";
    }
    public static LocLang GetLocLang(string abbr)
    {
        switch (abbr)
        {
            case "en": return LocLang.English;
            case "zh-cn": return LocLang.SimplifiedChinese;
            case "zh-tw": return LocLang.TraditionalChinese;
            case "th": return LocLang.Thai;
            case "vn": return LocLang.Vietnamese;
            case "ind": return LocLang.Indonesia;
            case "pt-br": return LocLang.BrazilianPortuguese;
            case "es": return LocLang.Spanish;
            case "ru": return LocLang.Russian;
            case "ko": return LocLang.Korean;
            case "fr": return LocLang.French;
            case "de": return LocLang.German;
            case "tr": return LocLang.Turkish;
            case "hi": return LocLang.HINDI;
            case "ja": return LocLang.Japanese;
            case "rou": return LocLang.Romanian;
        }
        return LocLang.English;
    }

    public static LocLang GetLocLang(SystemLanguage systemLan)
    {
        switch (systemLan)
        {
            case SystemLanguage.English: return LocLang.English;
            case SystemLanguage.Chinese: return LocLang.TraditionalChinese;
            case SystemLanguage.ChineseSimplified: return LocLang.SimplifiedChinese;
            case SystemLanguage.ChineseTraditional: return LocLang.TraditionalChinese;
            case SystemLanguage.Thai: return LocLang.Thai;
            case SystemLanguage.Vietnamese: return LocLang.Vietnamese;
            case SystemLanguage.Indonesian: return LocLang.Indonesia;
            case SystemLanguage.Portuguese: return LocLang.BrazilianPortuguese;
            case SystemLanguage.Spanish: return LocLang.Spanish;
            case SystemLanguage.Russian: return LocLang.Russian;
            case SystemLanguage.Korean: return LocLang.Korean;
            case SystemLanguage.French: return LocLang.French;
            case SystemLanguage.German: return LocLang.German;
            case SystemLanguage.Turkish: return LocLang.Turkish;
            case SystemLanguage.Japanese: return LocLang.Japanese;
            case SystemLanguage.Romanian: return LocLang.Romanian;
        }
        return LocLang.English;
    }

    public static string GetLocLangName(LocLang loc)
    {
        switch (loc)
        {
            case LocLang.English: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_ENGLISH");
            case LocLang.SimplifiedChinese: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_CHINESESIM");
            case LocLang.TraditionalChinese: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_CHINESETRA");
            case LocLang.Thai: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_THAI");
            case LocLang.Vietnamese: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_VIETNAMESE");
            case LocLang.Indonesia: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_INDONESIA");
            case LocLang.BrazilianPortuguese: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_BRAZILIAN");
            case LocLang.Spanish: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_SPANISH");
            case LocLang.Russian: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_RUSSIAN");
            case LocLang.Korean: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_KOREAN");
            case LocLang.French: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_FRENCH");
            case LocLang.German: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_GERMAN");
            case LocLang.Turkish: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_TURKEY");
            case LocLang.HINDI: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_HINDI");
            case LocLang.Japanese: return LocManager.instance.DoLoc("TXT_SETTING_LANGUAGE_JAPANESE");
            case LocLang.Romanian: return LocManager.instance.DoLoc("TXT_OB7_HY_LANGUAGE_SETTING_ROU");
        }
        return string.Empty;
    }
}