using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace p1final2024
{
    public partial class Articulos : Form
    {
        ArticuloDAO articuloDAO = new ArticuloDAO();

        public Articulos()
        {
            InitializeComponent();
            dgvArticulo.DataBindingComplete += dgvArticulo_DataBindingComplete; 
            listarArticulos();
        }

        private void listarArticulos()
        {
            List<Articulo> articulos = articuloDAO.ReadAll();
            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = new BindingList<Articulo>(articulos);

            dgvArticulo.Refresh();
            dgvArticulo.Update();
            Application.DoEvents();

            if (dgvArticulo.Columns.Contains("Imagen"))
            {
                dgvArticulo.Columns["Imagen"].Visible = false;
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            TxtDescripcion.Clear();
            txtPrecio.Clear();
            pcbImagen.Image = null;
        }


        private void dgvArticulo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvArticulo.Columns.Contains("image"))
            {
                dgvArticulo.Columns["image"].Visible = false;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string archivoSeleccionado = openFileDialog.FileName;

                    pcbImagen.Image = Image.FromFile(archivoSeleccionado);
                }
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            guardarNuevo();
        }

        private void guardarNuevo()
        {
            try
            {
                Articulo articulo = new Articulo
                {
                    Nombre = txtNombre.Text,
                    Descripcion = TxtDescripcion.Text,
                    Precio = Convert.ToDecimal(txtPrecio.Text),
                    Imagen = obtenerImagen()
                };

                int nuevoId = articuloDAO.Create(articulo);
                if (nuevoId > 0)
                {
                    MessageBox.Show($"El artículo se guardó correctamente con el ID: {nuevoId}", "Guardar Artículo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listarArticulos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el artículo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el artículo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private byte[] obtenerImagen()
        {
            if (pcbImagen.Image == null)
            {
                MessageBox.Show("Por favor, selecciona una imagen.");
                return null;
            }

            byte[] imagenBytes;
            using (var ms = new System.IO.MemoryStream())
            {
                pcbImagen.Image.Save(ms, pcbImagen.Image.RawFormat);
                imagenBytes = ms.ToArray();
            }
            return imagenBytes;
        }

    }
}
