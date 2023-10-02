using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SIAR
{
    public enum Screen { Vertical, Horizontal }

    public partial class Form1 : Form
    {
        private string _verticalPath;
        private string _horizontalPath;

        List<string> _imageFiles;


        public Form1()
        {
            InitializeComponent();
            _imageFiles = new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text =
                    "From folders: " + folderBrowserDialog1.SelectedPath + " files will be transferred:" + Environment.NewLine +
                    "1) Images with a vertical file orientation will be moved to: " + _verticalPath + Environment.NewLine +
                    "2) Images with a horizontal file orientation will be moved to: " + _horizontalPath + Environment.NewLine;

                button1.BackColor = Color.Blue;

                _imageFiles.AddRange(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg"));
                _imageFiles.AddRange(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.png"));
                _imageFiles.AddRange(Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpeg"));
            }
            folderBrowserDialog1.Dispose();

            
            List<(FileInfo, Screen)> values = new();

            foreach (string file in _imageFiles)
            {
                FileInfo fileInfo = new(file);
                using Image objImage = Image.FromFile(fileInfo.FullName);
                values.Add((fileInfo, objImage.Height / objImage.Width == 1 ? Screen.Vertical : Screen.Horizontal));
            }

            foreach (var item in values) item.Item1.MoveTo(item.Item2 == Screen.Vertical ? _verticalPath : _horizontalPath + @"\" + item.Item1.Name);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) _verticalPath = folderBrowserDialog.SelectedPath;
            folderBrowserDialog.Dispose();

            button1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) _horizontalPath = folderBrowserDialog.SelectedPath;
            folderBrowserDialog.Dispose();

            button2.Enabled = true;
        }
    }
}