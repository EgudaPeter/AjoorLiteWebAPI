using System;
using System.Text.RegularExpressions;

namespace AjoorLiteWebAPI.Core
{
    static public class Utilities
    {
        static public string KEY = string.Empty;
        static public string USERNAME = string.Empty;
        static public string ERRORMESSAGE = "An error has occured! Please contact system developer!";

        public static bool EnsureCurrencyOnly(string str)
        {
            int count = 0;
            foreach (char c in str)
            {
                if (char.IsDigit(c) || c==',' || c=='.')
                    count++;
            }
            if (str.Length == count)
                return true;
            return false;
        }

        public static string CurrencyFormat(string str)
        {
            decimal value;
            if (decimal.TryParse(str, out value))
            {
                if(value == 0m)
                {
                    str = "0.00";
                }
                else
                {
                    str = value.ToString("#,#.00");
                }
            }
            else
            {
                str = string.Empty;
            }
            return str;
        }

        public static string RemoveCommasAndDots(string str)
        {
            string appendedForm = string.Empty;
            var splittedFormWithComma = str.Split(new string[] { "." }, StringSplitOptions.None)[0];
            var splittedFormWithoutComma = splittedFormWithComma.Split(new string[] { "," }, StringSplitOptions.None);
            for (int i = 0; i < splittedFormWithoutComma.Length; i++)
            {
                appendedForm += splittedFormWithoutComma[i];
            }
            return appendedForm;
        }

        public static bool EnsureNumericOnly(string str)
        {
            int count = 0;
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                    count++;
            }
            if (str.Length == count)
                return true;
            return false;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }
    }
}
