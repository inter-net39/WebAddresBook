using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using _1_AdressBook.Models;

namespace _1_AdressBook
{
    /*
       Klasa SourceManager będziemy używać do
       zapisywania, aktualizowania i pobierania danych
       z bazy.     */
    public class SourceManager
    {
        /*
           metoda powinna zwracać listę
           obiektów z bazy zaczynającą się od podanego
           numeru wiersza. Długość listy określa drugi
           parametr.
           */
        private string _connectionString = "";
        private SqlConnection _connection;
        public List<PersonModel> Get(int start, int take)
        {
            return null;
        }

        public PersonModel GetByID(int id)
        {
            return null;
        }

        public int Add(PersonModel personModel)
        {

        }

        public void Update(PersonModel personModel)
        {

        }

        public void Remove(int id)
        {

        }

        private bool OpenDB()
        {
            if (_connectionString == "")
            {
                return false;
            }

            if (_connection == null)
            {
                //TODO: ROLLBACK
                _connection = new SqlConnection()
                {
                    ConnectionString = _connectionString,
                };
                _connection.Open();
               // OnCloseAction?.Invoke("Połączono z bazą danych");
            }
            else
            {
               // OnCloseActionERR?.Invoke("AlreadyOpenedDBConnectionExeption: Jesteś już połączony z bazą danych.");
                throw new AlreadyOpenedDBConnectionExeption("Jesteś już połączony z bazą danych.");
            }
        }

    }
}