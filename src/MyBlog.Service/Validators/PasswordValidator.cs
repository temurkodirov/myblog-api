namespace MyBlog.Service.Validators;

public class PasswordValidator
{
 
    public static (bool IsValid, string Message) IsStrongPassword(string password)
    {
        if (password.Length < 6) return (IsValid: false, Message: "Password can not be less than 6 characters");
        bool isUppercase = false;
        bool isLowercase = false;
    
    

        foreach (var item in password)
        {
            if (char.IsUpper(item)) isUppercase = true;
            if (char.IsLower(item)) isLowercase = true;
           
        
        }

        if (isUppercase is false) return (IsValid: false, Message: "Password should contain at least one Uppercase letter");
        if (isLowercase is false) return (IsValid: false, Message: "Password should contain at least one Lowercase letter");
       
   

        return (IsValid: true, Message: "");
    }
}
