using System;
using SimplePhoneBook.Core;

namespace SimplePhoneBook
{
    class Program
    {
        static void Main(String[] args)
        {
           UserInterface simplePhoneBook = new UserInterface();
           simplePhoneBook.Start();
        }
    }
}