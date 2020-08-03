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
    public class RentBookRepo : IRentBook
    {

        private IConfiguration Configuration { get; }
        private IBook BookMgr { get; }

        public RentBookRepo(IConfiguration configuration)
        {
            Configuration = configuration;
            BookMgr = new BookRepo(configuration);
        }
        public int Create(RentBook data)
        {
            int result = 0;
            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {

                    cmd.CommandText = @"INSERT INTO RentBook
                                            (BookID,PricePerDay,RentLenght,StartDate,EndDate,UserName)
                                    VALUES  (@BookID,@PricePerDay,@RentLenght,@StartDate,@EndDate,@UserName);
                                        
                                    SELECT SCOPE_IDENTITY()";

                    cmd.Parameters.AddWithValue("@BookID", data.BookID);
                    cmd.Parameters.AddWithValue("@PricePerDay", data.PricePerDay);
                    cmd.Parameters.AddWithValue("@RentLenght", data.RentLenght);
                    cmd.Parameters.AddWithValue("@StartDate", data.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", data.EndDate);
                    cmd.Parameters.AddWithValue("@UserName", data.UserName);

                    object lastId = cmd.ExecuteScalar();
                    lastId = (lastId == DBNull.Value) ? null : lastId;
                    result = Convert.ToInt32(lastId);
                }
            }

            return result;
        }

        public List<RentBook> GetList()
        {
            List<RentBook> result = new List<RentBook>();

            using (var tx = new SafeTx(Configuration))
            {
                using (var cmd = tx.GetCommand())
                {
                    cmd.CommandText = @"SELECT ID,BookID,PricePerDay,RentLenght,StartDate,EndDate,UserName FROM RentBook";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RentBook data = CreateObject(reader);
                            result.Add(data);
                        }
                    }
                }
            }

            return result;
        }

        private RentBook CreateObject(SqlDataReader reader)
        {
            RentBook result = new RentBook();

            result.ID = reader.GetInt32(reader.GetOrdinal("ID"));
            result.BookID = reader.GetInt32(reader.GetOrdinal("BookID"));
            result.PricePerDay = reader.GetDecimal(reader.GetOrdinal("PricePerDay"));
            result.RentLenght = reader.GetInt32(reader.GetOrdinal("RentLenght"));
            result.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
            result.EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"));
            result.UserName = reader.GetString(reader.GetOrdinal("UserName"));
            result.Book = BookMgr.GetBookByID(result.BookID);
            result.Total = result.RentLenght * result.PricePerDay;
            return result;

        }
    }
}
