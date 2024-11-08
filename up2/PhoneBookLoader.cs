using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace up2
{
    public static class PhoneBookLoader
    {
        // Метод для загрузки контактов из файла в телефонную книгу
        public static void Load(PhoneBook phoneBook, string fileName)
        {
            // Проверка существования файла
            if (!File.Exists(fileName))
            {
                return;
            }

            // Очистка текущего списка контактов
            phoneBook.GetAllContacts().Clear();

            // Чтение файла и добавление контактов в телефонную книгу
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Разделение строки на части по символам ';' и ','
                    string[] parts = line.Split(';', ',');
                    if (parts.Length == 2)
                    {
                        // Добавление контакта в телефонную книгу
                        phoneBook.AddContact(new Contact { Name = parts[0], Phone = parts[1] });
                    }
                }
            }
        }

        // Метод для сохранения контактов из телефонной книги в файл
        public static void Save(PhoneBook phoneBook, string fileName)
        {
            // Запись контактов в файл
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var contact in phoneBook.GetAllContacts())
                {
                    // Запись контакта в файл в формате "Имя,Телефон"
                    writer.WriteLine($"{contact.Name},{contact.Phone}");
                }
            }
        }
    }
}
