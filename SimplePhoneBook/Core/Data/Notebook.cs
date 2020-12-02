using System;
using System.Collections.Generic;
using SimplePhoneBook.Core.Data.Note;

namespace SimplePhoneBook.Core.Data
{
    public class Notebook
    {
        private readonly Dictionary<String, NoteInfo> _note = new Dictionary<String, NoteInfo>();

        public void CreateNote(NoteName noteName, NoteInfo info) => _note.Add(noteName.ToString(), info);

        public void RemoveNote(NoteName noteName)
        {
            if (_note.ContainsKey(noteName.ToString()))
            {
                _note.Remove(noteName.ToString());
                Console.WriteLine("\tDone");
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void EditNote(NoteName noteName, Type type, String text)
        {
            var name = noteName.ToString();
            NoteInfo noteInfo;
            if (_note.ContainsKey(name))
            {
                noteInfo = _note[name];
            }
            else
            {
                throw new KeyNotFoundException();
            }
            switch (type)
            {
                case Type.Name:
                    noteName.SetName(text);
                    break;
                case Type.Surname:
                    noteName.SetSurname(text);
                    break;
                case Type.MiddleName:
                    noteName.SetMiddleName(text);
                    break;
                case Type.PhoneNumber:
                    noteInfo.PhoneNumber = int.Parse(text);
                    break;
                case Type.Country:
                    noteInfo.Country = text;
                    break;
                case Type.BirthdayDate:
                    noteInfo.BirthdayDate = DateTime.Parse(text);
                    break;
                case Type.Organization:
                    noteInfo.Organization = text;
                    break;
                case Type.Job:
                    noteInfo.Job = text;
                    break;
                case Type.Notes:
                    noteInfo.Notes = text;
                    break;
                default:
                    return;
            }
        }

        public void ReadNote(NoteName noteName)
        {
            if (_note.ContainsKey(noteName.ToString()))
            {
                Console.WriteLine(noteName);
                Console.WriteLine(_note[noteName.ToString()]);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void ShowAllNotes()
        {
            if (_note.Count == 0)
            {
                Console.WriteLine("\tNotebook is empty");
            }
            foreach (var note in _note)
            {
                Console.WriteLine(note.Key);
                Console.WriteLine(note.Value);
            }
        }
    }
}