﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _1_AdressBook.Models
{
    public class PersonModel
    {
        public PersonModel()
        {

        }
        public PersonModel(int iD, string firstName, string lastName, int phone, string email, DateTime created, DateTime? updated)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Created = created;
            Updated = updated;
        }

        [Required(ErrorMessage = "To Pole jest wymagane"), DisplayName("Identyfikator")]
        public int ID { get; set; }

        [Required(ErrorMessage = "To Pole jest wymagane"), MinLength(2, ErrorMessage = "Minimalna długość to 2 znaki"), MaxLength(20, ErrorMessage = "Maksymalna długość to 2 znaki"), DisplayName("Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "To Pole jest wymagane"), MinLength(2,ErrorMessage = "Minimalna długość to 2 znaki"), MaxLength(20, ErrorMessage = "Maksymalna długość to 2 znaki"), DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "To Pole jest wymagane"), Range(9,9,ErrorMessage = "Wymagana długość to 9 cyfr"), DisplayName("Telefon")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "To Pole jest wymagane"), EmailAddress(ErrorMessage = "Bład formatu E-mail"),StringLength(50,ErrorMessage = "Zbyt długi adres E-mail"), DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To Pole jest wymagane"), DataType(DataType.Date, ErrorMessage = "Bład formatu Daty"), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}"), DisplayName("Data Utworzenia")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Bład formatu Daty"), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}"), DisplayName("Data Modyfikacji")]
        public DateTime? Updated { get; set; }
    }
}