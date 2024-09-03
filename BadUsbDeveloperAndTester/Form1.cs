using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadUsbDeveloperAndTester
{
    public partial class Form1 : Form
    {
        private bool isExecuting = false;
        private BadUsbInterpreter interpreter; // Adding the interpreter

        public Form1()
        {
            InitializeComponent();
            InitializeTextEditor();
            interpreter = new BadUsbInterpreter(); // Initializing the interpreter
        }

        private void InitializeTextEditor()
        {
            textEditor.Language = Language.Custom;
            textEditor.ClearStylesBuffer();

            // Create styles for keywords and strings
            var keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
            var stringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

            // Text change handler for dynamic syntax highlighting
            textEditor.TextChanged += (sender, args) =>
            {
                // Clear old styles
                args.ChangedRange.ClearStyle(keywordStyle);
                args.ChangedRange.ClearStyle(stringStyle);

                // Apply styles for keywords and strings
                args.ChangedRange.SetStyle(keywordStyle, @"\b(STRING|DELAY|GUI|REM)\b");
                args.ChangedRange.SetStyle(stringStyle, "\".*?\"");
            };
        }

        private async void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (isExecuting)
            {
                isExecuting = false;
                executeButton.Text = "Execute (F3)";
                statusLabel.Text = "Execution stopped";
                textEditor.ReadOnly = false;  // Unlock the text field
                return;
            }

            isExecuting = true;
            textEditor.ReadOnly = true; // Lock the text field
            executeButton.Text = "Stop (F4)";
            string[] lines = textEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                if (!isExecuting)
                    break;

                string line = lines[i];

                try
                {
                    statusLabel.Text = $"Executing, line {i + 1} out of {lines.Length}";
                    await interpreter.ExecuteLineAsync(line); // Pass the line to the interpreter
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Error on line {i + 1}: {ex.Message}";
                    isExecuting = false;
                    executeButton.Text = "Execute (F3)";
                    textEditor.ReadOnly = false;  // Unlock the text field
                    return;
                }
            }

            if (isExecuting)
            {
                statusLabel.Text = "Completed";
            }

            isExecuting = false;
            executeButton.Text = "Execute (F3)";
            textEditor.ReadOnly = false;  // Unlock the text field
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    textEditor.Text = fileContent;
                    statusLabel.Text = "File loaded";
                }
            }
        }

        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, textEditor.Text);
                    statusLabel.Text = "File saved";
                }
            }
        }

        private void textEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            // Example: updating text styles on change
            var keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
            var stringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

            // Clear old styles
            e.ChangedRange.ClearStyle(keywordStyle);
            e.ChangedRange.ClearStyle(stringStyle);

            // Separate keywords into multiple groups
            e.ChangedRange.SetStyle(keywordStyle, @"\b(STRING|DEFAULT_DELAY|DELAY)\b");
            e.ChangedRange.SetStyle(keywordStyle, @"\b(GUI|REM|REPEAT)\b");

            // Apply styles for string values
            e.ChangedRange.SetStyle(stringStyle, "\".*?\"");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
