﻿namespace TaskManagementAPI.Models.Dtos.Responses
{
    public class ErrorResponse
    {
        public  string Title { get; set; }
        public int StatusCode { get; set; }
        public  string Message { get; set; }
    }
}
