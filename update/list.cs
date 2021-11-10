using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace MyLists
{
    public partial class FormLists : Form
    {
        public FormLists()
        {
            InitializeComponent();
        }
        List<string> ColourList = new List<string>();
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
            if (sr == DialogResult.Cancel)
            {
                saveBinary.FileName = fileName;
            }
            if (sr == DialogResult.OK)
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
            string textFileName;
            OpenFileDialog openTextFileDialog = new OpenFileDialog();
            openTextFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            openTextFileDialog.Filter = "txt flies (*.txt)|*.txt";
            DialogResult sr = openTextFileDialog.ShowDialog();
            if (sr == DialogResult.OK)
            {
                textFileName = openTextFileDialog.FileName;
                currentFileName = textFileName;
            }
            else
            {
                return;
            }
            try
            {
                ColourList.Clear();
                using (StreamReader reader = new StreamReader(File.OpenRead(textFileName)))
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
                MessageBox.Show("Open Text File Error");
            }
        }
        private void buttonSaveText_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveTextFileDialog = new SaveFileDialog();
            saveTextFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            DialogResult sr = saveTextFileDialog.ShowDialog();
            if (sr == DialogResult.OK)
            {
                SaveTextFile(saveTextFileDialog.FileName);
            }
            if (sr == DialogResult.Cancel)
            {
                return;
            }
        }
        private void SaveTextFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false))
                {
                    foreach (var colour in ColourList)
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
        private void FormLists_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //currentFileName = Path.GetFileNameWithoutExtension(currentFileName);
                //string twoDigits = currentFileName.Remove(0, 5);
                //int num = int.Parse(twoDigits);
                int num = int.Parse(Path.GetFileNameWithoutExtension(currentFileName).Remove(0, 5));
                num++;
                string newValue;
                if (num <= 9)
                    newValue = "0" + num.ToString();
                else
                    newValue = num.ToString();
                string newfilename = "demo_" + newValue + ".txt";
                SaveTextFile(newfilename);
            }
            catch
            {
                return;
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
        private void TextBoxInput_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxInput.Clear();
        }
        private void ListBoxDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                listBoxDisplay.SetSelected(listBoxDisplay.SelectedIndex, true);
                textBoxInput.Text = ColourList.ElementAt(listBoxDisplay.SelectedIndex);
            }
            catch
            {
                return;
            }
        }
        #endregion Utility
    }
}
