using Dapper;
using backend.Database;
using backend.Models;

namespace backend.Repository;

public class UserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all users from database.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<User>(
            "SELECT * FROM users"
        );
    }

    /// <summary>
    /// Get a user by their ID.
    /// Returns null if user does not exist.
    /// </summary>
    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE id = @Id",
            new { Id = id }
        );
    }

    /// <summary>
    /// Find a user by username.
    /// Useful for login and duplicate username checks.
    /// </summary>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE username = @Username",
            new { Username = username }
        );
    }

    /// <summary>
    /// Find a user by email.
    /// Useful for registration and duplicate email checks.
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email = @Email",
            new { Email = email }
        );
    }

    /// <summary>
    /// Get all users with a specific role.
    /// Example: admin, owner.
    /// </summary>
    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<User>(
            "SELECT * FROM users WHERE role = @Role",
            new { Role = role }
        );
    }

    /// <summary>
    /// Get all active users.
    /// </summary>
    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        using var connection = _db.CreateConnection();

        return await connection.QueryAsync<User>(
            "SELECT * FROM users WHERE is_active = 1"
        );
    }
}