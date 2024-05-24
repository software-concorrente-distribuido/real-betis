namespace WFTrucoTestClient {
    partial class EnterNameDialogForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            enterNameTextBox = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // enterNameTextBox
            // 
            enterNameTextBox.Location = new Point(47, 38);
            enterNameTextBox.Name = "enterNameTextBox";
            enterNameTextBox.PlaceholderText = "Insira seu nome...";
            enterNameTextBox.Size = new Size(100, 23);
            enterNameTextBox.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(47, 67);
            button1.Name = "button1";
            button1.Size = new Size(100, 23);
            button1.TabIndex = 1;
            button1.Text = "Confirmar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // EnterNameDialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(190, 117);
            Controls.Add(button1);
            Controls.Add(enterNameTextBox);
            Name = "EnterNameDialogForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox enterNameTextBox;
        private Button button1;
    }
}