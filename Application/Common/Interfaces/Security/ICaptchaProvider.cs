namespace Application.Common.Interfaces.Security;

public interface ICaptchaProvider
{
    public string GetRandomString(int length);
    public CaptchaResultModel GenerateImage(string text = "");
    public bool Validate(CaptchaValidateModel model);
}

public record CaptchaValidateModel(Guid Key, string Value);
public record CaptchaResultModel(Guid Key, byte[] Data);