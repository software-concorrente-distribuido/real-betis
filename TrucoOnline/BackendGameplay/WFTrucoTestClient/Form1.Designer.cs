namespace WFTrucoTestClient {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            hostServerTextBox = new TextBox();
            Host = new Label();
            connectToServerButton = new Button();
            serverInfoText = new Label();
            lobbyIdTextBox = new TextBox();
            enterLobbyButton = new Button();
            yourCardsBox = new GroupBox();
            card3Button = new Button();
            card2Button = new Button();
            card1Button = new Button();
            montePanel = new Panel();
            monteCard4 = new Button();
            monteCard3 = new Button();
            monteCard2 = new Button();
            monteCard1 = new Button();
            scoreLabel = new Label();
            team1PointsLabel = new Label();
            team2PointsLabel = new Label();
            cangadoLabel = new Label();
            playersLabel = new Label();
            playersListLabel = new Label();
            startLobbyButton = new Button();
            roundScoreLabel = new Label();
            yourCardsBox.SuspendLayout();
            montePanel.SuspendLayout();
            SuspendLayout();
            // 
            // hostServerTextBox
            // 
            hostServerTextBox.Location = new Point(615, 51);
            hostServerTextBox.Name = "hostServerTextBox";
            hostServerTextBox.PlaceholderText = "192.168.0.0";
            hostServerTextBox.Size = new Size(100, 23);
            hostServerTextBox.TabIndex = 0;
            hostServerTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // Host
            // 
            Host.AutoSize = true;
            Host.Location = new Point(615, 29);
            Host.Name = "Host";
            Host.Size = new Size(50, 15);
            Host.TabIndex = 1;
            Host.Text = "Servidor";
            Host.Click += label1_Click;
            // 
            // connectToServerButton
            // 
            connectToServerButton.Location = new Point(615, 82);
            connectToServerButton.Name = "connectToServerButton";
            connectToServerButton.Size = new Size(100, 23);
            connectToServerButton.TabIndex = 2;
            connectToServerButton.Text = "Conectar";
            connectToServerButton.UseVisualStyleBackColor = true;
            connectToServerButton.Click += connectToServerButton_Click;
            // 
            // serverInfoText
            // 
            serverInfoText.AutoSize = true;
            serverInfoText.Location = new Point(616, 115);
            serverInfoText.Name = "serverInfoText";
            serverInfoText.Size = new Size(0, 15);
            serverInfoText.TabIndex = 3;
            // 
            // lobbyIdTextBox
            // 
            lobbyIdTextBox.Location = new Point(615, 208);
            lobbyIdTextBox.Name = "lobbyIdTextBox";
            lobbyIdTextBox.PlaceholderText = "Insira o ID do Lobby...";
            lobbyIdTextBox.Size = new Size(130, 23);
            lobbyIdTextBox.TabIndex = 4;
            lobbyIdTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // enterLobbyButton
            // 
            enterLobbyButton.Location = new Point(615, 244);
            enterLobbyButton.Name = "enterLobbyButton";
            enterLobbyButton.Size = new Size(75, 23);
            enterLobbyButton.TabIndex = 5;
            enterLobbyButton.Text = "Entrar";
            enterLobbyButton.UseVisualStyleBackColor = true;
            enterLobbyButton.Click += enterLobbyButton_Click;
            // 
            // yourCardsBox
            // 
            yourCardsBox.BackColor = SystemColors.ControlDark;
            yourCardsBox.Controls.Add(card3Button);
            yourCardsBox.Controls.Add(card2Button);
            yourCardsBox.Controls.Add(card1Button);
            yourCardsBox.Location = new Point(107, 293);
            yourCardsBox.Name = "yourCardsBox";
            yourCardsBox.Size = new Size(220, 100);
            yourCardsBox.TabIndex = 6;
            yourCardsBox.TabStop = false;
            yourCardsBox.Text = "Suas cartas";
            // 
            // card3Button
            // 
            card3Button.Location = new Point(150, 22);
            card3Button.Name = "card3Button";
            card3Button.Size = new Size(61, 72);
            card3Button.TabIndex = 2;
            card3Button.UseVisualStyleBackColor = true;
            card3Button.Click += card3Button_Click;
            // 
            // card2Button
            // 
            card2Button.Location = new Point(80, 22);
            card2Button.Name = "card2Button";
            card2Button.Size = new Size(61, 72);
            card2Button.TabIndex = 1;
            card2Button.UseVisualStyleBackColor = true;
            card2Button.Click += card2Button_Click;
            // 
            // card1Button
            // 
            card1Button.Location = new Point(10, 22);
            card1Button.Name = "card1Button";
            card1Button.Size = new Size(61, 72);
            card1Button.TabIndex = 0;
            card1Button.UseVisualStyleBackColor = true;
            card1Button.Click += card1Button_Click;
            // 
            // montePanel
            // 
            montePanel.Controls.Add(monteCard4);
            montePanel.Controls.Add(monteCard3);
            montePanel.Controls.Add(monteCard2);
            montePanel.Controls.Add(monteCard1);
            montePanel.Location = new Point(70, 115);
            montePanel.Name = "montePanel";
            montePanel.Size = new Size(290, 100);
            montePanel.TabIndex = 7;
            // 
            // monteCard4
            // 
            monteCard4.Enabled = false;
            monteCard4.Location = new Point(219, 12);
            monteCard4.Name = "monteCard4";
            monteCard4.Size = new Size(61, 72);
            monteCard4.TabIndex = 6;
            monteCard4.UseVisualStyleBackColor = true;
            // 
            // monteCard3
            // 
            monteCard3.Enabled = false;
            monteCard3.Location = new Point(150, 12);
            monteCard3.Name = "monteCard3";
            monteCard3.Size = new Size(61, 72);
            monteCard3.TabIndex = 5;
            monteCard3.UseVisualStyleBackColor = true;
            // 
            // monteCard2
            // 
            monteCard2.Enabled = false;
            monteCard2.Location = new Point(80, 12);
            monteCard2.Name = "monteCard2";
            monteCard2.Size = new Size(61, 72);
            monteCard2.TabIndex = 4;
            monteCard2.UseVisualStyleBackColor = true;
            // 
            // monteCard1
            // 
            monteCard1.Enabled = false;
            monteCard1.Location = new Point(10, 12);
            monteCard1.Name = "monteCard1";
            monteCard1.Size = new Size(61, 72);
            monteCard1.TabIndex = 3;
            monteCard1.UseVisualStyleBackColor = true;
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.Location = new Point(407, 35);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(39, 15);
            scoreLabel.TabIndex = 8;
            scoreLabel.Text = "Placar";
            // 
            // team1PointsLabel
            // 
            team1PointsLabel.AutoSize = true;
            team1PointsLabel.Location = new Point(408, 57);
            team1PointsLabel.Name = "team1PointsLabel";
            team1PointsLabel.Size = new Size(54, 15);
            team1PointsLabel.TabIndex = 9;
            team1PointsLabel.Text = "Time 1: 0";
            // 
            // team2PointsLabel
            // 
            team2PointsLabel.AutoSize = true;
            team2PointsLabel.Location = new Point(408, 81);
            team2PointsLabel.Name = "team2PointsLabel";
            team2PointsLabel.Size = new Size(54, 15);
            team2PointsLabel.TabIndex = 10;
            team2PointsLabel.Text = "Time 2: 0";
            // 
            // cangadoLabel
            // 
            cangadoLabel.AutoSize = true;
            cangadoLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cangadoLabel.ForeColor = Color.OrangeRed;
            cangadoLabel.Location = new Point(72, 83);
            cangadoLabel.Name = "cangadoLabel";
            cangadoLabel.Size = new Size(95, 21);
            cangadoLabel.TabIndex = 11;
            cangadoLabel.Text = "CANGADO!";
            cangadoLabel.Visible = false;
            // 
            // playersLabel
            // 
            playersLabel.AutoSize = true;
            playersLabel.Location = new Point(412, 134);
            playersLabel.Name = "playersLabel";
            playersLabel.Size = new Size(127, 15);
            playersLabel.TabIndex = 12;
            playersLabel.Text = "Jogadores conectados:";
            // 
            // playersListLabel
            // 
            playersListLabel.AutoSize = true;
            playersListLabel.Location = new Point(414, 158);
            playersListLabel.Name = "playersListLabel";
            playersListLabel.Size = new Size(0, 15);
            playersListLabel.TabIndex = 13;
            playersListLabel.Visible = false;
            // 
            // startLobbyButton
            // 
            startLobbyButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            startLobbyButton.Location = new Point(614, 346);
            startLobbyButton.Name = "startLobbyButton";
            startLobbyButton.Size = new Size(101, 47);
            startLobbyButton.TabIndex = 14;
            startLobbyButton.Text = "Começar Partida";
            startLobbyButton.UseVisualStyleBackColor = true;
            startLobbyButton.Visible = false;
            startLobbyButton.Click += startLobbyButton_Click;
            // 
            // roundScoreLabel
            // 
            roundScoreLabel.AutoSize = true;
            roundScoreLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            roundScoreLabel.Location = new Point(193, 244);
            roundScoreLabel.Name = "roundScoreLabel";
            roundScoreLabel.Size = new Size(45, 21);
            roundScoreLabel.TabIndex = 15;
            roundScoreLabel.Text = "0 x 0";
            roundScoreLabel.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(roundScoreLabel);
            Controls.Add(startLobbyButton);
            Controls.Add(playersListLabel);
            Controls.Add(playersLabel);
            Controls.Add(cangadoLabel);
            Controls.Add(team2PointsLabel);
            Controls.Add(team1PointsLabel);
            Controls.Add(scoreLabel);
            Controls.Add(montePanel);
            Controls.Add(yourCardsBox);
            Controls.Add(enterLobbyButton);
            Controls.Add(lobbyIdTextBox);
            Controls.Add(serverInfoText);
            Controls.Add(connectToServerButton);
            Controls.Add(Host);
            Controls.Add(hostServerTextBox);
            Name = "Form1";
            Text = "Truco Goiano";
            Load += Form1_Load;
            yourCardsBox.ResumeLayout(false);
            montePanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox hostServerTextBox;
        private Label Host;
        private Button connectToServerButton;
        private Label serverInfoText;
        private TextBox lobbyIdTextBox;
        private Button enterLobbyButton;
        private GroupBox yourCardsBox;
        private Button card3Button;
        private Button card2Button;
        private Button card1Button;
        private Panel montePanel;
        private Button monteCard4;
        private Button monteCard3;
        private Button monteCard2;
        private Button monteCard1;
        private Label scoreLabel;
        private Label team1PointsLabel;
        private Label team2PointsLabel;
        private Label cangadoLabel;
        private Label playersLabel;
        private Label playersListLabel;
        private Button startLobbyButton;
        private Label roundScoreLabel;
    }
}
