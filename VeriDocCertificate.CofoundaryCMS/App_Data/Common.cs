using System.Text.RegularExpressions;

namespace VeriDocCertificate.CofoundaryCMS.App_Data
{
    public class Common
    {
        public const string AppNameKey = "AppName";
        public const string AppUrlKey = "AppUrl";
        public const string AppMailKey = "AppMail";


        public static bool IsValidEmail(string email)
        {
            Regex regex = new ("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", RegexOptions.Compiled);
            return regex.IsMatch(email);
        }
    }
}
