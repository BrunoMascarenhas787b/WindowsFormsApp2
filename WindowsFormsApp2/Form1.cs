using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string connectionString ="Server = OSA0716356W11-1\\SQLEXPRESS; Integrated Security= True;"; //Declarando a string de conexão com o banco de dados

        public Form1()
        {
            InitializeComponent();
            CarregarDados(); //Chama o metódo de carregar os dados e abrir no formulário 
        }

        private void CarregarDados() //É o metodo para carregar os dados do banco no list box
        {
            listBox1.Items.Clear(); // Limpa os items da List Box antes de adicionar novos
            using (SqlConnection conn = new SqlConnection(connectionString)) //Cria a conexão com o banco de dados
            {
                conn.Open(); //Abre a conexão com o banco de dados
                SqlCommand cmd = new SqlCommand("USE MeuBanco SELECT Nome, Email FROM Usuarios", conn);  //Cria um comando SQL Para selecionar os usuários da tabela 
                SqlDataReader reader = cmd.ExecuteReader(); //Executa o comando e retorna um leitor de dados

                while (reader.Read()) //Percorre os resultados pela pesquisa
                {
                    listBox1.Items.Add(reader["Nome"].ToString() + " * " + reader["Email"].ToString());
                    //Adiciona o nome e o email de cada usuário a list box



                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void LimparCampos()
        {
            txt.Nome Clear();
            txtEmail.Clear();

        }
    }
}
