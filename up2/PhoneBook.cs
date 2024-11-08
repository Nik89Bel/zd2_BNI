using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace up2
{
    public class PhoneBook
    {
        private List<Contact> contacts = new List<Contact>();

        // Метод для добавления нового контакта
        public void AddContact(Contact contact)
        {
            // Проверка на наличие контакта с таким же номером телефона
            if (FindByPhone(contact.Phone) != null)
                throw new Exception("Контакт с таким номером уже есть");
            // Добавление контакта в список
            contacts.Add(contact);
        }

        // Метод для удаления контакта
        public void RemoveContact(Contact contact)
        {
            // Удаление контакта из списка
            contacts.Remove(contact);
        }

        // Метод для редактирования контакта
        public void EditContact(Contact oldContact, Contact newContact)
        {
            // Поиск индекса контакта, который нужно изменить
            int index = contacts.IndexOf(oldContact);
            if (index != -1)
            {
                // Замена старого контакта на новый
                contacts[index] = newContact;
            }
        }

        // Метод для получения всех контактов
        public List<Contact> GetAllContacts()
        {
            // Возвращает список всех контактов
            return contacts;
        }

        // Метод для поиска контактов по имени
        public List<Contact> SearchContacts(string name)
        {
            // Возвращает список контактов, имена которых содержат заданную строку
            return contacts.Where(c => c.Name.Contains(name)).ToList();
        }

        // Метод для поиска контакта по номеру телефона
        public Contact FindByPhone(string phone)
        {
            // Поиск контакта с заданным номером телефона
            foreach (var contact in contacts)
            {
                if (contact.Phone == phone)
                    return contact;
            }
            // Если контакт не найден, возвращает null
            return null;
        }
    }
}
