﻿using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArandaSoftLab.Core.UseCase.Util
{
    public static class FluentValidacionExtensions
    {
        /// <summary>
        /// Concatena cada resultado en una Retorna una cadena de carácteres
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ToTextWithBr(this ValidationResult validationResult)
        {
            switch (validationResult.IsValid)
            {
                case true:
                    return "Ok";
                case false:
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Error:<br/>");
                    foreach (var error in validationResult.Errors)
                    {
                        sb.Append($"{error.ErrorMessage}<br/>");
                    }
                    string Error = sb.ToString();
                    return Error;
            }
            return "";


        }
        public static string ToText(this ValidationResult validationResult)
        {
            switch (validationResult.IsValid)
            {
                case true:
                    return "Ok";
                case false:
                    StringBuilder sb = new StringBuilder();
                    foreach (var error in validationResult.Errors)
                    {
                        sb.Append($"\n{error.ErrorMessage}\n");
                    }
                    string Error = sb.ToString();
                    return Error;
            }
            return "";
        }
    }
}
