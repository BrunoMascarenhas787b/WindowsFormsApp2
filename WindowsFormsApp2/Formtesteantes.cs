using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Declarando a string de conexão com o banco de dados
        private string connectionString = "Server = OSA0716356W11-1\\SQLEXPRESS; Integrated Security= True;";

        public Form1()
        {
            InitializeComponent();
            CarregarDados(); // Chama o método de carregar os dados e abrir no formulário 
        }

        private void CarregarDados() // Método para carregar os dados do banco no list box
        {
            listBox1.Items.Clear(); // Limpa os itens da List Box antes de adicionar novos
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("USE MeuBanco SELECT Nome, Email, CPF FROM Usuarios", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Adiciona Nome, Email e CPF à list box separados por asterisco
                    listBox1.Items.Add(reader["Nome"].ToString() + " * " + reader["Email"].ToString() + " * " + reader["CPF"].ToString());
                }
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear(); // Limpar Nome
            txtEmail.Clear(); // Limpar Email
            txtCPF.Clear(); // Limpar a terceira TextBox (CPF)
            listBox1.ClearSelected(); // Remove a seleção da lista
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString)) // Cria Conexão com o banco
            {
                conn.Open();
                string query = "USE MeuBanco INSERT INTO Usuarios (Nome, Email, CPF) VALUES (@Nome, @Email, @CPF)";

                using (SqlCommand cmd = new SqlCommand(query, conn)) // Cria o comando SQL
                {
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@CPF", txtCPF.Text); // Parâmetro da terceira TextBox
                    cmd.ExecuteNonQuery(); // Executa o comando de inserção
                }

                CarregarDados(); // Atualizar os dados do listbox
                LimparCampos();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            // Obtém o nome da list box antes do primeiro asterisco
            string NomeSelecionado = listBox1.SelectedItem.ToString().Split('*')[0].Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "USE MeuBanco UPDATE Usuarios SET Nome = @Nome, Email = @Email, CPF = @CPF WHERE Nome = @NomeAntigo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@CPF", txtCPF.Text);
                    cmd.Parameters.AddWithValue("@NomeAntigo", NomeSelecionado);
                    cmd.ExecuteNonQuery(); // Atualiza dados no banco
                }
            }

            CarregarDados(); // Atualizar os dados do listbox
            LimparCampos();
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            // Obtém o nome antes do asterisco
            string nomeSelecionado = listBox1.SelectedItem.ToString().Split('*')[0].Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "USE MeuBanco DELETE FROM Usuarios WHERE Nome = @Nome";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", nomeSelecionado);
                    cmd.ExecuteNonQuery(); // Deleta o usuário
                }
            }
            CarregarDados(); // Atualizar os dados do listbox
            LimparCampos();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); // Limpa a lista para mostrar apenas o resultado da busca
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Busca por nomes que contenham o texto digitado (LIKE)
                string query = "USE MeuBanco SELECT Nome, Email, CPF FROM Usuarios WHERE Nome LIKE @Busca";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Busca", "%" + txtNome.Text + "%");
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader["Nome"].ToString() + " * " + reader["Email"].ToString() + " * " + reader["CPF"].ToString());
                    }
                }
            }
        }
    }
}
