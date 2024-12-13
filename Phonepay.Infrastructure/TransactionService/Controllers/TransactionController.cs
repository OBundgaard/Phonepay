using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(IRepositoryById<Transaction> repository) : Controller
{
    [HttpPost("post")]
    public async Task<ActionResult<Transaction>> Post([FromBody] Transaction entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Post and verify existence
        var transaction = await repository.Post(entry);
        if (transaction == null)
            return BadRequest("Failed to create the object. Please verify the input data and try again."); // Return 400

        // Return 201
        return Created(Url.Action(nameof(Get), new { id = transaction.ID }), transaction);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<Transaction>> Get(int id)
    {
        // Get and verify existence
        var transaction = await repository.Get(id);
        if (transaction == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Return 200
        return Ok(transaction);
    }

    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll(int userId)
    {
        // Get all
        var transactions = await repository.GetAll(userId);

        // Return 200
        return Ok(transactions);
    }

    [HttpPut("put/{id}")]
    public async Task<ActionResult<Transaction>> Put(int id, [FromBody] Transaction entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Verify id
        if (id != entry.ID)
            return BadRequest("ID does not match Entry ID."); // Return 400

        // Get and verify existence
        var transaction = await repository.Get(id);
        if (transaction == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404 - (OPTIONAL)

        // Put
        transaction = await repository.Put(entry);

        // Return 200
        return Ok(transaction);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        // Get and verify existence
        var transaction = await repository.Get(id);
        if (transaction == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Delete
        await repository.Delete(id);

        // Return 204
        return NoContent();
    }
}