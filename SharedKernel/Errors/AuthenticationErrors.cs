﻿using FluentResults;

namespace SharedKernel.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCaptcha = new Error("");
    public static readonly Error InvalidCredentials = new Error("");
    public static readonly Error InvalidOtp = new Error("");
    public static readonly Error InvalidUsername = new Error("");
    public static readonly Error InvalidRefereshToken = new Error("");
    public static readonly Error InvalidAccessToken = new Error("");
    public static readonly Error TokenNotExpiredYet = new Error("");
    public static readonly Error UserNotFound = new Error("");
    public static readonly Error UserCreationFailed = new Error("");
    public static readonly Error TooManyRequestsForOtp = new Error("");

}
public static class CommunicationErrors
{
    public static readonly Error SmsError = new Error("");
}

public static class GenericErrors
{
    public static readonly Error NotFound = new Error("");
    public static readonly Error AttachmentFailed = new Error("");
}

public static class EncryptionErrors
{
    public static readonly Error KeyGenerationFailed = new Error("");
}

public static class UserErrors
{
    public static readonly Error UnExpected = new Error("");
    public static readonly Error UserNotExsists = new Error("");
    public static readonly Error PasswordUpdateFailed = new Error("");
    public static readonly Error RoleUpdateFailed = new Error("");
}

public static class ComplaintErrors
{
    public static readonly Error InconsistentContent = new Error("");
    public static readonly Error InvalidOperation = new Error("");
    public static readonly Error NotFound = new Error("");
    public static readonly Error PublicKeyNotFound = new Error("");
}