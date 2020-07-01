using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }
    }
}