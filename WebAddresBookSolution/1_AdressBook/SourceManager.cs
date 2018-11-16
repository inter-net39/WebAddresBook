using _1_AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace _1_AdressBook
{
    public class SourceManager
    {
        private string _connectionString = @"Data Source=DESKTOP-11PTBON\SQLEXPRESS;Initial Catalog=AddressBookDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection _connection;

        public List<PersonModel> Get(int start, int take)
        {
            List<PersonModel> temporaryList = new List<PersonModel>();
            if (start >= 0 && take > 0)
            {
                if (TryOpenDB())
                {
                    SqlTransaction transaction = _connection.BeginTransaction();
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "SELECT * " +
                                      "FROM [AddressBookDB].[dbo].[People] ",
                        CommandType = CommandType.Text,
                        Connection = _connection,
                        Transaction = transaction
                    };
                    try
                    {
                        using (SqlDataReader sqlReader = cmd.ExecuteReader())
                        {
                            if (sqlReader.HasRows)
                            {
                                while (sqlReader.Read())
                                {
                                    temporaryList.Add(new PersonModel(Convert.ToInt32(sqlReader.GetValue(0)),
                                        Convert.ToString(sqlReader.GetValue(1)),
                                        Convert.ToString(sqlReader.GetValue(2)),
                                        Convert.ToInt32(sqlReader.GetValue(3)),
                                        Convert.ToString(sqlReader.GetValue(4)),
                                        Convert.ToDateTime(sqlReader.GetValue(5)),
                                        (sqlReader.GetValue(6) as DateTime?)
                                           ));
                                }
                            }
                            else
                            {
                                throw new Exception("#098");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        CloseDB();
                    }

                    return temporaryList.Skip(start).Take(take).ToList();
                }
            }
            else
            {
                throw new Exception("#092");
            }
            return null;
        }

        public PersonModel GetByID(int id)
        {
            PersonModel person = null;
            if (id > 1)
            {
                if (TryOpenDB())
                {
                    SqlTransaction transaction = _connection.BeginTransaction();
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "SELECT * " +
                                      "FROM [AddressBookDB].[dbo].[People] " +
                                      "WHERE [ID] = @id",
                        CommandType = CommandType.Text,
                        Connection = _connection,
                        Transaction = transaction
                    };
                    SqlParameter myID = new SqlParameter()
                    {
                        ParameterName = "@id",
                        Value = id,
                        DbType = DbType.Int32
                    };
                    cmd.Parameters.Add(myID);
                    try
                    {
                        using (SqlDataReader sqlReader = cmd.ExecuteReader())
                        {
                            if (sqlReader.HasRows)
                            {
                                while (sqlReader.Read())
                                {
                                    person = new PersonModel(Convert.ToInt32(sqlReader.GetValue(0)),
                                         Convert.ToString(sqlReader.GetValue(1)),
                                         Convert.ToString(sqlReader.GetValue(2)),
                                         Convert.ToInt32(sqlReader.GetValue(3)),
                                         Convert.ToString(sqlReader.GetValue(4)),
                                         Convert.ToDateTime(sqlReader.GetValue(5)),
                                         Convert.ToDateTime(sqlReader.GetValue(6))
                                            );
                                }
                            }
                            else
                            {
                                throw new Exception("#09325668");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("#024112");
                    }
                    finally
                    {
                        CloseDB();
                    }

                    return person;
                }
            }
            else
            {
                throw new Exception("#09669492");
            }
            return person;
        }

        public int Add(PersonModel personModel)
        {
            int result = -1;
            if (TryOpenDB())
            {
                SqlTransaction transaction = _connection.BeginTransaction();
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "INSERT INTO [AddressBookDB].[dbo].[People] " +
                                  "VALUES( @FirstName, @LastName, @Phone, @Email ,@Created );" +
                                  "SELECT SCOPE_IDENTITY(); ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                    Transaction = transaction
                };
                SqlParameter FirstName = new SqlParameter()
                {
                    ParameterName = "@FirstName",
                    Value = personModel.FirstName,
                    DbType = DbType.String
                };
                SqlParameter LastName = new SqlParameter()
                {
                    ParameterName = "@FirstName",
                    Value = personModel.LastName,
                    DbType = DbType.String
                };
                SqlParameter Phone = new SqlParameter()
                {
                    ParameterName = "@Phone",
                    Value = personModel.Phone,
                    DbType = DbType.Int32
                };
                SqlParameter Email = new SqlParameter()
                {
                    ParameterName = "@Email",
                    Value = personModel.Email,
                    DbType = DbType.String
                };
                SqlParameter Created = new SqlParameter()
                {
                    ParameterName = "@Created",
                    Value = DateTime.Now,
                    DbType = DbType.DateTime
                };
                //SqlParameter Updated = new SqlParameter()
                //{
                //    ParameterName = "@Updated",
                //    Value = personModel.Updated.Value,
                //    DbType = DbType.DateTime
                //};
                cmd.Parameters.Add(FirstName);
                cmd.Parameters.Add(LastName);
                cmd.Parameters.Add(Phone);
                cmd.Parameters.Add(Email);
                cmd.Parameters.Add(Created);
                //cmd.Parameters.Add(Updated);
                try
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new Exception("#024112");
                }
                finally
                {
                    CloseDB();
                }

                return result;
            }
            throw new Exception("#32250660");
        }

        public void Update(PersonModel personModel)
        {
            if (TryOpenDB())
            {
                SqlTransaction transaction = _connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "UPDATE [AddressBookDB].[dbo].[People]" +
                                  "SET [FirstName] = @FirstName," +
                                  "    [LastName] = @LastName," +
                                  "    [Phone] = @Phone," +
                                  "    [Email] = @Email, " +
                                  "    [Created] = @Created " +
                                  "    [Updated] = @Updated" +
                                  "WHERE ID = @ID",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                    Transaction = transaction
                };
                SqlParameter myID = new SqlParameter()
                {
                    ParameterName = "@ID",
                    Value = personModel.ID,
                    DbType = DbType.Int32
                };
                SqlParameter FirstName = new SqlParameter()
                {
                    ParameterName = "@FirstName",
                    Value = personModel.FirstName,
                    DbType = DbType.String
                };
                SqlParameter LastName = new SqlParameter()
                {
                    ParameterName = "@FirstName",
                    Value = personModel.LastName,
                    DbType = DbType.String
                };
                SqlParameter Phone = new SqlParameter()
                {
                    ParameterName = "@Phone",
                    Value = personModel.Phone,
                    DbType = DbType.Int32
                };
                SqlParameter Email = new SqlParameter()
                {
                    ParameterName = "@Email",
                    Value = personModel.Email,
                    DbType = DbType.String
                };
                SqlParameter Created = new SqlParameter()
                {
                    ParameterName = "@Created",
                    Value = personModel.Created,
                    DbType = DbType.DateTime
                };
                SqlParameter Updated = new SqlParameter()
                {
                    ParameterName = "@Updated",
                    Value = personModel.Updated.Value,
                    DbType = DbType.DateTime
                };
                cmd.Parameters.Add(myID);
                cmd.Parameters.Add(FirstName);
                cmd.Parameters.Add(LastName);
                cmd.Parameters.Add(Phone);
                cmd.Parameters.Add(Email);
                cmd.Parameters.Add(Created);
                cmd.Parameters.Add(Updated);
                try
                {
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    new Exception("#86868633");
                }
                finally
                {
                    CloseDB();
                }
            }
            else
            {
                new Exception("#54338838");
            }
        }

        public void Remove(int id)
        {
            if (TryOpenDB())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM [AddressBookDB].[dbo].[People] " +
                                  "WHERE ID = @ID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _connection;

                SqlParameter para1 = new SqlParameter()
                {
                    ParameterName = "@ID",
                    Value = id,
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Input,
                };
                cmd.Parameters.Add(para1);
                try
                {
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        new Exception("32252222995");
                    }
                }
                catch (Exception)
                {
                    new Exception("$112265865854732");
                }
                finally
                {
                    CloseDB();
                }
            }
            else
            {
                new Exception("$11246262");
            }
        }

        private bool TryOpenDB()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection()
                {
                    ConnectionString = _connectionString,
                };
                _connection.Open();
                if (_connection.State == ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            throw new Exception("#054322");
        }

        protected void CloseDB()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                _connection = null;
            }
            else
            {
                throw new Exception("#0532252525086694322");
            }
        }

        private void _addLog(string str)
        {
            using (StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Dane.csv"), true))
            {
                sw.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} - {str}");
            }
        }
    }
}