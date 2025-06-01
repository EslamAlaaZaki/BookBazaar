using BookBazaar.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookBazaar.Application.Validators
{
    public static class UserValidator
    {
        public static bool RegisterUserValidation (CreateUserDto createUserDto, out string errorMessage) 
        {
            errorMessage = string.Empty;

            if (createUserDto == null) 
            {
                errorMessage = "Invalide Passed Data";
                return false;
            }

            if (createUserDto.Email == null || IsValidEmail(createUserDto.Email) == false) 
            {
                errorMessage = "Invalid Email Address.";
                return false;
            }

            if (createUserDto.Name == null || IsValidName(createUserDto.Name) == false)
            {
                errorMessage = "Invalid name. The name must be between 3 and 20 characters long.";
                return false;
            }

            if (createUserDto.Password == null || IsValidPassword(createUserDto.Password) == false)
            {
                errorMessage = "Invalid password. It must be 8–20 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.";
                return false;
            }

            if (createUserDto.Phone != null && IsValidPhoneNumber(createUserDto.Phone) == false)
            {
                errorMessage = "Invalid Phone. The Phone must only numbers be between 4 and 15 characters long.";
                return false;
            }

            return true;
        }

        public static bool UpdateUserValidation (UpdateUserDto dto,out string errorMessage) 
        {
            errorMessage = string.Empty;

            if (dto == null)
            {
                errorMessage = "Invalide Passed Data";
                return false;
            }

            if (dto.Name != null && IsValidName(dto.Name) == false)
            {
                errorMessage = "Invalid name. The name must be between 3 and 15 characters long.";
                return false;
            }

            if (dto.Password != null && IsValidPassword(dto.Password) == false)
            {
                errorMessage = "Invalid password. It must be 8–20 characters long and include at least one uppercase letter, one lowercase letter, one number, and one special character.";
                return false;
            }

            if (dto.Phone != null && IsValidPhoneNumber(dto.Phone) == false)
            {
                errorMessage = "Invalid Phone. The Phone must only numbers be between 4 and 15 characters long.";
                return false;
            }

            return true;
        }
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private static bool IsValidName(string name) 
        {
            return name != null && name.Length > 2 && name.Length < 21;
        }

        private static bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^\d{4,15}$";
            return Regex.IsMatch(phone, pattern);
        }

        private static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[^\s]{8,20}$";
            return Regex.IsMatch(password, pattern);
        }

    }
}
