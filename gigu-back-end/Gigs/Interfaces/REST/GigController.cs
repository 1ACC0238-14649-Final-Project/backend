using Microsoft.AspNetCore.Mvc;
using Gigs.Application.CommandService;
using Gigs.Application.QueryService;
using Gigs.Domain.Models.Exceptions;
using Gigs.Domain.Models.Queries;
using Gigs.Interfaces.REST.Resources;
using Gigs.Interfaces.REST.Transform;
using System.ComponentModel.DataAnnotations;

namespace Gigs.Interfaces.REST.Controllers
{
    /// <summary>
    /// REST Controller for managing gigs (freelance jobs).
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GigController : ControllerBase
    {
        private readonly GigCommandService _gigCommandService;
        private readonly GigQueryService _gigQueryService;

        public GigController(GigCommandService gigCommandService, GigQueryService gigQueryService)
        {
            _gigCommandService = gigCommandService;
            _gigQueryService = gigQueryService;
        }

        /// <summary>
        /// Creates a new gig.
        /// </summary>
        /// <param name="resource">Gig creation resource containing all necessary information.</param>
        /// <returns>The created gig resource.</returns>
        /// <response code="201">Gig created successfully.</response>
        /// <response code="400">Validation error.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [ProducesResponseType(typeof(GigResource), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGig([FromBody] CreateGigResource resource)
        {
            try
            {
                var command = CreateGigCommandFromResourceAssembler.ToCommandFromResource(resource);
                var gig = await _gigCommandService.CreateGigAsync(command);
                var gigResource = GigResourceFromEntityAssembler.ToResourceFromEntity(gig);
                
                return CreatedAtAction(nameof(GetGigById), new { id = gig.Id }, gigResource);
            }
            catch (GigValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.ValidationErrors });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a specific gig by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the gig.</param>
        /// <returns>The requested gig resource.</returns>
        /// <response code="200">Gig retrieved successfully.</response>
        /// <response code="404">Gig not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GigResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigById(int id)
        {
            try
            {
                var query = new GetGigByIdQuery(id);
                var gig = await _gigQueryService.GetGigByIdAsync(query);
                var resource = GigResourceFromEntityAssembler.ToResourceFromEntity(gig);
                
                return Ok(resource);
            }
            catch (GigNotFoundException)
            {
                return NotFound(new { message = $"Gig with id {id} not found" });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves all gigs with filtering and pagination support.
        /// </summary>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="searchTerm">Search term for filtering by title or description.</param>
        /// <param name="minPrice">Minimum price filter.</param>
        /// <param name="maxPrice">Maximum price filter.</param>
        /// <param name="maxDeliveryDays">Maximum delivery days filter.</param>
        /// <param name="sortBy">Field to sort by (default: createdAt).</param>
        /// <param name="descending">Sort in descending order (default: true).</param>
        /// <returns>Paginated list of gig resources.</returns>
        /// <response code="200">Gigs retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGigs(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string searchTerm = "",
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? maxDeliveryDays = null,
            [FromQuery] string sortBy = "createdAt",
            [FromQuery] bool descending = true)
        {
            try
            {
                var query = new GetAllGigsQuery(
                    page, 
                    pageSize, 
                    searchTerm,
                    minPrice,
                    maxPrice,
                    maxDeliveryDays,
                    sortBy,
                    descending);

                var (gigs, totalCount) = await _gigQueryService.GetAllGigsAsync(query);
                var resources = GigResourceFromEntityAssembler.ToResourceFromEntities(gigs);
                
                return Ok(new {
                    data = resources,
                    total = totalCount,
                    page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves all gigs for a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <returns>Paginated list of gigs for the specified seller.</returns>
        /// <response code="200">Seller's gigs retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("seller/{sellerId:int}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigsBySellerId(
            int sellerId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetGigsBySellerIdQuery(sellerId, page, pageSize);
                var (gigs, totalCount) = await _gigQueryService.GetGigsBySellerIdAsync(query);
                var resources = GigResourceFromEntityAssembler.ToResourceFromEntities(gigs);
        
                return Ok(new {
                    data = resources,
                    total = totalCount,
                    page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves all gigs for a specific category.
        /// </summary>
        /// <param name="category">The category name.</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="isResponsive">Filter for responsive gigs.</param>
        /// <returns>Paginated list of gigs in the specified category.</returns>
        /// <response code="200">Category gigs retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigsByCategory(
            string category, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isResponsive = null)
        {
            try
            {
                var query = new GetGigsByCategoryQuery(
                    category, 
                    page, 
                    pageSize,
                    isResponsive);

                var (gigs, totalCount) = await _gigQueryService.GetGigsByCategoryAsync(query);
                var resources = GigResourceFromEntityAssembler.ToResourceFromEntities(gigs);
                
                return Ok(new {
                    data = resources,
                    total = totalCount,
                    page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves all gigs that match the specified tags.
        /// </summary>
        /// <param name="tags">List of tags to filter by.</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <returns>Paginated list of gigs matching the specified tags.</returns>
        /// <response code="200">Tagged gigs retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigsByTags(
            [FromQuery] List<string> tags,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = new GetGigsByTagsQuery(tags, page, pageSize);
                var (gigs, totalCount) = await _gigQueryService.GetGigsByTagsAsync(query);
                var resources = GigResourceFromEntityAssembler.ToResourceFromEntities(gigs);
        
                return Ok(new {
                    data = resources,
                    total = totalCount,
                    page,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates an existing gig.
        /// </summary>
        /// <param name="id">The unique identifier of the gig to update.</param>
        /// <param name="resource">Updated gig information.</param>
        /// <returns>The updated gig resource.</returns>
        /// <response code="200">Gig updated successfully.</response>
        /// <response code="400">Validation error.</response>
        /// <response code="404">Gig not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(GigResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGig(int id, [FromBody] UpdateGigResource resource)
        {
            try
            {
                var command = UpdateGigCommandFromResourceAssembler.ToCommandFromResource(resource, id);
                var gig = await _gigCommandService.UpdateGigAsync(command);
                var gigResource = GigResourceFromEntityAssembler.ToResourceFromEntity(gig);
                
                return Ok(gigResource);
            }
            catch (GigNotFoundException)
            {
                return NotFound(new { message = $"Gig with id {id} not found" });
            }
            catch (GigValidationException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.ValidationErrors });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a gig.
        /// </summary>
        /// <param name="id">The unique identifier of the gig to delete.</param>
        /// <param name="sellerId">The seller ID for validation.</param>
        /// <returns>No content if deletion was successful.</returns>
        /// <response code="204">Gig deleted successfully.</response>
        /// <response code="400">Validation error.</response>
        /// <response code="404">Gig not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGig(int id, [FromQuery] int sellerId)
        {
            try
            {
                await _gigCommandService.DeleteGigAsync(id, sellerId);
                return NoContent();
            }
            catch (GigNotFoundException)
            {
                return NotFound(new { message = $"Gig with id {id} not found" });
            }
            catch (GigValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves the total count of gigs in a specific category.
        /// </summary>
        /// <param name="category">The category name.</param>
        /// <returns>The total count of gigs in the category.</returns>
        /// <response code="200">Count retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("stats/category/{category}/count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigCountByCategory(string category)
        {
            try
            {
                var count = await _gigQueryService.GetGigCountByCategoryAsync(category);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves the total count of gigs for a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <returns>The total count of gigs for the seller.</returns>
        /// <response code="200">Count retrieved successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("stats/seller/{sellerId:int}/count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGigCountBySellerId(int sellerId)
        {
            try
            {
                var count = await _gigQueryService.GetGigCountBySellerIdAsync(sellerId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Checks if a gig exists.
        /// </summary>
        /// <param name="id">The unique identifier of the gig.</param>
        /// <returns>True if the gig exists, false otherwise.</returns>
        /// <response code="200">Existence check completed successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GigExists(int id)
        {
            try
            {
                var exists = await _gigQueryService.GigExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}