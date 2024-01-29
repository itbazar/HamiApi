using FluentResults;

namespace SharedKernel.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCaptcha = new Error("InvalidCaptcha");
    public static readonly Error InvalidCredentials = new Error("InvalidCredentials");
    public static readonly Error InvalidOtp = new Error("InvalidOtp");
    public static readonly Error InvalidUsername = new Error("InvalidUsername");
    public static readonly Error InvalidRefereshToken = new Error("InvalidRefereshToken");
    public static readonly Error InvalidAccessToken = new Error("InvalidAccessToken");
    public static readonly Error TokenNotExpiredYet = new Error("TokenNotExpiredYet");
    public static readonly Error UserNotFound = new Error("UserNotFound");
    public static readonly Error UserCreationFailed = new Error("UserCreationFailed");
    public static readonly Error TooManyRequestsForOtp = new Error("TooManyRequestsForOtp");

}
public static class CommunicationErrors
{
    public static readonly Error SmsError = new Error("SmsError");
}

public static class GenericErrors
{
    public static readonly Error NotFound = new Error("NotFound");
    public static readonly Error AttachmentFailed = new Error("AttachmentFailed");
}

public static class EncryptionErrors
{
    public static readonly Error KeyGenerationFailed = new Error("KeyGenerationFailed");
    public static readonly Error InvalidKey = new Error("InvalidKey");
    public static readonly Error WrongPassword = new Error("WrongPassword");
    public static readonly Error KeyNotFound = new Error("KeyNotFound");
    public static readonly Error Failed = new Error("Failed");
    public static readonly Error NullCipher = new Error("NullCipher");
    public static readonly Error InvalidHash = new Error("InvalidHash");
}

public static class UserErrors
{
    public static readonly Error UnExpected = new Error("UnExpected");
    public static readonly Error UserNotExsists = new Error("UserNotExsists");
    public static readonly Error PasswordUpdateFailed = new Error("PasswordUpdateFailed");
    public static readonly Error RoleUpdateFailed = new Error("RoleUpdateFailed");
}

public static class ComplaintErrors
{
    public static readonly Error InconsistentContent = new Error("InconsistentContent");
    public static readonly Error InvalidOperation = new Error("InvalidOperation");
    public static readonly Error NotFound = new Error("NotFound");
    public static readonly Error PublicKeyNotFound = new Error("PublicKeyNotFound");
}