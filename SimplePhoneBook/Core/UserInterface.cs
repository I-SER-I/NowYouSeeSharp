using System;
using System.Collections.Generic;
using System.Globalization;
using SimplePhoneBook.Core.Data;
using SimplePhoneBook.Core.Data.Note;

namespace SimplePhoneBook.Core
{
    public class UserInterface
    {
        private readonly List<string> _parameters = new List<string>()
        {
            "surname",
            "name",
            "middle name*",
            "phone number",
            "country",
            "birthday date*",
            "organization*",
            "job*",
            "some notes*"
        };

        private string GetTextByParameter(string parameter)
        {
            var text = string.Empty;
            switch (parameter)
            {
                case "middle name*":
                case "organization*":
                case "job*":
                case "some notes*":
                    {
                        text = Console.ReadLine();
                        break;
                    }
                case "phone number":
                    {
                        while (true)
                        {
                            try
                            {
                                if (!long.TryParse(Console.ReadLine(), out var number))
                                    throw new ArgumentException();
                                text = number.ToString();
                                break;
                            }
                            catch (ArgumentException)
                            {
                                PrintError("Incorrect text");
                                Console.Write($"\t\tAdd {parameter}: ");
                            }
                        }

                        break;
                    }
                case "surname":
                case "name":
                case "country":
                    {
                        while (true)
                        {
                            text = Console.ReadLine();
                            if (text != string.Empty)
                                break;
                            PrintError("Empty text");
                            Console.Write($"\t\tAdd {parameter}: ");
                        }

                        break;
                    }
                case "birthday date*":
                    {
                        while (true)
                        {
                            text = Console.ReadLine();
                            try
                            {
                                if (text == string.Empty)
                                {
                                    text = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                }

                                if (!DateTime.TryParse(text, out var date))
                                {
                                    throw new ArgumentException();
                                }

                                text = date.ToString(CultureInfo.InvariantCulture);
                                break;
                            }
                            catch (ArgumentException)
                            {
                                PrintError("Incorrect text");
                                Console.Write($"\t\tAdd {parameter}: ");
                            }
                        }

                        break;
                    }
            }

            return text;
        }

        private List<string> GetListInputs()
        {
            var info = new List<string>();
            Console.WriteLine("\tParameter with * is optional, to skip press enter");
            Console.WriteLine("\tEnter the following parameters: ");
            foreach (var parameter in _parameters)
            {
                Console.Write($"\t\tAdd {parameter}: ");
                string text = GetTextByParameter(parameter);
                info.Add(text);
            }

            return info;
        }

        private void PrintError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t\t  {errorMessage}. Try again");
            Console.ResetColor();
        }

        private NoteName GetFullName()
        {
            Console.Write("\tPress full name: ");
            var text = Console.ReadLine();
            if (text == null)
                throw new ArgumentException();
            var names = text.Split(' ');
            return names.Length != 2 && names.Length != 3
                ? throw new ArgumentException()
                : names.Length == 2
                    ? new NoteName(names[1], names[0])
                    : new NoteName(names[1], names[0], names[2]);
        }

        private enum Commands
        {
            CreateNote = 1,
            EditNote,
            DeleteNote,
            ReadNote,
            ShowAllNotes,
            Exit,
        }
        public void Start()
        {
            Console.WriteLine("\t\tWelcome to a simple notebook!");
            Notebook notebook = new Notebook();
            Commands? command = null;
            while (command != Commands.Exit)
            {
                Console.WriteLine();
                Console.WriteLine("1. Create note");
                Console.WriteLine("2. Edit note");
                Console.WriteLine("3. Delete note");
                Console.WriteLine("4. Read note");
                Console.WriteLine("5. Show all notes");
                Console.WriteLine("6. Exit\n");
                try
                {
                    Console.Write("Press number: ");
                    if (!Enum.TryParse<Commands>(Console.ReadLine(), out command))
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    switch (command)
                    {
                        case Commands.CreateNote:
                            var info = GetListInputs();
                            var noteName = info[2] == string.Empty
                                ? new NoteName(
                                    info[1],
                                    info[0])
                                : new NoteName(
                                    info[1],
                                    info[0],
                                    info[2]);
                            var noteInfo = new NoteInfo(
                                long.Parse(info[3]),
                                info[4],
                                DateTime.Parse(info[5]),
                                info[6],
                                info[7],
                                info[8]);
                            notebook.CreateNote(noteName, noteInfo);
                            break;
                        case Commands.EditNote:
                            //notebook.ReadNote(GetFullName(), , text);
                            break;
                        case Commands.DeleteNote:
                            notebook.RemoveNote(GetFullName());
                            break;
                        case Commands.ReadNote:
                            notebook.ReadNote(GetFullName());
                            break;
                        case Commands.ShowAllNotes:
                            notebook.ShowAllNotes();
                            break;
                        case Commands.Exit:
                            Console.WriteLine("\t\tHave a nice day!");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    PrintError("Command not found");
                }
                catch (ArgumentException)
                {
                    PrintError("Incorrect full name");
                }
                catch (KeyNotFoundException)
                {
                    PrintError("Note exist");
                }
            }
        }
    }
}