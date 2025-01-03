using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CounselingSessions.Commands.AddCounselingSessionCommand;

public record AddCounselingSessionCommand(
    Guid PatientGroupId,
    string MentorId,
    DateTime ScheduledDate,
    string Topic,
    string MeetingLink,
    string MentorNote) : IRequest<Result<CounselingSession>>;
