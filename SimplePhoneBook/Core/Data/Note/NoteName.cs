namespace SimplePhoneBook.Core.Data.Note
{
    public class NoteName
    {
        private string _name;
        private string _surname;
        private string _middleName;

        public NoteName(string name, string surname, string middleName)
        {
            _name = name;
            _surname = surname;
            _middleName = middleName;
        }

        public NoteName(string name, string surname)
        {
            _name = name;
            _surname = surname;
            _middleName = string.Empty;
        }

        public void SetName(string name) =>
            _name = name;

        public void SetSurname(string surname) =>
            _surname = surname;

        public void SetMiddleName(string middleName) =>
            _middleName = middleName;

        public override string ToString() =>
            $"\t{_surname} {_name} {_middleName}";
    }
}