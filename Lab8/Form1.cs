using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab8
{
    [Serializable]
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Branch> Database = new List<Branch>();

        #region File 
        //открывает файл
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    Database = (List<Branch>)bf.Deserialize(fs);
                    DatabaseUpdate();
                    saveToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Tag = openFileDialog1.FileName;
                }
            }
        }

        //сохраняет текущий файл. Имя текущего файла записано в saveToolStripMenuItem.Tag
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(saveToolStripMenuItem.Tag as string, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, Database);
            }
        }

        //сохраняет файл как новый
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, Database);
                }
            }
        }
        #endregion

        //обновляет dataGridView1
        private void DatabaseUpdate()
        {
            dataGridView1.Columns.Clear();
            for (int i = 0; i < Database.Count; i++)
                dataGridView1.Columns.Add($"column{i}", Database[i].Name);
            dataGridView1.Rows.Add(GetRowCount());
            Date currentDate = new Date(GetFirstDate());
            for (int i = 0; i < GetRowCount(); i++)
            {
                for (int j = 0; j < Database.Count; j++)
                    if (Database[j].Income.Keys != null)
                        if (Database[j].Income.ContainsKey(currentDate))
                            dataGridView1.Rows[i].Cells[j].Value = Database[j].Income[currentDate];
                dataGridView1.Rows[i].HeaderCell.Value = currentDate.ToString();
                currentDate++;
            }
        }

        #region Mini-methods
        private Date GetFirstDate()
        {
            Date date = new Date(100000, 1);
            foreach (var a in Database)
                foreach (var b in a.Income)
                    if (date.CompareTo(b.Key) == 1) date = b.Key;
            return date;
        }

        private Date GetLastDate()
        {
            Date date = new Date(0, 1);
            foreach (var a in Database)
                foreach (var b in a.Income)
                    if (date.CompareTo(b.Key) == -1) date = b.Key;
            return date;
        }

        private int GetRowCount()
        {
            int rowCount = 0;
            Date firstDate = new Date(GetFirstDate());
            while (firstDate.CompareTo(GetLastDate()) == -1 || firstDate.CompareTo(GetLastDate()) == 0)
                foreach (var c in Database)
                    if (c.Income.ContainsKey(firstDate))
                    {
                        firstDate++;
                        rowCount++;
                    }
            return rowCount;
        }
        #endregion

        #region Edit
        private void atTheTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Date date = new Date(GetFirstDate());
            date--;
            foreach (var c in Database)
                c.Income.Add(date, null);
            DatabaseUpdate();
        }

        private void atTheBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Date date = new Date(GetLastDate());
            date++;
            foreach (var c in Database)
                c.Income.Add(date, null);
            DatabaseUpdate();
        }

        //добавляет варианты выбора в comboBox2
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.AddRange((from o1 in Database
                                      where o1.Name == (string)comboBox1.SelectedItem
                                      select o1.Income.Keys into o2
                                      from o3 in o2
                                      select o3.ToString()).ToArray());
        }

        private void editCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int value;
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
                for (int i = 0; i < Database.Count; i++)
                    if (Database[i].Name == (string)comboBox1.SelectedItem)
                        for (int j = 0; j < GetRowCount(); j++)
                            if (Database[i].Income.Keys.ToArray()[j].ToString() == (string)comboBox2.SelectedItem)
                            {
                                if (textBox1.Text == "")
                                    Database[i][j] = null;
                                else if (int.TryParse(textBox1.Text, out value))
                                    Database[i][j] = value;
                                else
                                    MessageBox.Show("Enter proper value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                DatabaseUpdate();
                            }
        }
        #endregion
    }
}
