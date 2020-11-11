using System;

namespace SimplePhoneBook.Core.Data.Note
{
    public class NoteInfo
    {
        public long PhoneNumber { get; set; }
        public string Country { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Organization { get; set; }
        public string Job { get; set; }
        public string Notes { get; set; }

        public NoteInfo(long phoneNumber, string country, DateTime birthdayDate, string organization, string job, string notes)
        {
            PhoneNumber = phoneNumber;
            Country = country;
            BirthdayDate = birthdayDate;
            Organization = organization;
            Job = job;
            Notes = notes;
        }

        public override string ToString()
        {
            string result = $"\tphone: {PhoneNumber}\n" +
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