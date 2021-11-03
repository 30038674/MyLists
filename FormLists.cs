using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// WARNING Limited error trapping!
// This code could be used in the Project
namespace MyLists
{
    public partial class FormLists : Form
    {
        public FormLists()
        {
            InitializeComponent();
        }
        List<string> ColourList = new List<string>() {"Yellow", "Red", "Green", "Blue", "Orange", "Amber", "Canary" };

        #region AddEditDel
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxInput.Text) && (ValidName(textBoxInput.Text)))
            {
                ColourList.Add(textBoxInput.Text);
                DisplayList();
                textBoxInput.Clear();
                // focus
            }
            else
            {
                MessageBox.Show("A Bad thing happened");
            }
        }
        private bool ValidName(string checkThisName)
        {
            if (ColourList.Exists(duplicate => duplicate.Equals(checkThisName)))
                return false;
            else
                return true;
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            ColourList[listBoxDisplay.SelectedIndex] = textBoxInput.Text;
            textBoxInput.Clear();
            DisplayList();
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            listBoxDisplay.SetSelected(listBoxDisplay.SelectedIndex, true);
            ColourList.RemoveAt(listBoxDisplay.SelectedIndex);
            DisplayList();
        }
        #endregion AddEditDel

        #region FileIO
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            string fileName = "Rainbow.bin";
            OpenFileDialog OpenBinary = new OpenFileDialog();
            DialogResult sr = OpenBinary.ShowDialog();
            if (sr == DialogResult.OK)
            {
                fileName = OpenBinary.FileName;
            }
            try
            {
                ColourList.Clear();
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    while (stream.Position < stream.Length)
                    {
                        ColourList.Add((string)binaryFormatter.Deserialize(stream));
                    }
                }
                DisplayList();
            }
            catch (IOException)
            {
                MessageBox.Show("cannot open file");
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            string fileName = "Rainbow.bin";
            SaveFileDialog saveBinary = new SaveFileDialog();
            DialogResult sr = saveBinary.ShowDialog();
            if(sr == DialogResult.Cancel)
            {
                saveBinary.FileName = fileName;
            }
            if(sr == DialogResult.OK)
            {
                fileName = saveBinary.FileName;
            }
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    foreach (var item in ColourList)
                    {
                        binFormatter.Serialize(stream, item);
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("cannot save file");
            }
        }
        #endregion FileIO

        #region Utility
        private void DisplayList()
        {
            listBoxDisplay.Items.Clear();
            ColourList.Sort();
            foreach (var color in ColourList)
            {
                listBoxDisplay.Items.Add(color);
            }
        }
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            ColourList.Sort();
            if (ColourList.BinarySearch(textBoxInput.Text) >= 0)
                MessageBox.Show("found");
            else
                MessageBox.Show("Not Found");
            textBoxInput.Clear();
        }
        private void FormLists_Load(object sender, EventArgs e)
        {
            DisplayList();
        }
        private void TextBoxInput_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxInput.Clear();
        }
        private void ListBoxDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            listBoxDisplay.SetSelected(listBoxDisplay.SelectedIndex, true);
            textBoxInput.Text = ColourList.ElementAt(listBoxDisplay.SelectedIndex);
        }
        #endregion Utility
    }
}