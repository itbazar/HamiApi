namespace Api.Contracts.Sliders;

public record UpdateSliderDto(
    string? Title,
    IFormFile? Image,
    string? Url,
    string? Description);
