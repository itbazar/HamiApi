
using Domain.Primitives;

namespace Domain.Models.Hami;

public class Stage : Entity
{
    // Constructor ?????
    private Stage(Guid id) : base(id) { }

    // ??? ??????? ???? ????? Stage
    public static Stage Create(string name, string description)
    {
        var disease = new Stage(Guid.Empty);
        disease.Title = name;
        disease.Description = description;
        disease.IsDeleted = false;
        return disease;
    }

    // ??? Update ???? ???????????
    public void Update(string? name, string? description)
    {
        Title = name ?? Title;
        Description = description ?? Description;
    }

    // ??? Delete ???? ??? ?????
    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    // ????????? Stage
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}