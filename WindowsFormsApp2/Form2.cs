using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void usuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form33 form1 = new Form33();
            form1.Show();
            this.Hide();
        }
        

        private void produtosToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form3 telaProdutos = new Form3();
            telaProdutos.Show();
            this.Hide();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form telaProdutos = new Form3();
            telaProdutos.Show();
            this.Hide();
        }

        private void serviçosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b telaServicos = new b();
            telaServicos.Show();
            this.Hide(); // Esconde o menu principal
        }

        private void fordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. Cria a tela Form5
            Form5 telaFornecedor = new Form5();

            // 2. Mostra a tela Form5
            telaFornecedor.Show();

            // 3. Esconde o Menu Principal (Form2)
            this.Hide();
        }
    }
}
