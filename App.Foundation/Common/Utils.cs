using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Foundation.Common
{

    public enum MailType
    {
        ContactMail,
        DemoRequest
    }

    public class Utils
    {
       
        public const string ConnectionString = @"Data Source=tcp:yourflowdb.database.windows.net,1433; Initial Catalog=VERIDOC_TEST;User ID=adminveridoccertificates;Password=Sg9_Mt6x8Dw#dB^6;MultipleActiveResultSets=true;";
        public const string ConnectionStringProd = "Data Source=tcp:yourflowdb.database.windows.net,1433; Initial Catalog=Veridoc_prod;User ID=adminveridoccertificates;Password=Sg9_Mt6x8Dw#dB^6;MultipleActiveResultSets=true;";


        public static bool IsEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GeneratePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new();
            Random rnd = new();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static bool ValidatePassword(string password)
        {
            Regex regex = new ("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled);
            return regex.IsMatch(password);
        }

    }
}
