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
    public partial class Form3 : Form
    {
        // String de conexão - Verifique se o nome do servidor e banco estão corretos
        private string connectionString = "Server = OSA0716356W11-1\\SQLEXPRESS; Database=MeuBanco; Integrated Security= True;";

        public Form3()
        {
            InitializeComponent();
            CarregarDados(); // Carrega a lista ao iniciar
        }

        // --- MÉTODO PARA CARREGAR DADOS ---
        private void CarregarDados()
        {
            try
            {
                // IMPORTANTE: Limpa a lista antes de carregar para NÃO REPETIR ITENS no grid/lista
                listBox1.Items.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Produto, Codigo, DataCompra FROM Compras";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Adiciona Produto * Código * Data à list box
                        listBox1.Items.Add(reader["Produto"].ToString() + " * " + reader["Codigo"].ToString() + " * " + reader["DataCompra"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        // --- MÉTODO PARA LIMPAR OS CAMPOS ---
        private void LimparCampos()
        {
            txtProduto.Clear();
            txtCodigo.Clear();
            listBox1.ClearSelected();
            txtProduto.Focus(); // Coloca o cursor de volta no nome do produto
        }

        // --- BOTÃO CADASTRAR (CORRIGIDO) ---
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProduto.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Por favor, preencha o Produto e o Código.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Compras (Produto, Codigo, DataCompra) VALUES (@Produto, @Codigo, @Data)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Produto", txtProduto.Text);
                        cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text);

                        // CORREÇÃO: Enviando a Data Atual do sistema (Hoje) para o banco
                        cmd.Parameters.AddWithValue("@Data", DateTime.Now.ToString("dd/MM/yyyy"));

                        cmd.ExecuteNonQuery(); // Executa o comando apenas UMA vez
                    }
                }

                MessageBox.Show("Produto cadastrado com sucesso!");
                CarregarDados(); // Recarrega a lista atualizada
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message);
            }
        }

        // --- BOTÃO PESQUISAR ---
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Produto, Codigo, DataCompra FROM Compras WHERE Produto LIKE @Busca";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Busca", "%" + txtProduto.Text + "%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader["Produto"].ToString() + " * " + reader["Codigo"].ToString() + " * " + reader["DataCompra"].ToString());
                    }
                }
            }
        }

        // --- BOTÃO DELETAR ---
        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Selecione um item na lista para deletar.");
                return;
            }

            // Pega o Código (segundo item após o primeiro '*') para deletar
            string codigoSelecionado = listBox1.SelectedItem.ToString().Split('*')[1].Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Compras WHERE Codigo = @Codigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigoSelecionado);
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarDados();
            LimparCampos();
        }

        // --- BOTÃO ALTERAR ---
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            string codigoAntigo = listBox1.SelectedItem.ToString().Split('*')[1].Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Compras SET Produto = @Produto, Codigo = @Codigo, DataCompra = @Data WHERE Codigo = @CodigoAntigo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Produto", txtProduto.Text);
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@Data", DateTime.Now.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@CodigoAntigo", codigoAntigo);
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarDados();
            LimparCampos();
        }

        // --- BOTÃO VOLTAR (PARA O MENU - FORM2) ---
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form2 menu = new Form2();
            menu.Show();
            this.Hide(); // Esconde a tela de produtos
        }

        
        private void btnAvançar_Click(object sender, EventArgs e)
        {
            Form33 telaUsuarios = new Form33();
            telaUsuarios.Show();
            this.Hide(); // Esconde a tela de produtos e avança
        }

       
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Se o usuário fechar no X, o programa inteiro encerra
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void btnVoltar_Click_1(object sender, EventArgs e)
        {
            Form2 menu = new Form2();
            menu.Show();

            
            this.Hide();
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                
                string linha = listBox1.SelectedItem.ToString();

                
                string[] partes = linha.Split('*');

                
                if (partes.Length >= 2)
                {
                    
                    txtProduto.Text = partes[0].Trim();
                    txtCodigo.Text = partes[1].Trim();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um item na lista primeiro!");
            }
        }
    }
}