namespace Api.Contracts.Sliders;

public record AddSliderDto(
    string Title,
    IFormFile Image,
    string Url = "",
    string Description = "");