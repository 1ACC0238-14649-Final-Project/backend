using System.ComponentModel.DataAnnotations;
using System.Data;
using gigu_back_end.Briefcases.Domain.Models.Commands;
using gigu_back_end.Briefcases.Domain.Models.Exceptions;
using gigu_back_end.Briefcases.Domain.Models.Queries;
using gigu_back_end.Briefcases.Domain.Services;
using gigu_back_end.Briefcases.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace gigu_back_end.Briefcases.Interfaces.REST;

/// <summary>
/// REST Controller for managing briefcases (portfolios).
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
public class BriefcaseController(IBriefcaseQueryService briefcaseQueryService, IBriefcaseCommandService briefcaseCommandService) : ControllerBase
{
    private readonly IBriefcaseQueryService _briefcaseQueryService = briefcaseQueryService;
    private readonly IBriefcaseCommandService _briefcaseCommandService = briefcaseCommandService;

    /// <summary>
    /// Retrieves all briefcases.
    /// </summary>
    /// <returns>List of all briefcase resources.</returns>
    /// <response code="200">Briefcases retrieved successfully.</response>
    /// <response code="404">No briefcases found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            var result = await _briefcaseQueryService.Handle(new GetAllBriefcasesQuery());
            return result.Any()
                ? Ok(result.Select(BriefcaseResourceFromEntityAssembler.ToResourceFromEntity))
                : NotFound(new { message = "No briefcases found." });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Retrieves a briefcase by seller ID.
    /// </summary>
    /// <param name="sellerId">The unique identifier of the seller.</param>
    /// <returns>The briefcase resource for the specified seller.</returns>
    /// <response code="200">Briefcase retrieved successfully.</response>
    /// <response code="400">Invalid seller ID.</response>
    /// <response code="404">Briefcase not found for the specified seller.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("by-seller/{sellerId:int}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBySellerId(int sellerId)
    {
        if (sellerId <= 0) 
            return BadRequest(new { message = "Invalid seller ID." });

        try
        {
            var result = await _briefcaseQueryService.Handle(new GetBriefcaseBySellerIdQuery(sellerId));
            return result != null
                ? Ok(BriefcaseResourceFromEntityAssembler.ToResourceFromEntity(result))
                : NotFound(new { message = $"Briefcase for seller ID {sellerId} not found." });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new briefcase.
    /// </summary>
    /// <param name="command">Briefcase creation command containing all necessary information.</param>
    /// <returns>Status indicating the result of the creation.</returns>
    /// <response code="201">Briefcase created successfully.</response>
    /// <response code="400">Invalid request or project not found.</response>
    /// <response code="409">Briefcase with the same name already exists.</response>
    /// <response code="422">Validation error.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(object), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreateBriefcaseCommand command)
    {
        if (command == null) 
            return BadRequest(new { message = "Briefcase command cannot be null." });

        try
        {
            await _briefcaseCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (NotProjectFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DuplicateNameException)
        {
            return Conflict(new { message = "A briefcase with the same name already exists." });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing briefcase.
    /// </summary>
    /// <param name="id">The unique identifier of the briefcase to update.</param>
    /// <param name="command">Updated briefcase information.</param>
    /// <returns>Status indicating the result of the update.</returns>
    /// <response code="200">Briefcase updated successfully.</response>
    /// <response code="400">Invalid briefcase ID.</response>
    /// <response code="404">Briefcase not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateBriefcaseCommand command)
    {
        if (id <= 0) 
            return BadRequest(new { message = "Invalid briefcase ID." });

        try
        {
            await _briefcaseCommandService.Handle(command, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes a briefcase.
    /// </summary>
    /// <param name="id">The unique identifier of the briefcase to delete.</param>
    /// <returns>No content if deletion was successful.</returns>
    /// <response code="204">Briefcase deleted successfully.</response>
    /// <response code="400">Invalid briefcase ID.</response>
    /// <response code="404">Briefcase not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0) 
            return BadRequest(new { message = "Invalid briefcase ID." });

        try
        {
            await _briefcaseCommandService.Handle(new DeleteBriefcaseCommand(id));
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}