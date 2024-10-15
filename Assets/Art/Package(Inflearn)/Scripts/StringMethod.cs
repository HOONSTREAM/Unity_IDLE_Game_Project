using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Default,
    SI,
}

public static partial class StringMethod
{
    const string Zero = "0";

    static readonly string[] CurrencyUnits = new string[]
    {
            "",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "AO",
            "AP",
            "AQ",
            "AR",
            "AS",
            "AT",
            "AU",
            "AV",
            "AW",
            "AX",
            "AY",
            "AZ",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "BO",
            "BP",
            "BQ",
            "BR",
            "BS",
            "BT",
            "BU",
            "BV",
            "BW",
            "BX",
            "BY",
            "BZ",
            "CA",
            "CB",
            "CC",
            "CD",
            "CE",
            "CF",
            "CG",
            "CH",
            "CI",
            "CJ",
            "CK",
            "CL",
            "CM",
            "CN",
            "CO",
            "CP",
            "CQ",
            "CR",
            "CS",
            "CT",
            "CU",
            "CV",
            "CW",
            "CX",
    };

    static readonly string[] SI = new string[]
    {
    };
    public static string ToCurrencyString(this double number, CurrencyType currencyType = CurrencyType.Default)
    {
        if (-1d < number && number < 1d)
        {
            return Zero;
        }

        if (true == double.IsInfinity(number))
        {
            return "Infinity";
        }

        string significant = (number < 0) ? "-" : string.Empty;

        string showNumber = string.Empty;

        string unitString = string.Empty;

        string[] partsSplit = number.ToString("E").Split('+');

        if (partsSplit.Length < 2)
        {
            UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0})", number));
            return Zero;
        }

        if (false == int.TryParse(partsSplit[1], out int exponent))
        {
            UnityEngine.Debug.LogWarning(string.Format("Failed - ToCurrencyString({0}) : partsSplit[1] = {1}", number, partsSplit[1]));
            return Zero;
        }

        int quotient = exponent / 3;

        int remainder = exponent % 3;

        if (exponent < 3)
        {
            showNumber = Math.Truncate(number).ToString();
        }
        else
        {
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * Math.Pow(10, remainder);

            showNumber = temp.ToString("F").Replace(".00", "");
        }


        if (currencyType == CurrencyType.Default)
        {
            unitString = CurrencyUnits[quotient];
        }
        else
        {
            unitString = SI[quotient];
        }

        return string.Format("{0}{1}{2}", significant, showNumber, unitString);
    }
    public static double ToCurrencyDouble(this string currencyString, CurrencyType stringType = CurrencyType.Default)
    {
        double result = 0;

        bool isNumber = double.TryParse(currencyString, out result);

        if (true == isNumber)
        {
            return result;
        }
        else
        {
            int length = currencyString.Length;
            int lastNumberIndex = -1;

            for (int i = length - 1; 0 <= i; --i)
            {
                if (true == char.IsNumber(currencyString, i))
                {
                    lastNumberIndex = i;
                    break;
                }
            }

            if (lastNumberIndex < 0)
            {
                throw new Exception("Failed currency string");
            }

            string number = currencyString.Substring(0, lastNumberIndex + 1);
            string unit = currencyString.Substring(lastNumberIndex + 1);

            int index = (CurrencyType.Default == stringType) ? Array.FindIndex(CurrencyUnits, p => p == unit) : Array.FindIndex(SI, p => p == unit);
            if (-1 == index)
            {
                throw new Exception("Failed currency string");
            }

            string exponentNumber = string.Format("{0}E+{1}", number, index * 3);

            return double.Parse(exponentNumber);
        }
    }

}
