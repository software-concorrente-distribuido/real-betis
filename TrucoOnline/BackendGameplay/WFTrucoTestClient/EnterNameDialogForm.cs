using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFTrucoTestClient {
    public partial class EnterNameDialogForm : Form {
        private Form1 _form1;
        public EnterNameDialogForm(Form1 form1) {
            InitializeComponent();
            _form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e) {
            _form1.PlayerName = enterNameTextBox.Text;
            Dispose();
        }
    }
}
