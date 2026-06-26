using Microsoft.Data.Sqlite;
using System.Data;
using Microsoft.Extensions.Configuration;
using System;

namespace backend.Database;

public class AppDbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        return new SqliteConnection(connectionString);
    }

    // HÀM TỰ ĐỘNG ĐỒNG BỘ VÀ NÂNG CẤP CẤU TRÚC DATABASE
    public void SyncDatabaseSchema()
    {
        try
        {
            using var connection = CreateConnection();
            connection.Open();

            // 1. Kiểm tra xem cột 'status' đã tồn tại chưa
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "PRAGMA table_info(eateries);";
            using var reader = checkCommand.ExecuteReader();
            
            bool hasStatusColumn = false;
            while (reader.Read())
            {
                if (reader["name"].ToString() == "status") 
                {
                    hasStatusColumn = true;
                    break;
                }
            }

            // 2. Nếu chưa có, tiến hành tách lẻ từng lệnh để đảm bảo SQLite không bị ngộp
            if (!hasStatusColumn)
            {
                // Thêm cột status
                using var cmd1 = connection.CreateCommand();
                cmd1.CommandText = "ALTER TABLE eateries ADD COLUMN status TEXT DEFAULT 'pending';";
                cmd1.ExecuteNonQuery();

                // Thêm cột action_reason
                using var cmd2 = connection.CreateCommand();
                cmd2.CommandText = "ALTER TABLE eateries ADD COLUMN action_reason TEXT;";
                cmd2.ExecuteNonQuery();

                // Đồng bộ dữ liệu cũ (Duyệt rồi thì thành approved)
                using var cmd3 = connection.CreateCommand();
                cmd3.CommandText = "UPDATE eateries SET status = 'approved' WHERE is_approved = 1;";
                cmd3.ExecuteNonQuery();

                // Đồng bộ dữ liệu cũ (Chưa duyệt thì thành pending)
                using var cmd4 = connection.CreateCommand();
                cmd4.CommandText = "UPDATE eateries SET status = 'pending' WHERE is_approved = 0;";
                cmd4.ExecuteNonQuery();
                
                Console.WriteLine("[DB Sync] Đã cập nhật thành công cấu trúc bảng eateries (Thêm Status & Reason)!");
            }
        }
        catch (Exception ex)
        {
            // Bắt lỗi đỏ ra Terminal thay vì làm sập cả Server
            Console.WriteLine($"[DB Sync Error] Lỗi khi đồng bộ Database: {ex.Message}");
        }
    }
}