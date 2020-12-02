using System;

namespace SimplePhoneBook.Core.Data.Note
{
    public class NoteName
    {
        private String _name;
        private String _surname;
        private String _middleName;

        public NoteName(String name, String surname, String middleName)
        {
            _name = name;
            _surname = surname;
            _middleName = middleName;
        }

        public NoteName(String name, String surname)
        {
            _name = name;
            _surname = surname;
            _middleName = string.Empty;
        }

        public void SetName(String name) =>
            _name = name;

        public void SetSurname(String surname) =>
            _surname = surname;

        public void SetMiddleName(String middleName) =>
            _middleName = middleName;

        public override String ToString() =>
            $"\t{_surname} {_name} {_middleName}";
    }
}