using System;
using System.Collections.Generic;
using System.Globalization;
using SimplePhoneBook.Core.Data;
using SimplePhoneBook.Core.Data.Note;
using Type = SimplePhoneBook.Core.Data.Type;

namespace SimplePhoneBook.Core
{
    public class UserInterface
    {
        private readonly Dictionary<Type, String> _parameters = new Dictionary<Type, String>()
        {
            {Type.Surname, "surname"},
            {Type.Name, "name"},
            {Type.MiddleName, "middle name*"},
            {Type.PhoneNumber, "phone number"},
            {Type.Country, "country"},
            {Type.BirthdayDate, "birthday date*"},
            {Type.Organization, "organization*"},
            {Type.Job, "job*"},
            {Type.Notes, "some notes*"}
        };

        private String GetTextByParameter(String parameter)
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

        private List<String> GetListInputs()
        {
            var info = new List<String>();
            Console.WriteLine("\tParameter with * is optional, to skip press enter");
            Console.WriteLine("\tEnter the following parameters: ");
            foreach (var parameter in _parameters)
            {
                Console.Write($"\t\tAdd {parameter.Value}: ");
                String text = GetTextByParameter(parameter.Value);
                info.Add(text);
            }

            return info;
        }

        private void PrintError(String errorMessage)
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

        private Type ChooseType()
        {
            Console.WriteLine();
            Console.WriteLine(
                "\tsurname - 1\n" +
                "\tname - 2\n" +
                "\tmiddle name* - 3\n" +
                "\tphone number - 4\n" +
                "\tcountry - 5\n" +
                "\tbirthday date* - 6\n" +
                "\torganization* - 7\n" +
                "\tjob* - 8\n" +
                "\tsome notes* - 9");
            Console.Write("\tChoose the parameter" +
                          "\n\tyou want to change: ");
            if (!Enum.TryParse<Type>(Console.ReadLine(), out var type))
            {
                throw new ArgumentOutOfRangeException();
            }

            return type;
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
            Commands command = Commands.CreateNote;
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
                            var name = GetFullName();
                            var type = ChooseType();
                            Console.Write("\tAdd text: ");
                            var text = GetTextByParameter(_parameters[type]);
                            notebook.EditNote(name, type, text);
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