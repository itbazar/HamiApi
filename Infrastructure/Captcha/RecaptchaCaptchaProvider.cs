using Newtonsoft.Json;

namespace Infrastructure.Captcha;

public class RecaptchaCaptchaProvider
{
    public string ClientKey => _recaptchaInfo.ClientKey;

    private readonly RecaptchaInfo _recaptchaInfo;
    public RecaptchaCaptchaProvider(RecaptchaInfo recaptchaInfo)
    {
        _recaptchaInfo = recaptchaInfo;
    }

    public async Task<bool> IsCaptchaValid(string token)
    {
        bool? result = false;

        var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

        try
        {
            using var client = new HttpClient();

            var response = await client.PostAsync($"{googleVerificationUrl}?secret={_recaptchaInfo.ServerKey}&response={token}", null);
            var jsonString = await response.Content.ReadAsStringAsync();
            var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

            result = captchaVerfication?.Success;
        }
        catch (Exception e)
        {
            // fail gracefully, but log
        }

        return result ?? false;
    }
}

internal class CaptchaVerificationResponse
{
    public bool Success { get; set; }
    public DateTime Challenge_ts { get; set; }
    public string Hostname { get; set; } = null!;
    [JsonProperty("error-codes")]
    public IEnumerable<string> ErrorCodes { get; set; } = new List<string>();
    /*
     * {
          "success": true|false,
          "challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
          "hostname": string,         // the hostname of the site where the reCAPTCHA was solved
          "error-codes": [...]        // optional
        }
     */
}

public record RecaptchaInfo(string ClientKey, string ServerKey);