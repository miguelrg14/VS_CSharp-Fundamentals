using System.Globalization;
using ConsoleUtils;

namespace Fundamentals.Exercises.MiniApps;

/// <summary>A struct is enough here: a contact is a small group of values with no behaviour of its own.</summary>
public readonly record struct Contact(string FirstName, string LastName, int Age, string Phone)
{
    public override string ToString() => $"{FirstName} {LastName}, {Age} years old, phone {Phone}";
}

public sealed class AddressBook : IExercise
{
    public const string FileName = "contacts.tsv";
    private const char Separator = '\t';

    private static readonly string[] Options = ["Add contact", "List contacts", "Delete contact", "Export to file", "Import from file"];

    public string Title => "Address book";

    public string Description => $"Add, list and delete contacts, and save them to / load them from {FileName}.";

    public void Run()
    {
        var contacts = new List<Contact>();
        while (ConsoleMenu.Show("Address book", Options) is int option)
        {
            switch (option)
            {
                case 0:
                    contacts.Add(ReadContact());
                    Console.WriteLine("Contact added.");
                    break;
                case 1:
                    PrintContacts(contacts);
                    break;
                case 2:
                    Delete(contacts);
                    break;
                case 3:
                    File.WriteAllLines(FileName, contacts.Select(Serialize));
                    Console.WriteLine($"{contacts.Count} contact(s) exported to {Path.GetFullPath(FileName)}.");
                    break;
                case 4:
                    Import(contacts);
                    break;
            }
        }
    }

    public static string Serialize(Contact contact) =>
        string.Join(Separator, contact.FirstName, contact.LastName, contact.Age.ToString(CultureInfo.InvariantCulture), contact.Phone);

    public static bool TryDeserialize(string line, out Contact contact)
    {
        string[] fields = line.Split(Separator);
        if (fields.Length == 4 && int.TryParse(fields[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int age))
        {
            contact = new Contact(fields[0], fields[1], age, fields[3]);
            return true;
        }

        contact = default;
        return false;
    }

    private static Contact ReadContact() => new(
        ReadField("First name:"),
        ReadField("Last name:"),
        ConsoleInput.ReadInt("Age:", 0, 150),
        ReadField("Phone:")); // A phone is text: it can start with 0 or contain '+'.

    /// <summary>Tabs are not allowed because they separate the fields in the file.</summary>
    private static string ReadField(string prompt)
    {
        while (true)
        {
            string value = ConsoleInput.ReadString(prompt);
            if (!value.Contains(Separator))
            {
                return value;
            }

            Console.WriteLine("Tabs are not allowed.");
        }
    }

    private static void PrintContacts(IReadOnlyList<Contact> contacts)
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("The address book is empty.");
            return;
        }

        for (int i = 0; i < contacts.Count; i++)
        {
            Console.WriteLine($"{i + 1,3}. {contacts[i]}");
        }
    }

    private static void Delete(List<Contact> contacts)
    {
        PrintContacts(contacts);
        if (contacts.Count == 0)
        {
            return;
        }

        int number = ConsoleInput.ReadInt("Number of the contact to delete (0 to cancel):", 0, contacts.Count);
        if (number > 0)
        {
            Console.WriteLine($"Deleted: {contacts[number - 1]}");
            contacts.RemoveAt(number - 1); // List<T> closes the gap; no manual shifting needed.
        }
    }

    private static void Import(List<Contact> contacts)
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine($"{FileName} does not exist. Export some contacts first.");
            return;
        }

        if (contacts.Count > 0 && !ConsoleInput.ReadYesNo($"Importing replaces the {contacts.Count} contact(s) in memory. Continue?"))
        {
            return;
        }

        int skipped = 0;
        contacts.Clear();
        foreach (string line in File.ReadLines(FileName))
        {
            if (TryDeserialize(line, out Contact contact))
            {
                contacts.Add(contact);
            }
            else
            {
                skipped++;
            }
        }

        Console.WriteLine($"{contacts.Count} contact(s) imported{(skipped > 0 ? $", {skipped} invalid line(s) skipped" : "")}.");
    }
}
