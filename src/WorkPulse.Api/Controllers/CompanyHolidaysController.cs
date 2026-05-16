using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;
using WorkPulse.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;
using WorkPulse.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;
using WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;
using WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
public sealed class CompanyHolidaysController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCompanyHolidaysQuery(from, to), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCompanyHolidayByIdQuery(id), cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyHolidayRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateCompanyHolidayCommand(request.Date, request.Name),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCompanyHolidayRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateCompanyHolidayCommand(id, request.Date, request.Name),
            cancellationToken);

        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteCompanyHolidayCommand(id), cancellationToken);
        return FromResult(result);
    }
}

public sealed record CreateCompanyHolidayRequest(DateOnly Date, string Name);

public sealed record UpdateCompanyHolidayRequest(DateOnly Date, string Name);
