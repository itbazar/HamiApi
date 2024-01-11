using Application.Common.Interfaces.Communication;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;

namespace Application.Common.ExtensionMethods;

public static class ComplaintMessageExtensionMethods
{
    public static async Task SendMessages(
        this List<ComplaintMessage> messages,
        ICommunicationService communicationService,
        ApplicationUser? citizen,
        ApplicationUser? inspector)
    {
        foreach (var message in messages)
        {
            var phoneNumber = message.To == Actor.Citizen ? citizen?.PhoneNumber : inspector?.PhoneNumber;
            if (phoneNumber is null)
                continue;
            await communicationService.SendAsync(phoneNumber, message.Message);
        }
    }
}
