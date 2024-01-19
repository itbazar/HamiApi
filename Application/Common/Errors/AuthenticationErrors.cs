namespace Application.Common.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCaptcha = new Error("");
    public static readonly Error InvalidCredentials = new Error("");
    public static readonly Error UserNotFound = new Error("");

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
}