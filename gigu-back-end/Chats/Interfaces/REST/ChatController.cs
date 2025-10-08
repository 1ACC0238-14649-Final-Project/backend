using System.ComponentModel.DataAnnotations;
using Chats.Domain.Models.Commands;
using Chats.Domain.Services;
using Chats.Interfaces.REST.Resources;
using Chats.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Chats.Interfaces.REST;

/// <summary>
/// REST Controller for managing chat messages between users.
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatDomainService _chatDomainService;
    private readonly IChatQueryService _chatQueryService;

    public ChatController(IChatDomainService chatDomainService, IChatQueryService chatQueryService)
    {
        _chatDomainService = chatDomainService;
        _chatQueryService = chatQueryService;
    }

    /// <summary>
    /// Creates a new chat message between two users.
    /// </summary>
    /// <param name="command">Chat creation command containing senderId, receiverId, and content.</param>
    /// <returns>The created chat resource.</returns>
    /// <response code="201">Chat created successfully.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="404">Sender or receiver not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ChatResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreateChatCommand command)
    {
        try
        {
            var chat = await _chatDomainService.Handle(command);
            var resource = ChatResourceFromEntityAssembler.ToResourceFromEntity(chat);
            return StatusCode(StatusCodes.Status201Created, resource);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Retrieves all chat messages where the user is either sender or receiver.
    /// </summary>
    /// <param name="userId">ID of the user.</param>
    /// <returns>List of chats involving the user.</returns>
    /// <response code="200">Chats retrieved successfully.</response>
    /// <response code="404">No chats found.</response>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ChatResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChatsByUserId(int userId)
    {
        var chats = await _chatQueryService.Handle(new GetChatsByUserIdQuery(userId));
        if (chats == null || !chats.Any())
            return NotFound($"No chats found for user {userId}");

        var result = chats.Select(ChatResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all chat messages exchanged between two users.
    /// </summary>
    /// <param name="senderId">ID of the sender user.</param>
    /// <param name="receiverId">ID of the receiver user.</param>
    /// <returns>List of chats exchanged between the sender and receiver.</returns>
    /// <response code="200">Chats retrieved successfully.</response>
    /// <response code="404">No conversation found.</response>
    [HttpGet("conversation")]
    [ProducesResponseType(typeof(IEnumerable<ChatResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversation([FromQuery] int senderId, [FromQuery] int receiverId)
    {
        var chats = await _chatQueryService.Handle(new GetChatsBetweenUsersQuery(senderId, receiverId));
        if (chats == null || !chats.Any())
            return NotFound($"No conversation found between users {senderId} and {receiverId}");

        var result = chats.Select(ChatResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(result);
    }
}
