using SimplePhoneBook.Core;

namespace SimplePhoneBook
{
    class Program
    {
        static void Main(string[] args)
        {
           UserInterface simplePhoneBook = new UserInterface();
           simplePhoneBook.Start();
        }
    }
}