using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form5 : Form
    {
        // String de conexão com o MeuBanco
        private string connectionString = "Server = OSA0716356W11-1\\SQLEXPRESS; Database=MeuBanco; Integrated Security= True;";

        public Form5()
        {
            InitializeComponent();
            CarregarDados(); // Carrega a lista de fornecedores assim que a tela abre
        }

        // --- 1. CARREGAR DADOS NO LISTBOX ---
        private void CarregarDados()
        {
            try
            {
                listBox1.Items.Clear();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CNPJ, Nome, Telefone, Email, Categoria FROM Fornecedores";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            // Junta os 5 campos separados por " * "
                            listBox1.Items.Add(reader["CNPJ"].ToString() + " * " +
                                               reader["Nome"].ToString() + " * " +
                                               reader["Telefone"].ToString() + " * " +
                                               reader["Email"].ToString() + " * " +
                                               reader["Categoria"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar: " + ex.Message); }
        }

        // --- 2. CADASTRAR ---
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Preencha pelo menos o CNPJ e o Nome do fornecedor!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Fornecedores (CNPJ, Nome, Telefone, Email, Categoria) VALUES (@CNPJ, @Nome, @Tel, @Email, @Cat)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CNPJ", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Nome", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Tel", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Cat", textBox5.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Fornecedor Cadastrado com sucesso!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao cadastrar: " + ex.Message); }
        }

        // --- 3. PESQUISAR ---
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Pesquisa pelo Nome (textBox2)
                    string query = "SELECT CNPJ, Nome, Telefone, Email, Categoria FROM Fornecedores WHERE Nome LIKE @Busca";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Busca", "%" + textBox2.Text + "%");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["CNPJ"].ToString() + " * " + reader["Nome"].ToString() + " * " + reader["Telefone"].ToString() + " * " + reader["Email"].ToString() + " * " + reader["Categoria"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro na pesquisa: " + ex.Message); }
        }

        // --- 4. ALTERAR ---
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Preencha o CNPJ do fornecedor que deseja alterar.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Fornecedores SET Nome = @Nome, Telefone = @Tel, Email = @Email, Categoria = @Cat WHERE CNPJ = @CNPJ";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CNPJ", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Nome", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Tel", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Email", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Cat", textBox5.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Fornecedor Alterado!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao alterar: " + ex.Message); }
        }

        // --- 5. DELETAR ---
        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Selecione ou digite o CNPJ que deseja deletar.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Fornecedores WHERE CNPJ = @CNPJ";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CNPJ", textBox1.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Fornecedor Deletado!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao deletar: " + ex.Message); }
        }

        // --- 6. LIMPAR ---
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            listBox1.ClearSelected();
            textBox1.Focus();
            CarregarDados();
        }

        // --- 7. BOTÃO SELECIONAR (Joga da ListBox para as 5 TextBoxes) ---
        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                // Corta a linha onde tem o " * "
                string[] partes = listBox1.SelectedItem.ToString().Split('*');

                // Distribui os 5 pedaços para as caixas de texto
                if (partes.Length >= 5)
                {
                    textBox1.Text = partes[0].Trim(); // CNPJ
                    textBox2.Text = partes[1].Trim(); // Nome
                    textBox3.Text = partes[2].Trim(); // Telefone
                    textBox4.Text = partes[3].Trim(); // Email
                    textBox5.Text = partes[4].Trim(); // Categoria
                }
            }
            else
            {
                MessageBox.Show("Clique em um fornecedor da lista primeiro!");
            }
        }

        // Esses métodos podem ficar vazios se você não for digitar nada que precise alterar a tela na mesma hora
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void Form5_Load(object sender, EventArgs e)
        {
            CarregarDados(); // Puxa os fornecedores do banco e joga no ListBox
        }
    }
}