using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HoThiKy_Audio_Guide
{
    public class DatabaseHelper
    {
        // 1. Lấy chuỗi kết nối từ file App.config
        private string connectionString = ConfigurationManager.ConnectionStrings["PhoAmThucConn"].ConnectionString;

        // 2. Hàm kiểm tra kết nối (Bước 2)
        public bool TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    return true; // Kết nối thành công
                }
                catch (Exception)
                {
                    return false; // Thất bại
                }
            }
        }

        // 3. Hàm lấy dữ liệu mẫu (Bước 3) - DÁN VÀO KHÚC NÀY
        public DataTable GetDataTable(string query)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            conn.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            // Xử lý lỗi nếu có
                            Console.WriteLine("Lỗi truy vấn: " + ex.Message);
                        }
                    }
                }
            }
            return dataTable;
        }
    }
}