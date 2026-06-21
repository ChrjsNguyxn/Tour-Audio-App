using backend.Database;
using backend.Models;
using Dapper;

namespace backend.Repository;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    //========
    // GET
    //========
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<User>(
            "SELECT * FROM users"
        );
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM users
            WHERE id = @Id
            """,
            new { Id = id }
        );
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM users
            WHERE username = @Username
            """,
            new { Username = username }
        );
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            """
            SELECT *
            FROM users
            WHERE email = @Email
            """,
            new { Email = email }
        );
    }

    /// <summary>
    /// get user by role
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<User>(
            """
            SELECT *
            FROM users
            WHERE role = @Role
            """,
            new { Role = role }
        );
    }

    //=========
    // CREATE
    //=========

    public async Task<int> CreateAsync(User user)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO users
            (
                username,
                password,
                role,
                full_name,
                email,
                avatar_url
            )
            VALUES
            (
                @Username,
                @Password,
                @Role,
                @FullName,
                @Email,
                @AvatarUrl
            );

            SELECT last_insert_rowid();
            """,
            user
        );
    }


    //=========
    // UPDATE
    //=========
    
    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            UPDATE users
            SET
                full_name = @FullName,
                email = @Email,
                avatar_url = @AvatarUrl,
                role = @Role,
                is_active = @IsActive
            WHERE id = @Id
            """,
            user
        );

        return rows > 0;
    }

    //================================
    // DISABLE - is_active = false/0
    //================================


    /// <summary>
    /// Disable user(is_active = false or 0)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DisableAsync(int id)
    {
        using var connection = _context.CreateConnection();

        var rows = await connection.ExecuteAsync(
            """
            UPDATE users
            SET is_active = 0
            WHERE id = @Id
            """,
            new { Id = id }
        );

        return rows > 0;
    }
}



// finish user repo