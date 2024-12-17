using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp
{
    ///Agregar manejo de excepciones en TODOS los lugares críticos!!!

    public delegate void DelegadoThreadConParam(object param);

    public partial class FrmPrincipal : Form
    {
        protected Task hilo;
        protected CancellationTokenSource cts;

        public FrmPrincipal()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            this.Text = "Cambiar por su apellido y nombre";
            MessageBox.Show(this.Text);
        }

        ///
        /// CRUD
        ///
        private void listadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmListado frm = new FrmListado();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show(this);
        }

        ///
        /// VER LOG
        ///
        private void verLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult rta = DialogResult.OK;///Reemplazar por la llamada al método correspondiente del OpenFileDialog

            if (rta == DialogResult.OK)
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Configurar opciones del cuadro de diálogo
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*"; // Filtro de tipos de archivo
                openFileDialog.InitialDirectory = @"C:\\Users\\santi\\OneDrive\\Escritorio\\Logs\\logs.txt"; 
                openFileDialog.Title = "Selecciona un archivo"; // Título del cuadro de diálogo

                // Mostrar el cuadro de diálogo y obtener la respuesta
                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK) // Si el usuario seleccionó un archivo
                {
                    // Obtener la ruta completa del archivo seleccionado
                    string filePath = openFileDialog.FileName;
                    Console.WriteLine("Archivo seleccionado: " + filePath);
                }
                else
                {
                    Console.WriteLine("No se seleccionó ningún archivo.");
                }

                // Leer el contenido del archivo
                string contenidoArchivo = File.ReadAllText(openFileDialog.FileName);

                // Mostrar el contenido en el TextBox
                txtUsuariosLog.Text = contenidoArchivo;

                /// Mostrar en txtUsuariosLog.Text el contenido del archivo .log
            }
            else
            {
                MessageBox.Show("No se muestra .log");
            }
        }

        ///
        /// DESERIALIZAR JSON
        ///
        private void deserializarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Entidades.Final.Usuario> listado = null;
            string path = "C:\\Users\\santi\\OneDrive\\Escritorio\\Logs\\JSON.json"; 

            bool todoOK = false; /// Reemplazar por la llamada al método correspondiente de Manejadora

            Manejadora.DerealizarJson(path, listado);

            if (Manejadora.DerealizarJson(path, listado))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                this.txtUsuariosLog.Clear();
                DialogResult result = openFileDialog.ShowDialog();
                string contenidoArchivo = File.ReadAllText(openFileDialog.FileName);

                // Mostrar el contenido en el TextBox
                txtUsuariosLog.Text = contenidoArchivo;

                /// Mostrar en txtUsuariosLog.Text el contenido de la deserialización.
            }
            else
            {
                MessageBox.Show("NO se pudo deserializar a JSON");
            }

        }

        ///
        /// TASK
        ///
        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.cts = new CancellationTokenSource();
            ///Se inicia el hilo.
            this.hilo = null; /// inicializar tarea
                              ///Se desasocia al manejador de eventos.
            this.taskToolStripMenuItem.Click -= new EventHandler(this.taskToolStripMenuItem_Click);
        }


        ///PARA ACTUALIZAR LISTADO DESDE BD EN HILO
        public void ActualizarListadoUsuarios(object param)
        {
            /// Implementar...

        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            ///CANCELAR HILO
            this.cts.Cancel();
        }

        private void txtUsuariosLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
