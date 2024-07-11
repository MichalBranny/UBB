using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using SimpleNotes.Models;

namespace SimpleNotes
{
    public class AppDbContext : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;

            // Ensure the database and table exist
            EnsureDatabaseAndTableExist();

            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        private void EnsureDatabaseAndTableExist()
        {
            var masterConnectionString = "Data Source=DESKTOP-GONFCJC;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

                // Check if the database exists
                var checkDbCmd = new SqlCommand("IF DB_ID('SimpleNotesDb') IS NULL CREATE DATABASE SimpleNotesDb;", connection);
                checkDbCmd.ExecuteNonQuery();

                // Check if the table exists and create it if it does not, also add an example row
                var checkTableCmd = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM SimpleNotesDb.sys.tables WHERE name = 'Notes')
                    BEGIN
                        USE SimpleNotesDb;
                        CREATE TABLE Notes
                        (
                            Id INT PRIMARY KEY IDENTITY,
                            Title NVARCHAR(100) NOT NULL,
                            Content NVARCHAR(MAX) NOT NULL,
                            Category NVARCHAR(100),
                            CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
                            ModificationDate DATETIME NOT NULL DEFAULT GETDATE()
                        );

                        INSERT INTO Notes (Title, Content, Category, CreationDate, ModificationDate) VALUES 
                        ('Example Note', 'This is an example note.', 'General', GETDATE(), GETDATE());
                    END
                    ELSE
                    BEGIN
                        USE SimpleNotesDb;
                        IF COL_LENGTH('Notes', 'Category') IS NULL
                        ALTER TABLE Notes ADD Category NVARCHAR(100);

                        IF COL_LENGTH('Notes', 'CreationDate') IS NULL
                        ALTER TABLE Notes ADD CreationDate DATETIME NOT NULL DEFAULT GETDATE();

                        IF COL_LENGTH('Notes', 'ModificationDate') IS NULL
                        ALTER TABLE Notes ADD ModificationDate DATETIME NOT NULL DEFAULT GETDATE();
                    END", connection);
                checkTableCmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Note> GetNotes()
        {
            return _connection.Query<Note>("SELECT * FROM Notes");
        }

        public void InsertNote(Note note)
        {
            note.CreationDate = DateTime.Now;
            note.ModificationDate = DateTime.Now;
            _connection.Execute("INSERT INTO Notes (Title, Content, Category, CreationDate, ModificationDate) VALUES (@Title, @Content, @Category, @CreationDate, @ModificationDate)", note);
        }

        public void UpdateNoteContent(Note note)
        {
            _connection.Execute("UPDATE Notes SET Content = @Content, Category = @Category, ModificationDate = @ModificationDate WHERE Id = @Id", note);
        }

        public void DeleteNoteById(int id)
        {
            _connection.Execute("DELETE FROM Notes WHERE Id = @Id", new { Id = id });
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
