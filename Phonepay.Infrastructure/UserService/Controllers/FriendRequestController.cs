﻿using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FriendRequestController(IRepositoryById<FriendRequest> repository) : Controller
{
    [HttpPost("post")]
    public async Task<ActionResult<FriendRequest>> Post([FromBody] FriendRequest entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Post and verify existence
        var friendRequest = await repository.Post(entry);
        if (friendRequest == null)
            return BadRequest("Failed to create the object. Please verify the input data and try again."); // Return 400

        // Return 201
        return Created(Url.Action(nameof(Get), new { id = friendRequest.ID }), friendRequest);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<FriendRequest>> Get(int id)
    {
        // Get and verify existence
        var friendRequest = await repository.Get(id);
        if (friendRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Return 200
        return Ok(friendRequest);
    }

    [HttpGet("getall/{userId}")]
    public async Task<ActionResult<IEnumerable<FriendRequest>>> GetAll(int userId)
    {
        // Get all
        var friendRequests = await repository.GetAll(userId);

        // Return 200
        return Ok(friendRequests);
    }

    [HttpPut("put/{id}")]
    public async Task<ActionResult<FriendRequest>> Put(int id, [FromBody] FriendRequest entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Verify id
        if (id != entry.ID)
            return BadRequest("ID does not match Entry ID."); // Return 400

        // Get and verify existence
        var friendRequest = await repository.Get(id);
        if (friendRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404 - (OPTIONAL)

        // Put
        friendRequest = await repository.Put(entry);

        // Return 200
        return Ok(friendRequest);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        // Get and verify existence
        var friendRequest = await repository.Get(id);
        if (friendRequest == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Delete
        await repository.Delete(id);

        // Return 204
        return NoContent();
    }
}