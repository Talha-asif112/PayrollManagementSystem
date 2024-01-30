using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;

namespace Utilities;

public class Response<T> : BaseResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public object Misc { get; set; } // Like Permissions Etc..

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<KeyValuePair<string, string>> Errors { get; set; }

    public static Response<T> NotFound(string message)
    {
        return new Response<T>() { StatusCode = HttpStatusCode.NotFound, Message = message };
    }

    public static Response<T> BadRequest(string message)
    {
        return new Response<T>() { StatusCode = HttpStatusCode.BadRequest, Message = message };
    }

    public static Response<T> Ok(string message, T data, object miscData = null)
    {
        return new Response<T>() { StatusCode = HttpStatusCode.OK, Data = data, Misc = miscData, Message = message };
    }
}

public class ErrorResponse : BaseResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<KeyValuePair<string, string>> Errors { get; set; }

    public static ErrorResponse BadRequest(string message)
    {
        return new ErrorResponse() { StatusCode = HttpStatusCode.BadRequest, Message = message };
    }
}

public class SuccessAddResponse<T, TT> : SuccessResponse<T>
{
    /// <summary>
    /// Gets or sets the miscellaneous object of the successful response.
    /// </summary>
    /// <example>{"key": "value"}</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TT? Misc { get; set; }

    public static SuccessAddResponse<T, TT> Ok(string message, T data, TT? miscData = default)
    {
        return new SuccessAddResponse<T, TT>() { StatusCode = HttpStatusCode.OK, Data = data, Misc = miscData, Message = message };
    }
}

/// <summary>
/// Represents a successful response with a generic type parameter.
/// </summary>
/// <remarks>
/// This response includes the data of type T and an optional miscellaneous object.
/// </remarks>
/// <typeparam name="T">The type of the data in the response.</typeparam>
public class SuccessResponse<T> : BaseResponse
{
    /// <summary>
    /// Gets or sets the data of the successful response.
    /// </summary>
    /// <example>Example data</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; }

    public static Response<T> Ok(string message, T data, object miscData = null)
    {
        return new Response<T>() { StatusCode = HttpStatusCode.OK, Data = data, Misc = miscData, Message = message };
    }
}


/// <summary>
/// Represents the base response returned by the API.
/// </summary>
/// <remarks>
/// This response includes an optional status code and message.
/// </remarks>
public class BaseResponse
{
    /// <summary>
    /// Gets or sets the status code of the response. Possible values include:
    /// - `Continue = 100`
    /// - `SwitchingProtocols = 101`
    /// - `OK = 200`
    /// - `Created = 201`
    /// - `Accepted = 202`
    /// - `NonAuthoritativeInformation = 203`
    /// - `NoContent = 204`
    /// - `ResetContent = 205`
    /// - `PartialContent = 206`
    /// - `MultipleChoices = 300`
    /// - `Ambiguous = 300`
    /// - `MovedPermanently = 301`
    /// - `Moved = 301`
    /// - `Found = 302`
    /// - `Redirect = 302`
    /// - `SeeOther = 303`
    /// - `RedirectMethod = 303`
    /// - `NotModified = 304`
    /// - `UseProxy = 305`
    /// - `Unused = 306`
    /// - `TemporaryRedirect = 307`
    /// - `RedirectKeepVerb = 307`
    /// - `BadRequest = 400`
    /// - `Unauthorized = 401`
    /// - `PaymentRequired = 402`
    /// - `Forbidden = 403`
    /// - `NotFound = 404`
    /// - `MethodNotAllowed = 405`
    /// - `NotAcceptable = 406`
    /// - `ProxyAuthenticationRequired = 407`
    /// - `RequestTimeout = 408`
    /// - `Conflict = 409`
    /// - `Gone = 410`
    /// - `LengthRequired = 411`
    /// - `PreconditionFailed = 412`
    /// - `RequestEntityTooLarge = 413`
    /// - `RequestUriTooLong = 414`
    /// - `UnsupportedMediaType = 415`
    /// - `RequestedRangeNotSatisfiable = 416`
    /// - `ExpectationFailed = 417`
    /// - `UpgradeRequired = 426`
    /// - `InternalServerError = 500`
    /// - `NotImplemented = 501`
    /// - `BadGateway = 502`
    /// - `ServiceUnavailable = 503`
    /// - `GatewayTimeout = 504`
    /// - `HttpVersionNotSupported = 505`
    /// </summary>
    /// <example>200</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public HttpStatusCode? StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the message of the response.
    /// </summary>
    /// <example>Request processed successfully.</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; set; }
}