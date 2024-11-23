using CleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace CleanArchitecture.Application.Common.Messaging
{
    public static class Result
    {
        #region Static Methods

        public static Result<T> Failure<T>(Error error, string? source)
        {
            return new Result<T>(error.Code, error.Message, false, error, source);
        }
        public static Result<T> Failure<T>(Error error,
                                             [CallerMemberName] string memberName = "",
                                             [CallerLineNumber] int sourceLineNumber = 0)
        {
            StackFrame frame = new(1);
            Type? type = GetCallerType(frame);
            var source = GetSource(memberName, sourceLineNumber, type);

            return new Result<T>(error.Code, error.Message, false, error, source);
        }
        public static Result<bool> Failure(Error error,
                                             [CallerMemberName] string memberName = "",
                                             [CallerLineNumber] int sourceLineNumber = 0)
        {
            StackFrame frame = new(1);
            Type? type = GetCallerType(frame);
            var source = GetSource(memberName, sourceLineNumber, type);


            return new Result<bool>(error.Code, error.Message, false, error, source);
        }
        public static Result<T> Success<T>(T? data = default, int count = 0, string message = "OK")
        {
            return new Result<T>(data, count, 200, message, true, default, default);
        }
        public static Result<bool> Success(int affectedRows)
        {
            return new Result<bool>(true, affectedRows, 200, "OK", true, default, default);
        }


        #endregion

        #region Private Helper Methods
        private static Type? GetCallerType(StackFrame frame)
        {
            var method = frame.GetMethod();
            var type = method?.DeclaringType;
            return type;
        }
        private static string GetSource(string memberName, int sourceLineNumber, Type? type)
        {
            return $"at {type?.ReflectedType?.FullName} [Type] in {memberName} [Member] in [Line]: {sourceLineNumber}";
        }
        #endregion
    }

    public sealed record Result<T> : IResult<T>
    {
        #region  Properties
        public T? Data { get; set; }
        public int Count { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public Error? Error { get; set; }
        public string? Source { get; set; }


        #endregion

        #region Constructors

        [JsonConstructor]
        public Result()
        {

        }
        public Result(int code,
                        string message,
                        bool isSuccess,
                        Error? error,
                        string? source) : this(default, 0, code, message, isSuccess, error, source)
        {

        }
        public Result(T? data,
                        int count,
                        int code,
                        string message,
                        bool isSuccess,
                        Error? error,
                        string? source)
        {
            Data = data;
            Count = count;
            StatusCode = code;
            Message = message;
            IsSuccess = isSuccess;
            Error = error;
            Source = source;
        }


        #endregion

        public ProblemDetails ToProblemDetails()
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException();
            }
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return new ProblemDetails
            {
                Title = Error?.Message,
                Detail = JsonConvert.SerializeObject(Error, options),
                Extensions = Error?.SubErrors != null ? Error.SubErrors.Select(x => new
                {
                    Title = ((HttpStatusCode)int.Parse(x.Key)).ToString(),
                    Details = x.Value
                })
                .ToDictionary(s => s.Title, s => (object?)s.Details) : [],
                Instance = Source,
                Status = StatusCode
            };
        }
    }
}
