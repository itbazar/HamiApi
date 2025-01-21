using Domain.Primitives;
using Domain.Models.IdentityAggregate;
using Domain.Models.Hami.Events;
using FluentResults;
using System.Text.Json.Serialization;

namespace Domain.Models.Hami;

public class UserGroupMembership : Entity
{
    private UserGroupMembership(Guid id) : base(id) { }

    public static UserGroupMembership Create(string userId, Guid patientGroupId)
    {
        return new UserGroupMembership(Guid.NewGuid())
        {
            UserId = userId,
            PatientGroupId = patientGroupId
        };
    }

    public static Result<UserGroupMembership> Register(string userId, Guid patientGroupId,string phoneNumber)
    {
        UserGroupMembership info = new(Guid.NewGuid());
        info.UserId = userId;
        info.PatientGroupId = patientGroupId;
        info.Raise(new PatientApprovedDomainEvent(
            info.Id,
            info.UserId,
            phoneNumber));
        return info;
    }

    public void Update(Guid? patientGroupId)
    {
        PatientGroupId = patientGroupId ?? PatientGroupId;
    }

    public void Delete(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public string UserId { get; set; } = string.Empty; // شناسه کاربر
    [JsonIgnore]
    public ApplicationUser User { get; set; } = null!; // ارتباط با کاربر
    public Guid PatientGroupId { get; set; } // شناسه گروه
    [JsonIgnore]
    public PatientGroup PatientGroup { get; set; } = null!; // ارتباط با گروه
    public bool IsDeleted { get; set; } = false; // وضعیت حذف
}
