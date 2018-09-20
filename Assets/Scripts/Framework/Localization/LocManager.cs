using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LocManager : TSingleton<LocManager>
{
    private Dictionary<LocLang, Dictionary<string, string>> _LocText = new Dictionary<LocLang, Dictionary<string, string>>();
    List<LocLang> _SupportedLanguages = new List<LocLang> { LocLang.English, LocLang.SimplifiedChinese, LocLang.TraditionalChinese, LocLang.Japanese };

    private LocLang _CurrentLang = LocLang.None;
    public LocLang CurrentLanguage
    {
        get
        {
            return _CurrentLang;
        }
        set
        {
            _CurrentLang = value;
            for (int i = 0, length = LocComponent._Locs.Count; i < length; i++)
            {
                var l = LocComponent._Locs[i];
                l.Process();
            }
        }
    }

    public void Init(LocLang currentLang)
    {
        _CurrentLang = currentLang;
        if (_CurrentLang == LocLang.None && _SupportedLanguages.Count > 0)
        {
            _CurrentLang = _SupportedLanguages[0];
        }
        _LocText = new Dictionary<LocLang, Dictionary<string, string>>();
        ParseTranslation();
    }

    public List<string> GetSupportLanguages()
    {
        //string[] ss = new string[_SupportedLanguages.Count];
        //for (int i = 0, length = _SupportedLanguages.Count; i < length; i++)
        //{
        //    ss[i] = LocLangConvert.GetCSVColumnName(_SupportedLanguages[i]);
        //}
        //return ss;

        List<string> ss = new List<string>();
        for (int i = 0, length = _SupportedLanguages.Count; i < length; i++)
        {
            ss.Add(_SupportedLanguages[i].ToString());  
        }
        return ss;
    }

    // get localization string
    public string DoLoc(string stringID, params object[] param)
    {
        string trans;
        if (string.IsNullOrEmpty(stringID))
            return string.Empty;
        if (_LocText.ContainsKey(CurrentLanguage) && _LocText[CurrentLanguage].TryGetValue(stringID, out trans))
        {
            if (param.Length > 0)
            {
                trans = string.Format(trans, param);
            }
            //trans = trans.Replace("\\n", "\n");
        }
        else
        {
            trans = stringID;
        }
        return trans;
    }

    private void ParseTranslation()
    {
        var list = ConfigDataManager.instance.GetDataList<LocCSV>();
        string trans = "";
        for (int j = 0, max = list.Count; j < max; j++)
        {
            var csv = list[j].As<LocCSV>();
            if (csv == null)
            {
                continue;
            }
            for (int i = 0, length = _SupportedLanguages.Count; i < length; i++)
            {
                var lang = _SupportedLanguages[i];
                switch (lang)
                {
                    case LocLang.English:
                        trans = csv._English;
                        break;
                    case LocLang.SimplifiedChinese:
                        trans = csv._Chinese;
                        break;
                    case LocLang.TraditionalChinese:
                        trans = csv._TraditionalChinese;
                        break;
                    case LocLang.Japanese:
                        trans = csv._Japanese;
                        break;
                }
                if (string.IsNullOrEmpty(trans))
                {
                    trans = csv._ID;
                }
                //trans = trans.Replace("\\n", "\n");
                if (!_LocText.ContainsKey(lang))
                {
                    _LocText[lang] = new Dictionary<string, string>();
                }
                _LocText[lang][csv._ID] = trans;
            }
        }
    }
}
