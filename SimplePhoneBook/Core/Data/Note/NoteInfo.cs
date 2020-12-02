using System;

namespace SimplePhoneBook.Core.Data.Note
{
    public class NoteInfo
    {
        public Int64 PhoneNumber { get; set; }
        public String Country { get; set; }
        public DateTime BirthdayDate { get; set; }
        public String Organization { get; set; }
        public String Job { get; set; }
        public String Notes { get; set; }

        public NoteInfo(Int64 phoneNumber, String country, DateTime birthdayDate, String organization, String job, String notes)
        {
            PhoneNumber = phoneNumber;
            Country = country;
            BirthdayDate = birthdayDate;
            Organization = organization;
            Job = job;
            Notes = notes;
        }

        public override String ToString()
        {
            String result = $"\tphone: {PhoneNumber}\n" +
                            $"\tcountry: {Country}\n";
            if (BirthdayDate != DateTime.MinValue)
            {
                result += $"\tbirthday: {BirthdayDate:dd.MM.yyyy}\n";
            }
            if (Organization != string.Empty)
            {
                result += $"\torganization: {Organization}\n";
            }
            if (Job != string.Empty)
            {
                result += $"\tjob: {Job}\n";
            }
            if (Notes != string.Empty)
            {
                result += $"\tsome notes: {Notes}\n";
            }

            return result;
        }
    }
}