namespace BadUsbDeveloperAndTester
{
    partial class Form1
    {
        private FastColoredTextBoxNS.FastColoredTextBox textEditor;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Label statusLabel;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            textEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            loadFileButton = new Button();
            saveFileButton = new Button();
            executeButton = new Button();
            statusLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)textEditor).BeginInit();
            SuspendLayout();
            // 
            // textEditor
            // 
            textEditor.AutoCompleteBracketsList = new char[]
    {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
    };
            textEditor.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
            textEditor.AutoScrollMinSize = new Size(2, 14);
            textEditor.BackBrush = null;
            textEditor.CharHeight = 14;
            textEditor.CharWidth = 8;
            textEditor.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            textEditor.Dock = DockStyle.Top;
            textEditor.Hotkeys = "Ctrl+C=Copy, Ctrl+V=Paste, Ctrl+X=Cut, Ctrl+Y=Redo, Ctrl+Z=Undo";
            textEditor.IsReplaceMode = false;
            textEditor.Location = new Point(0, 0);
            textEditor.Name = "textEditor";
            textEditor.Paddings = new Padding(0);
            textEditor.SelectionColor = Color.FromArgb(60, 0, 0, 255);
            textEditor.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("textEditor.ServiceColors");
            textEditor.Size = new Size(800, 300);
            textEditor.TabIndex = 0;
            textEditor.Zoom = 100;
            textEditor.TextChanged += textEditor_TextChanged;
            // 
            // loadFileButton
            // 
            loadFileButton.Dock = DockStyle.Top;
            loadFileButton.Location = new Point(0, 300);
            loadFileButton.Name = "loadFileButton";
            loadFileButton.Size = new Size(800, 23);
            loadFileButton.TabIndex = 1;
            loadFileButton.Text = "Load File";
            loadFileButton.UseVisualStyleBackColor = true;
            loadFileButton.Click += LoadFileButton_Click;
            // 
            // saveFileButton
            // 
            saveFileButton.Dock = DockStyle.Top;
            saveFileButton.Location = new Point(0, 323);
            saveFileButton.Name = "saveFileButton";
            saveFileButton.Size = new Size(800, 23);
            saveFileButton.TabIndex = 2;
            saveFileButton.Text = "Save File";
            saveFileButton.UseVisualStyleBackColor = true;
            saveFileButton.Click += SaveFileButton_Click;
            // 
            // executeButton
            // 
            executeButton.Dock = DockStyle.Top;
            executeButton.Location = new Point(0, 346);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(800, 23);
            executeButton.TabIndex = 3;
            executeButton.Text = "Execute (F3)";
            executeButton.UseVisualStyleBackColor = true;
            executeButton.Click += ExecuteButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Location = new Point(0, 435);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 15);
            statusLabel.TabIndex = 4;
            statusLabel.Text = "Ready";
            // 
            // Form1
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(statusLabel);
            Controls.Add(executeButton);
            Controls.Add(saveFileButton);
            Controls.Add(loadFileButton);
            Controls.Add(textEditor);
            Name = "Form1";
            Text = "BadUsb Editor";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)textEditor).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.ComponentModel.IContainer components;
    }
}
