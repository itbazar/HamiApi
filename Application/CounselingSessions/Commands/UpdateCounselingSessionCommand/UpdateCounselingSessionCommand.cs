using Domain.Models.Hami;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CounselingSessions.Commands.UpdateCounselingSessionCommand;

public record UpdateCounselingSessionCommand(
    Guid Id,
    string? MentorId,
    DateTime? ScheduledDate,
    string? Topic,
    string? MeetingLink,
    string? MentorNote) : IRequest<Result<CounselingSession>>;
