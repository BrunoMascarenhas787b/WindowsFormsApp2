using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class b : Form
    {
        // Sua string de conexão com o SQL Server
        private string connectionString = "Server = OSA0716356W11-1\\SQLEXPRESS; Database=MeuBanco; Integrated Security= True;";

        public b()
        {
            InitializeComponent();
        }

        // --- 1. CARREGAR AO ABRIR A TELA ---
        private void b_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        // --- MÉTODO PARA PREENCHER O DATAGRIDVIEW ---
        private void CarregarDados()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Busca os dados com os nomes corretos das colunas
                    string query = "SELECT Servico, DataServico, Preco FROM Servicos";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt; // Joga os dados na tabela visual
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar: " + ex.Message); }
        }

        // --- 2. BOTÃO CADASTRAR ---
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Preencha pelo menos o nome do Serviço!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Servicos (Servico, DataServico, Preco) VALUES (@Servico, @Data, @Preco)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Servico", textBox1.Text); // textBox1 = Serviço
                        cmd.Parameters.AddWithValue("@Data", textBox2.Text);    // textBox2 = Data
                        cmd.Parameters.AddWithValue("@Preco", textBox3.Text);   // textBox3 = Preço
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Serviço Cadastrado com sucesso!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao cadastrar: " + ex.Message); }
        }

        // --- 3. BOTÃO PESQUISAR ---
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Pesquisa pelo nome do serviço
                    string query = "SELECT Servico, DataServico, Preco FROM Servicos WHERE Servico LIKE @Busca";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@Busca", "%" + textBox1.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro na pesquisa: " + ex.Message); }
        }

        // --- 4. BOTÃO ALTERAR ---
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Selecione um serviço primeiro para alterar.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Atualiza a Data e o Preço usando o Nome do Serviço como referência
                    string query = "UPDATE Servicos SET DataServico = @Data, Preco = @Preco WHERE Servico = @Servico";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Servico", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Data", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Preco", textBox3.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Serviço Alterado!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao alterar: " + ex.Message); }
        }

        // --- 5. BOTÃO DELETAR ---
        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Selecione ou digite o Serviço que deseja deletar.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Servicos WHERE Servico = @Servico";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Servico", textBox1.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Serviço Deletado!");
                CarregarDados();
                LimparCampos();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao deletar: " + ex.Message); }
        }

        // --- 6. BOTÃO LIMPAR ---
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1.Focus(); // Coloca o cursor piscando no primeiro campo
            CarregarDados(); // Recarrega a tabela caso estivesse no meio de uma pesquisa
        }

        // --- 7. SELECIONAR ITEM (Clicando no Botão) ---
        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells["Servico"].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells["DataServico"].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells["Preco"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Clique em uma linha da tabela primeiro!");
            }
        }

        // --- 8. SELECIONAR ITEM (Clicando direto na linha do Grid) ---
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["Servico"].Value.ToString();
                textBox2.Text = row.Cells["DataServico"].Value.ToString();
                textBox3.Text = row.Cells["Preco"].Value.ToString();
            }
        }
    }
}