using System.ComponentModel.DataAnnotations;
using SS.Mvc.AxosoftApi.Properties;

namespace SS.Mvc.AxosoftApi.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.Email))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.UserName))]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.Password))]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.RememberMe))]
        public bool RememberMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Code { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.ConfirmPassword))]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.PasswordsDoNotMatch))]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.Email))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.Password))]
        //[PasswordComplexityRegularExpression]
        public string Password { get; set; }
    }

    public class ActivateAccountModel
    {
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.ConfirmPassword))]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.PasswordsDoNotMatch))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = nameof(Resources.FieldIsRequired))]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources), Name = nameof(Resources.Password))]
        //[PasswordComplexityRegularExpression]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
