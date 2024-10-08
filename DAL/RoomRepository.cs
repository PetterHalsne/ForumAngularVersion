﻿using ForumAngularVersion.DAL;
using Microsoft.EntityFrameworkCore;
using ForumAngularVersion.Models;

namespace ForumAngularVersion.DAL;



public class RoomRepository : IRoomRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<RoomRepository> _logger;


    // Constructor for the RoomRepository class that takes two parameters: CategoryDbContext and ILogger.
    public RoomRepository(ForumDbContext db, ILogger<RoomRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    //method to gett all rooms asynchronously
    public async Task<IEnumerable<Room>> GetAll()
    {
        //try to find all roms from the databse and retunr them.
        try
        {
            return await _db.Rooms.ToListAsync();
        }
        catch (Exception e)
        {

            _logger.LogError("[RoomRepository]  Room.ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    //Method to get a room by its id
    public async Task<Room?> GetRoomById(int id)
    {
        try
        {
            return await _db.Rooms.FindAsync(id);

        }
        catch (Exception e)
        {
            _logger.LogError("Error while retrieving Room with ID {id}: {e.Message}", id, e.Message);
            return null;
        }

    }

    //Method to get a category id bases on rooms ID
    public async Task<int?> GetCategoryId(int id)
    {
        //Try to fint a room by its ID, and then return the CategoyrID associated with the room.
        try
        {

            var room = await _db.Rooms.FindAsync(id);
            return room.CategoryId;
        }
        catch (Exception e)
        {
            //log an error if it can not find id and then returns null
            _logger.LogError("[RoomRepository]  FindAsync(id) failed when GetCategoryById for CategoryId {CategoryId}, error message: {e}", id, e.Message);
            return null;

        }
    }


    //Method to create a new room
    public async Task<bool> Create(Room room)
    {
        // Add the new room to the database and then saves the changes to database

        try
        {
            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();
            return true; // Indicate success

        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository] room creation failed for room {@room}, error message: {e}", room, e.Message);
            return true; // Indicate success
        }
    }

    // Method to update an existing room


    public async Task<bool> Update(Room room)
    {
        // Update the room in the database and then saves the changes to database.

        try
        {

            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();
            return true;

        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository] update room(room) failed when updating the Room {@room}, error message: {e}", room.RoomId, e.Message);
            return false;
        }
    }

    // Method to delete a room by its ID.
    public async Task<bool> Delete(int id)
    {
        try
        {
            // Try to find the room by its ID.
            var room = await _db.Rooms.FindAsync(id);
            // If the room doesn't exist, return false.

            if (room == null)
            {
                return false;
            }
            // Remove the room from the database and then Save changes to the database asynchronously.
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RoomRepository] room deletion failed for the RoomId {RoomId}, error message: {e}", id, e.Message);
            return false;
        }

    }
}

