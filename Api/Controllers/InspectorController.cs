using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Infrastructure.Options;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;

namespace Api.Controllers;

[Authorize(Roles = "Inspector")]
public class InspectorController : ApiController
{
    private readonly StorageOptions _storageOptions;
    public InspectorController(ISender sender, IOptions<StorageOptions> storageOptions) : base(sender)
    {
        _storageOptions = storageOptions.Value;
    }

}
