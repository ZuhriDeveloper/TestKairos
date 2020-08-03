using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosTest.Models;
using KairosTest.Helpers;
using KairosTest.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace KairosTest.Repository
{
    public class BookRepo : IBook
    {
        private IConfiguration Configuration { get; }

        public BookRepo(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Book GetBookByID(int id)
        {
           Book result = new Book();

            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {
                    cmd.CommandText = @"SELECT ID,BookTitle,Author,BookType,RentPrice FROM Book
                                        WHERE ID = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = CreateObject(reader);
                        }
                    }
                }
            }

            return result;
        }

        public List<Book> GetListBook()
        {
            List<Book> result = new List<Book>();

            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {
                    cmd.CommandText = @"SELECT ID,BookTitle,Author,BookType,RentPrice FROM Book";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book data = CreateObject(reader);
                            result.Add(data);
                        }
                    }
                }
            }

            return result;
        }

        private Book CreateObject(SqlDataReader reader)
        {
            Book result = new Book();

            result.ID = reader.GetInt32(reader.GetOrdinal("ID"));
            result.BookTitle = reader.GetString(reader.GetOrdinal("BookTitle"));
            result.Author = reader.GetString(reader.GetOrdinal("Author"));
            result.BookType = reader.GetString(reader.GetOrdinal("BookType"));
            result.RentPrice = reader.GetDecimal(reader.GetOrdinal("RentPrice"));

            return result;

        }

        public int Create(Book data)
        {
            int result = 0;
            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {

                    cmd.CommandText = @"INSERT INTO Book
                                            ( BookTitle,Author,BookType,RentPrice)
                                    VALUES  ( @BookTitle,@Author,@BookType,@RentPrice);
                                        
                                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@BookTitle", data.BookTitle);
                    cmd.Parameters.AddWithValue("@Author", data.Author);
                    cmd.Parameters.AddWithValue("@BookType", data.BookType);
                    cmd.Parameters.AddWithValue("@RentPrice", data.RentPrice);

                    object lastId = cmd.ExecuteScalar();
                    lastId = (lastId == DBNull.Value) ? null : lastId;
                     result = Convert.ToInt32(lastId);
                }
            }

            return result;
        }

        public void Update(Book data)
        {
            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {

                    cmd.CommandText = @"UPDATE Book
                                           SET BookTitle = @BookTitle
                                            ,Author = @Author
                                            ,BookType = @BookType
                                            ,RentPrice = @RentPrice
                                           WHERE ID = @Id
                                            ";

                    cmd.Parameters.AddWithValue("@BookTitle", data.BookTitle);
                    cmd.Parameters.AddWithValue("@Author", data.Author);
                    cmd.Parameters.AddWithValue("@BookType", data.BookType);
                    cmd.Parameters.AddWithValue("@RentPrice", data.RentPrice);
                    cmd.Parameters.AddWithValue("@ID", data.ID);

                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
