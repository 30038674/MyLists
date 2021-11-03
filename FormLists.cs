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
        string currentFileName = "";

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

        #region BinaryFileIO
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
        #endregion BinaryFileIO

        #region TextFileIO
        private void buttonOpenText_Click(object sender, EventArgs e)
        {
            string fileName = "demo_01.txt";
            OpenFileDialog OpenText = new OpenFileDialog();
            DialogResult sr = OpenText.ShowDialog();
            if (sr == DialogResult.OK)
            {
                fileName = OpenText.FileName;
            }
            currentFileName = fileName;
            try
            {
                ColourList.Clear();
                using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
                {
                    while (!reader.EndOfStream)
                    {                     
                        ColourList.Add(reader.ReadLine());
                    }
                }
                DisplayList();
            }
            catch (IOException)
            {
                MessageBox.Show("file not openned");
            }
        }
        private void buttonSaveText_Click(object sender, EventArgs e)
        {
            string fileName = "demo_01.txt";
            SaveFileDialog SaveText = new SaveFileDialog();
            DialogResult sr = SaveText.ShowDialog();
            if (sr == DialogResult.OK)
            {
                fileName = SaveText.FileName;
            }
            if (sr == DialogResult.Cancel)
            {
                SaveText.FileName = fileName;
            }
            // Validate file name and increment
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false))
                {
                    foreach(var colour in ColourList)
                    {
                        writer.WriteLine(colour);
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File NOT saved");
            }
        }

        #endregion TextFileIO

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