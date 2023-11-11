namespace App.Entity.Models.Mail
{
    public class SignUpMail
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string VerifyLink { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string PackName { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string RepayLink { get; set; } = string.Empty;
    }
}
