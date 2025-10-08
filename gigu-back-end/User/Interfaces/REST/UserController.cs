using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FluentValidation;
using gigu_back_end.Shared.Domain.Models.Commands;
using gigu_back_end.Shared.Infraestructure.Attribute;
using gigu_back_end.User.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Exceptions;
using gigu_back_end.User.Domain.Services;
using gigu_back_end.User.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gigu_back_end.User.Interfaces.REST;

/// <summary>
/// REST Controller for managing users.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController(IUserQueryService userQueryService, IUserCommandService userCommandService) : ControllerBase
{
    private readonly IUserQueryService _userQueryService = userQueryService;
    private readonly IUserCommandService _userCommandService = userCommandService;
    
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>List of all user resources.</returns>
    /// <response code="200">Users retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [CustomAuthorize("buyer,seller")]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userQueryService.Handle(new GetAllUsersQuery());
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }
    
    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The requested user resource.</returns>
    /// <response code="200">User retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id:int}")]
    [CustomAuthorize("seller,buyer")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userQueryService.Handle(new GetUserByIdQuery(id));
        if (user is null) return NotFound();
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="command">User creation command containing all necessary information.</param>
    /// <returns>Status indicating the result of the creation.</returns>
    /// <response code="201">User created successfully.</response>
    /// <response code="422">Validation error - required fields not completed.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        try
        {
            await _userCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotRequiredFieldsCompletedException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }

    /// <summary>
    /// Updates a user's basic information (Name, Lastname, Email).
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="command">The updated user information.</param>
    /// <returns>Status indicating the result of the update.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/v1/user/3
    ///     {
    ///         "name": "John",
    ///         "lastname": "Doe",
    ///         "email": "john.doe@example.com"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">User updated successfully.</response>
    /// <response code="400">Bad request due to invalid input.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _userCommandService.Handle(command, id);
            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error updating user", detail = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>No content if deletion was successful.</returns>
    /// <response code="204">User deleted successfully.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        await _userCommandService.Handle(new DeleteUserCommand(id));
        return NoContent();
    }
    
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="command">Sign-up command containing user registration information.</param>
    /// <returns>Status indicating the result of the registration.</returns>
    /// <response code="201">User registered successfully.</response>
    /// <response code="409">Email already taken.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        try
        {
            var user = await _userCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (EmailAlreadyTakenException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
        }
    }
    
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="command">Login command containing user credentials.</param>
    /// <returns>JWT token for authenticated user.</returns>
    /// <response code="200">Authentication successful, returns JWT token.</response>
    /// <response code="401">Invalid credentials.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var jwToken = await _userCommandService.Handle(command);
            return Ok(jwToken);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
        }
    }
    
    /// <summary>
    /// Retrieves the current authenticated user's information.
    /// </summary>
    /// <returns>Current user's resource information.</returns>
    /// <response code="200">Current user retrieved successfully.</response>
    /// <response code="401">Unauthorized or invalid token.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("me")]
    [CustomAuthorize("buyer,seller")] 
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (userIdClaim == null)
                return Unauthorized(new { message = "User ID claim not found" });

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Invalid user ID in token" });

            var user = await _userQueryService.Handle(new GetCurrentUserQuery(userId));
            if (user == null)
                return NotFound(new { message = "User not found" });

            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error fetching current user", detail = ex.Message });
        }
    }
}