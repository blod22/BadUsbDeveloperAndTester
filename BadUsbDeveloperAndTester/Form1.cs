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
        private DuckyScriptInterpreter interpreter; // Добавляем интерпретатор

        public Form1()
        {
            InitializeComponent();
            InitializeTextEditor();
            interpreter = new DuckyScriptInterpreter(); // Инициализируем интерпретатор
        }

        private void InitializeTextEditor()
        {
            textEditor.Language = Language.Custom;
            textEditor.ClearStylesBuffer();

            // Создаем стили для ключевых слов и строк
            var keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
            var stringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

            // Обработчик изменения текста для динамической подсветки синтаксиса
            textEditor.TextChanged += (sender, args) =>
            {
                // Очистка старых стилей
                args.ChangedRange.ClearStyle(keywordStyle);
                args.ChangedRange.ClearStyle(stringStyle);

                // Установка стилей для ключевых слов и строк
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
                return;
            }

            isExecuting = true;
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
                    await interpreter.ExecuteLineAsync(line); // Передаем строку в интерпретатор
                }
                catch (Exception ex)
                {
                    statusLabel.Text = $"Error on line {i + 1}: {ex.Message}";
                    isExecuting = false;
                    executeButton.Text = "Execute (F3)";
                    return;
                }
            }

            if (isExecuting)
            {
                statusLabel.Text = "Completed";
            }
            isExecuting = false;
            executeButton.Text = "Execute (F3)";
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
            // Пример: обновление стилей текста при изменении
            var keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
            var stringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

            // Очистка старых стилей
            e.ChangedRange.ClearStyle(keywordStyle);
            e.ChangedRange.ClearStyle(stringStyle);

            // Разделение ключевых слов на несколько групп
            e.ChangedRange.SetStyle(keywordStyle, @"\b(STRING|DEFAULT_DELAY|DELAY)\b");
            e.ChangedRange.SetStyle(keywordStyle, @"\b(GUI|REM|REPEAT)\b");

            // Установка стилей для строковых значений
            e.ChangedRange.SetStyle(stringStyle, "\".*?\"");
        }



    }
}
