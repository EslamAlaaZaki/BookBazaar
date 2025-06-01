using BookBazaar.Application.DTOs;
using BookBazaar.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.Application.Validators
{
    public static class BookValidator
    {
        public static bool CreateBookValidation(CreateBookDto createBookDto, out string errorMessage) 
        {
            errorMessage = string.Empty;

            if (createBookDto == null)
            {
                errorMessage = "Invalide Passed Data";
                return false;
            }

            if (createBookDto.Title == null || IsValidString(createBookDto.Title,3,125) == false)
            {
                errorMessage = "Invalid Title.The Title must be between 3 and 120 characters long.";
                return false;
            }
            if (createBookDto.Description == null || IsValidString(createBookDto.Description, 50, 500) == false)
            {
                errorMessage = "Invalid Description.The Description must be between 50 and 500 characters long.";
                return false;
            }

            if (createBookDto.Author == null || IsValidString(createBookDto.Author, 3, 50) == false)
            {
                errorMessage = "Invalid Author.The Author must be between 3 and 50 characters long.";
                return false;
            }

            if (createBookDto.Price < 0)
            {
                errorMessage = "Invalid Price.The Price zero or positive amount.";
                return false;
            }

            if (!Enum.TryParse<BookCondition>(createBookDto.Condition, ignoreCase: true, out var conditionEnum))
            {
                errorMessage = "Invalid BookCondition.BookCondition could be one of those values {LikeNew,Good,Fair}";
                return false;
            }

            return true;
        }

        public static bool UpdateBookValidation(UpdateBookDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto == null)
            {
                errorMessage = "Invalide Passed Data";
                return false;
            }

            if (dto.Title != null && IsValidString(dto.Title, 3, 125) == false)
            {
                errorMessage = "Invalid Title.The Title must be between 3 and 120 characters long.";
                return false;
            }
            if (dto.Description != null && IsValidString(dto.Description, 50, 500) == false)
            {
                errorMessage = "Invalid Description.The Description must be between 50 and 500 characters long.";
                return false;
            }

            if (dto.Author != null && IsValidString(dto.Author, 3, 50) == false)
            {
                errorMessage = "Invalid Author.The Author must be between 3 and 50 characters long.";
                return false;
            }

            if (dto.Price != null && dto.Price < 0)
            {
                errorMessage = "Invalid Price.The Price zero or positive amount.";
                return false;
            }

            if (dto.Condition != null && !Enum.TryParse<BookCondition>(dto.Condition, ignoreCase: true, out var conditionEnum))
            {
                errorMessage = "Invalid BookCondition.BookCondition could be one of those values {LikeNew,Good,Fair}";
                return false;
            }

            return true;
        }
        private static bool IsValidString(string name,int minLength ,int maxLength, bool couldBeEmpty = false) 
        {
            if(name.IsNullOrEmpty()) 
            {
                if(couldBeEmpty) 
                {
                    return true;
                }
                return false;
            }
            else if (name.Length > maxLength) 
            {
                return false; 
            }
            else if (name.Length < minLength)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
    }
}
