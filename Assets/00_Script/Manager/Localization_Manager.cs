using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_Manager 
{
    public class String_Data
    {
        public string ko;
        public string en;

        public string Get_Data(string lan = "")
        {
            if (lan != "")
            {
                switch (lan)
                {
                    case "ko": return ko;
                    case "en": return en;
                }
            }
            switch (LocalAccess)
            {
                case "ko": return ko;
                case "en": return en;
            }
            return "";
        }
    }
    public static string LocalAccess = "ko";

    public static Dictionary<string, String_Data> local_Data = new Dictionary<string, String_Data>();

    public void Init() => local_Data = dictionary();

    private static Dictionary<string, String_Data> dictionary()
    {
        var localization = CSV_Importer.Localization_Design;
        Dictionary<string, String_Data> value = new Dictionary<string, String_Data>();
        for (int i = 0; i < localization.Count; i++)
        {
            String_Data data = new String_Data();
            data.ko = localization[i]["ko"].ToString();
            data.en = localization[i]["en"].ToString();

            string temp = localization[i]["Key"].ToString();

            value.Add(temp, data);
        }

        string localCheck = PlayerPrefs.GetString("LOCAL");
        if (localCheck == "" || localCheck == null)
        {
            SystemLanguage lang = Application.systemLanguage;

            switch (lang)
            {
                case SystemLanguage.Korean: LocalAccess = "ko"; break;
                default: LocalAccess = "en"; break;
            }
        }
        else
        {
            LocalAccess = localCheck;
        }
        return value;
    }

}
