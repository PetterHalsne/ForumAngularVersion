﻿
using ForumAngularVersion.DAL;
using Microsoft.EntityFrameworkCore;
using ForumAngularVersion.Models;

namespace ForumAngularVersion.DAL;

public class PostRepository : IPostRepository
{
    private readonly ForumDbContext _db;

    private readonly ILogger<PostRepository> _logger;

    public PostRepository(ForumDbContext db, ILogger<PostRepository> logger)
    {
        _db = db;
        _logger = logger;
    }



    public async Task<IEnumerable<Post>?> GetAll()
    {
        try
        {
            return await _db.Posts.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] Post.ToListAsync() failed when GetAll(), error message: {e}", e.Message);// Log an error if the operation fails and return null.
            return null;
        }

    }

    //A method to get a topic's ID based on a post's ID.
    public async Task<int?> GetTopicId(int id)
    {
        try
        {
            var post = await _db.Posts.FindAsync(id);
            return post.TopicId;
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository]  Posts.FindAsync(id) failed when GetItemById for PostId {PostId:0000}, error message: {e}", id, e.Message);
            return null;

        }

    }

    // A method to get a post by its ID.
    public async Task<Post?> GetPostById(int id)
    {
        // Try to find and return a post based on its ID.
        try
        {
            return await _db.Posts.FindAsync(id);

        }
        catch (Exception e)
        {
            _logger.LogError("Error while retrieving post with ID {id}: {e.Message}", id, e.Message);
            return null;
        }
    }

    //Mehtod to create a new post
    public async Task<bool> Create(Post post)
    {
        try
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return true; // Indicate success
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] post creation failed for post {@post}, error message: {e}", post, e.Message);
            return false;
        }
    }

    // Method to update an existing post.

    public async Task<bool> Update(Post post)
    {

        try
        {
            // Update the post in the database 
            _db.Posts.Update(post);

            // Save changes to the database asynchronously.
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] post update(id) failed when updating the PostId {PostId:0000}, error message: {e}", post, e.Message);
            return false;
        }
    }


    // method to delete a post by its ID.
    public async Task<bool> Delete(int id)
    {
        try
        {
            // Try to find the post by its ID.
            var post = await _db.Posts.FindAsync(id);

            // If the post doesn't exist, return false.
            if (post == null)
            {
                return false;
            }

            // Remove the post from the database and then save the change.

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[PostRepository] post deletion failed for the PostId {PostId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }

}