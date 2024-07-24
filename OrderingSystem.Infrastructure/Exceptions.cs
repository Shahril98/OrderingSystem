using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure
{
    public class CustomException(string? message) : Exception(message);

    public class RecordNotFoundException : BadHttpRequestException
    {
        public RecordNotFoundException(string message = "Tiada rekod ditemui", object? identifier = null) : base(message, 404)
        {
            if (identifier is not null)
                Data.Add("identifier", identifier);
        }
    }

    public class EmailOrPasswordNotValidException(string message) : BadHttpRequestException(message, 400);
    public class ActionNotValidException(string message) : BadHttpRequestException(message, 400);
    public class BadRequestException(string message) : BadHttpRequestException(message, 400);
    public class UserNotAuthenticatedException(string message) : BadHttpRequestException(message, 401);
    public class UserNotAuthorizedException(string message) : BadHttpRequestException(message, 403);
    public class NoDataUpdatedException(string message) : BadHttpRequestException(message, 422);
    public class RecordAlreadyExistException(string message): BadHttpRequestException(message, 409);
    public class NotValidMediaType(string message): BadHttpRequestException(message, 415);
    public class OperationAbortedException(string message ="Sesuatu yang tidak dijangaka telah berlaku menyebabkan operasi dibatalkan. Sila hubungi pentadbir untuk bantuan") : BadHttpRequestException(message, 500);
}
