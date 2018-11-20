using _1_AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace _1_AdressBook
{
    public class SourceManager
    {
        private readonly string _setupFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "setup.txt");

        private string _connectionString = "";

        private SqlConnection _connection;

        public List<PersonModel> Get(int start, int take)
        {
            List<PersonModel> temporaryList = new List<PersonModel>();
            if (start >= 0 && take > 0)
            {
                if (TryOpenDB())
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "SELECT * " +
                                      "FROM [AddressBookDB].[dbo].[People] " +
                                      "ORDER BY ID " +
                                      "OFFSET @start ROWS " +
                                      "FETCH NEXT @take ROWS ONLY;",
                        CommandType = CommandType.Text,
                        Connection = _connection,
                    };
                    SqlParameter para1 = new SqlParameter()
                    {
                        ParameterName = "@start",
                        Value = start,
                        DbType = DbType.Int32
                    };
                    SqlParameter para2 = new SqlParameter()
                    {
                        ParameterName = "@take",
                        Value = take,
                        DbType = DbType.Int32
                    };
                    cmd.Parameters.Add(para1);
                    cmd.Parameters.Add(para2);
                    try
                    {
                        using (SqlDataReader sqlReader = cmd.ExecuteReader())
                        {
                            if (sqlReader.HasRows)
                            {
                                while (sqlReader.Read())
                                {
                                    temporaryList.Add(new PersonModel(
                                        Convert.ToInt32(sqlReader.GetValue(0)),
                                        Convert.ToString(sqlReader.GetValue(1)),
                                        Convert.ToString(sqlReader.GetValue(2)),
                                        Convert.ToInt32(sqlReader.GetValue(3)),
                                        Convert.ToString(sqlReader.GetValue(4)),
                                        Convert.ToDateTime(sqlReader.GetValue(5)),
                                        (sqlReader.GetValue(6) as DateTime?)
                                           ));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        CloseDB();
                    }
                }
            }

            if (temporaryList != null)
            {
                return temporaryList;
            }

            return null;
        }
        public List<PersonModel> Get(int start, int take, string filter)
        {
            List<PersonModel> temporaryList = new List<PersonModel>();
            if (start >= 0 && take > 0)
            {
                if (TryOpenDB())
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "SELECT * " +
                                      "FROM [AddressBookDB].[dbo].[People] " +
                                      "WHERE LastName LIKE @filter " +
                                      "ORDER BY ID " +
                                      "OFFSET @start ROWS " +
                                      "FETCH NEXT @take ROWS ONLY;",
                        CommandType = CommandType.Text,
                        Connection = _connection,
                    };
                    SqlParameter para1 = new SqlParameter()
                    {
                        ParameterName = "@start",
                        Value = start,
                        DbType = DbType.Int32
                    };
                    SqlParameter para2 = new SqlParameter()
                    {
                        ParameterName = "@take",
                        Value = take,
                        DbType = DbType.Int32
                    };
                    SqlParameter para3 = new SqlParameter()
                    {
                        ParameterName = "@filter",
                        Value = "%"+filter+"%",
                        DbType = DbType.String
                    };
                    cmd.Parameters.Add(para1);
                    cmd.Parameters.Add(para2);
                    cmd.Parameters.Add(para3);
                    try
                    {
                        using (SqlDataReader sqlReader = cmd.ExecuteReader())
                        {
                            if (sqlReader.HasRows)
                            {
                                while (sqlReader.Read())
                                {
                                    temporaryList.Add(new PersonModel(
                                        Convert.ToInt32(sqlReader.GetValue(0)),
                                        Convert.ToString(sqlReader.GetValue(1)),
                                        Convert.ToString(sqlReader.GetValue(2)),
                                        Convert.ToInt32(sqlReader.GetValue(3)),
                                        Convert.ToString(sqlReader.GetValue(4)),
                                        Convert.ToDateTime(sqlReader.GetValue(5)),
                                        (sqlReader.GetValue(6) as DateTime?)
                                           ));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        CloseDB();
                    }
                }
            }

            if (temporaryList != null)
            {
                return temporaryList;
            }

            return null;
        }

        public int GetCount()
        {
            int result = 0;
            if (TryOpenDB())
            {
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "SELECT COUNT(DISTINCT ID) " +
                                  "FROM [AddressBookDB].[dbo].[People] ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                };
                try
                {
                    using (SqlDataReader sqlReader = cmd.ExecuteReader())
                    {
                        if (sqlReader.HasRows)
                        {
                            while (sqlReader.Read())
                            {
                                result = Convert.ToInt32(sqlReader.GetValue(0));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    CloseDB();
                }
            }
            return result;
        }
        public int GetCount(string filter)
        {
            int result = 0;
            if (TryOpenDB())
            {
                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "SELECT COUNT(DISTINCT ID) " +
                                  "FROM [AddressBookDB].[dbo].[People] " +
                                  "WHERE LastName LIKE @filter ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                };
                SqlParameter para1 = new SqlParameter()
                {
                    ParameterName = "@filter",
                    Value = "%"+filter+"%",
                    DbType = DbType.String
                };
                cmd.Parameters.Add(para1);
                try
                {
                    using (SqlDataReader sqlReader = cmd.ExecuteReader())
                    {
                        if (sqlReader.HasRows)
                        {
                            while (sqlReader.Read())
                            {
                                result = Convert.ToInt32(sqlReader.GetValue(0));
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    CloseDB();
                }
            }
            return result;
        }
        public PersonModel GetByID(int id)
        {
            PersonModel person = null;
            if (id >= 1)
            {
                if (TryOpenDB())
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "SELECT * " +
                                      "FROM [AddressBookDB].[dbo].[People] " +
                                      "WHERE [ID] = @id",
                        CommandType = CommandType.Text,
                        Connection = _connection,
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
                                    person = new PersonModel(
                                         Convert.ToInt32(sqlReader.GetValue(0)),
                                         Convert.ToString(sqlReader.GetValue(1)),
                                         Convert.ToString(sqlReader.GetValue(2)),
                                         Convert.ToInt32(sqlReader.GetValue(3)),
                                         Convert.ToString(sqlReader.GetValue(4)),
                                         Convert.ToDateTime(sqlReader.GetValue(5)),
                                         (sqlReader.GetValue(6) as DateTime?)
                                            );
                                }
                            }
                            else
                            {
                                throw new Exception("#09325668");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
                    CommandText = "INSERT INTO [AddressBookDB].[dbo].[People] ( [FirstName], [LastName], [Phone], [Email], [Created], [Updated] ) " +
                                  "VALUES ( @FirstName, @LastName, @Phone, @Email ,@Created, @Updated ); " +
                                  "SELECT SCOPE_IDENTITY(); ",
                    CommandType = CommandType.Text,
                    Connection = _connection,
                    Transaction = transaction
                };
                SqlParameter FirstName = new SqlParameter()
                {
                    ParameterName = "@FirstName",
                    Value = _firstToUpper(personModel.FirstName),
                    DbType = DbType.String
                };
                SqlParameter LastName = new SqlParameter()
                {
                    ParameterName = "@LastName",
                    Value = _firstToUpper(personModel.LastName),
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
                    Value = personModel.Email.ToLower(),
                    DbType = DbType.String
                };
                SqlParameter Created = new SqlParameter()
                {
                    ParameterName = "@Created",
                    Value = DateTime.Now,
                    DbType = DbType.DateTime
                };
                SqlParameter Updated = new SqlParameter()
                {
                    ParameterName = "@Updated",
                    Value = (object)personModel.Updated ?? DBNull.Value,
                    DbType = DbType.DateTime
                };

                cmd.Parameters.Add(FirstName);
                cmd.Parameters.Add(LastName);
                cmd.Parameters.Add(Phone);
                cmd.Parameters.Add(Email);
                cmd.Parameters.Add(Created);
                cmd.Parameters.Add(Updated);
                try
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    CloseDB();
                }
            }
            return result;
        }

        public int Update(PersonModel personModel)
        {
            int result = -1;
            if (TryOpenDB())
            {
                SqlTransaction transaction = _connection.BeginTransaction();

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "UPDATE [AddressBookDB].[dbo].[People] " +
                                  "SET [FirstName] = @FirstName, " +
                                  "    [LastName] = @LastName, " +
                                  "    [Phone] = @Phone, " +
                                  "    [Email] = @Email, " +
                                  "    [Updated] = @Updated " +
                                  "WHERE ID = @ID ",

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
                    Value = _firstToUpper(personModel.FirstName),
                    DbType = DbType.String
                };
                SqlParameter LastName = new SqlParameter()
                {
                    ParameterName = "@LastName",
                    Value = _firstToUpper(personModel.LastName),
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
                    Value = personModel.Email.ToLower(),
                    DbType = DbType.String
                };

                SqlParameter Updated = new SqlParameter()
                {
                    ParameterName = "@Updated",
                    Value = DateTime.Now,
                    DbType = DbType.DateTime
                };
                cmd.Parameters.Add(myID);
                cmd.Parameters.Add(FirstName);
                cmd.Parameters.Add(LastName);
                cmd.Parameters.Add(Phone);
                cmd.Parameters.Add(Email);
                cmd.Parameters.Add(Updated);
                try
                {
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    result = 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
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

            return result;
        }

        public int Remove(int id)
        {
            int result = -1;
            if (TryOpenDB())
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "DELETE FROM [AddressBookDB].[dbo].[People] " +
                                  "WHERE ID = @ID",
                    CommandType = CommandType.Text,
                    Connection = _connection
                };

                SqlParameter para1 = new SqlParameter()
                {
                    ParameterName = "@ID",
                    Value = id,
                    DbType = DbType.Int32,
                };
                cmd.Parameters.Add(para1);
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
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

            return result;
        }

        private bool TryOpenDB()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    using (StreamReader sr = new StreamReader(_setupFilePath))
                    {
                        _connectionString = @sr.ReadToEnd();
                    }
                }
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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

        private string _firstToUpper(string text)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(text[0].ToString().ToUpper());
            sb.Append(text.Remove(0, 1));

            return sb.ToString();
        }
    }
}