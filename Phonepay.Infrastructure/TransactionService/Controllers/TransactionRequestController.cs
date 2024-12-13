using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionRequestsController(IRepositoryById<TransactionRequest> repository) : Controller
{
    [HttpPost("post")]
    public async Task<ActionResult<TransactionRequest>> Post([FromBody] TransactionRequest entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Post and verify existence
        var transactionRequest = await repository.Post(entry);
        if (transactionRequest == null)
            return BadRequest("Failed to create the object. Please verify the input data and try again."); // Return 400

        // Return 201
        return Created(Url.Action(nameof(Get), new { id = transactionRequest.ID }), transactionRequest);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<TransactionRequest>> Get(int id)
    {
        // Get and verify existence
        var transactionRequest = await repository.Get(id);
        if (transactionRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Return 200
        return Ok(transactionRequest);
    }

    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<TransactionRequest>>> GetAll(int userId)
    {
        // Get all
        var transactionRequests = await repository.GetAll(userId);

        // Return 200
        return Ok(transactionRequests);
    }

    [HttpPut("put/{id}")]
    public async Task<ActionResult<TransactionRequest>> Put(int id, [FromBody] TransactionRequest entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Verify id
        if (id != entry.ID)
            return BadRequest("ID does not match Entry ID."); // Return 400

        // Get and verify existence
        var transactionRequest = await repository.Get(id);
        if (transactionRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404 - (OPTIONAL)

        // Put
        transactionRequest = await repository.Put(entry);

        // Return 200
        return Ok(transactionRequest);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        // Get and verify existence
        var transactionRequest = await repository.Get(id);
        if (transactionRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Delete
        await repository.Delete(id);

        // Return 204
        return NoContent();
    }
}