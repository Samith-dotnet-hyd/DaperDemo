using DaperDemo.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DaperDemo.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            const string sql = "SELECT Id, Name, Email, EnrolledDate FROM Students";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Student>(sql);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            const string sql = "SELECT Id, Name, Email, EnrolledDate FROM Students WHERE Id = @Id";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Student student)
        {
            const string sql = @"
                INSERT INTO Students (Name, Email, EnrolledDate)
                VALUES (@Name, @Email, @EnrolledDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var conn = CreateConnection();
            var newId = await conn.QuerySingleAsync<int>(sql, student);
            return newId;
        }

        public async Task<bool> UpdateAsync(Student student)
        {
            const string sql = @"
                UPDATE Students
                SET Name = @Name, Email = @Email, EnrolledDate = @EnrolledDate
                WHERE Id = @Id";

            using var conn = CreateConnection();
            var rows = await conn.ExecuteAsync(sql, student);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM Students WHERE Id = @Id";
            using var conn = CreateConnection();
            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
