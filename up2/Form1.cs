using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace up2
{
    public partial class Form1 : Form
    {
        private PhoneBook phoneBook;
        private string fileName = "contacts.csv";

        public Form1()
        {
            InitializeComponent();
            phoneBook = new PhoneBook();
            PhoneBookLoader.Load(phoneBook, fileName);
        }

        // Метод для выхода из приложения
        private void ExitFromBook(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Метод для отображения всех контактов в DataGridView
        private void ViewAllContacts(object sender, EventArgs e)
        {
            List<Contact> contacts = phoneBook.GetAllContacts();
            for (int i = 0; i < contacts.Count; i++)
            {
                dataGridView1.RowCount++;
                dataGridView1[0, i].Value = contacts[i].Name;
                dataGridView1[1, i].Value = contacts[i].Phone;
            }
        }

        // Метод для добавления нового контакта
        private void AddContact(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                // Проверка имени и фамилии
                if (NamesProverka(textBox1.Text))
                {
                    // Проверка номера телефона
                    if (NumberProverka(textBox2.Text))
                    {
                        try
                        {
                            // Добавление контакта в телефонную книгу
                            phoneBook.AddContact(new Contact { Name = textBox1.Text, Phone = textBox2.Text });
                            dataGridView1.RowCount++;
                            dataGridView1[0, dataGridView1.RowCount - 2].Value = textBox1.Text;
                            dataGridView1[1, dataGridView1.RowCount - 2].Value = textBox2.Text;
                            MessageBox.Show("Контакт добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        MessageBox.Show("Введен неправильный номер\nНужно строго по примеру: '(999)999-99-99'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Введено неправильное имя или фамилия\nПример: 'Иван Иванов'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Для начала ведите имя, фамилию и номер телефона", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Метод для редактирования выбранного контакта
        private void EditContact(object sender, EventArgs e)
        {
            // Проверка на выбранную строку в DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string[] words = dataGridView1.SelectedCells[0].Value.ToString().Split(' ');
                // Проверка  на пустые поля
                if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    // Проверка имени и фамилии
                    if (NamesProverka(textBox1.Text))
                    {
                        // Проверка номера телефона
                        if (NumberProverka(textBox2.Text))
                        {
                            try
                            {
                                // Поиск контакта по имени и фамилии
                                List<Contact> contacts = phoneBook.SearchContacts(words[0] + " " + words[1]);
                                // Редактирование контакта
                                phoneBook.EditContact(contacts[0], new Contact { Name = textBox1.Text, Phone = textBox2.Text });
                                dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value = textBox1.Text;
                                dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value = textBox2.Text;
                                MessageBox.Show("Контакт изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                            MessageBox.Show("Введен неправильный номер\nНужно строго по примеру: '(999)999-99-99'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Введено неправильное имя или фамилия\nПример: 'Иван Иванов'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Для начала ведите имя, фамилию и номер телефона", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Для начала выберите контакт (строку)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Метод для удаления выбранного контакта
        private void DeleteContact(object sender, EventArgs e)
        {
            // Проверка на выбранную строку в DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string[] words = dataGridView1.SelectedCells[0].Value.ToString().Split(' ');
                // Поиск контакта по имени и фамилии
                List<Contact> contacts = phoneBook.SearchContacts(words[0] + " " + words[1]);
                // Удаление контакта
                phoneBook.RemoveContact(contacts[0]);
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                MessageBox.Show("Контакт удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Для начала выберите контакт (строку)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Метод для поиска контакта по имени и фамилии
        private void SearchContact(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                // Проверка имени и фамилии
                if (NamesProverka(textBox1.Text))
                {
                    // Поиск контакта
                    List<Contact> contacts = phoneBook.SearchContacts(textBox1.Text);
                    if (contacts.Count == 1)
                    {
                        dataGridView1.ClearSelection();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["NameSurname"].Value.ToString().Contains(textBox1.Text))
                            {
                                row.Selected = true;
                                MessageBox.Show("Контакт найден!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                        }
                    }
                    else
                        MessageBox.Show("Такого контакта нет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Введено неправильное имя или фамилия\nПример: 'Иван Иванов'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Для начала ведите имя и фамилию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Метод для сохранения контактов в файл
        private void SaveContacts(object sender, EventArgs e)
        {
            PhoneBookLoader.Save(phoneBook, fileName);
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Метод для проверки имени и фамилии
        private bool NamesProverka(string name)
        {
            string[] words = name.Split(' ');
            if (words.Length != 2)
                return false;
            foreach (string word in words)
            {
                char[] chars = word.ToCharArray();
                if (!char.IsUpper(chars[0]) || chars.Length < 2)
                    return false;
                chars = word.Remove(0, 1).ToCharArray();
                foreach (char c in chars)
                {
                    if (!char.IsLetter(c) || char.IsUpper(c))
                        return false;
                }
            }
            return true;
        }

        // Метод для проверки номера телефона
        private bool NumberProverka(string number)
        {
            char[] chars = number.ToCharArray();
            int sc1 = 0, sc2 = 0, tr = 0, cf = 0;
            if (chars.Length != 14)
                return false;
            foreach (char c in chars)
            {
                if (c == '(')
                    sc1++;
                if (c == ')')
                    sc2++;
                if (c == '-')
                    tr++;
                if (char.IsNumber(c))
                    cf++;
            }
            if (sc1 == 1 && sc2 == 1 && tr == 2 && cf == 10)
                return true;
            else
                return false;
        }
    }
}
