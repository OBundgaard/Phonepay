using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendshipController(IRepositoryById<Friendship> repository) : Controller
{
    [HttpPost("post")]
    public async Task<ActionResult<Friendship>> Post([FromBody] Friendship entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Post and verify existence
        var friendship = await repository.Post(entry);
        if (friendship == null)
            return BadRequest("Failed to create the object. Please verify the input data and try again."); // Return 400

        // Return 201
        return Created(Url.Action(nameof(Get), new { id = friendship.ID }), friendship);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<Friendship>> Get(int id)
    {
        // Get and verify existence
        var friendship = await repository.Get(id);
        if (friendship == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Return 200
        return Ok(friendship);
    }

    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<Friendship>>> GetAll(int userId)
    {
        // Get all
        var friendships = await repository.GetAll(userId);

        // Return 200
        return Ok(friendships);
    }

    [HttpPut("put/{id}")]
    public async Task<ActionResult<Friendship>> Put(int id, [FromBody] Friendship entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Verify id
        if (id != entry.ID)
            return BadRequest("ID does not match Entry ID."); // Return 400

        // Get and verify existence
        var friendship = await repository.Get(id);
        if (friendship == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404 - (OPTIONAL)

        // Put
        friendship = await repository.Put(entry);

        // Return 200
        return Ok(friendship);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        // Get and verify existence
        var friendship = await repository.Get(id);
        if (friendship == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Delete
        await repository.Delete(id);

        // Return 204
        return NoContent();
    }
}