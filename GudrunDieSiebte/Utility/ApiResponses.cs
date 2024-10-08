using GudrunDieSiebte.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace GudrunDieSiebte.Utility
{
    public class ApiResponses
    {
        private class Response
        {
            public bool Success { get; set; }
            public string Error { get; set; }
            public object Data { get; set; }
        }
        public static JsonResult GetResponse(object data) 
        {
            var returnData = new Response
            {
                Success = true,
                Data = data
            };
            return new JsonResult(returnData);
        }
        public static JsonResult GetErrorResponse(int error, string errorMessage, object data = null)
        {
            switch (error)
            {
                case 1:
                    errorMessage = "not Found";
                    break;
                case 2:
                    errorMessage = "not Valid";
                    break;
                default:
                    errorMessage = errorMessage;
                    break;
            }
            var returnData = new Response
            {
                Data = data,
                Error = errorMessage,
                Success = false
            };
            return new JsonResult(returnData);
        }
    }
}
