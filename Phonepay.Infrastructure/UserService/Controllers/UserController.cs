﻿using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IBaseRepository<User> repository) : Controller
{
    [HttpPost("post")]
    public async Task<ActionResult<User>> Post([FromBody] User entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Post and verify existence
        var user = await repository.Post(entry);
        if (user == null)
            return BadRequest("Failed to create the object. Please verify the input data and try again."); // Return 400

        // Return 201
        return Created(Url.Action(nameof(Get), new { id = user.ID }), user);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        // Get and verify existence
        var user = await repository.Get(id);
        if (user == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Return 200
        return Ok(user);
    }

    [HttpPut("put/{id}")]
    public async Task<ActionResult<User>> Put(int id, [FromBody] User entry)
    {
        // Verify entry
        if (entry == null)
            return BadRequest("Entry cannot be null."); // Return 400

        // Verify id
        if (id != entry.ID)
            return BadRequest("ID does not match Entry ID."); // Return 400

        // Get and verify existence
        var user = await repository.Get(id);
        if (user == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404 - (OPTIONAL)

        // Put
        user = await repository.Put(entry);

        // Return 200
        return Ok(user);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        // Get and verify existence
        var user = await repository.Get(id);
        if (user == null)
            return NotFound($"Object with ID: {id} is not found."); // Return 404

        // Delete
        await repository.Delete(id);

        // Return 204
        return NoContent();
    }
}