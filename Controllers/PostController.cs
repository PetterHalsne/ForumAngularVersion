﻿using Microsoft.AspNetCore.Mvc;
using ForumAngularVersion.Models;
using ForumAngularVersion.DAL;

namespace ForumAngularVersion.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostRepository postRepository, ILogger<PostController> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postRepository.GetAll();
        if (posts == null)
        {
            _logger.LogError("[PostController] Post list not found while executing _postRepository.GetAll()");
            return NotFound("Post list not found");
        }
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        try
        {
            var post = await _postRepository.GetPostById(id);

            if (post == null)
            {
                _logger.LogError("[PostController] post id not found while executing _postRepository.GetPostById()");
                return NotFound();
            }

            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting post with ID {Id}", id);
            return StatusCode(500, "An error occurred.");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] Post newPost) {
       if(newPost == null)
        {
            return BadRequest("Invalid Post data");
        }
       bool returnOK = await _postRepository.Create(newPost);
        if(returnOK)
        {
            var response = new { success = true, message = "Post " + newPost.PostTitle + " created successfully" };
            return Ok(response);
        }
        else
        {
            var response = new { success = false, message = "Post creation failed" };
            return Ok(response);
        }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Post updatedPost)
    {
        if (updatedPost == null)
        {
            return BadRequest("Invalid post data.");
        }
        bool returnOk = await _postRepository.Update(updatedPost);
        if (returnOk)
        {
            var response = new { success = true, message = "Post " + updatedPost.PostTitle + " updated successfully" };
            return Ok(response);
        }
        else
        {
            var response = new { success = false, message = "Post update failed" };
            return Ok(response);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        bool returnOk = await _postRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[PostController] Post deletion failed for the PostId {PostId:0000}", id);
            return BadRequest("Post deletion failed");
        }
        var response = new { success = true, message = "Post " + id.ToString() + " deletion succesfully" };
        return Ok(response);
    }






}