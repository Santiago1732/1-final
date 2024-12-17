﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Entidades.Final;
using System.IO;
using Microsoft.VisualBasic.ApplicationServices;

namespace WinFormsApp
{
    public partial class FrmListado : Form
    {
        List<Usuario> lista;
        public FrmListado()
        {
            InitializeComponent();

            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FrmListado_Load(object sender, EventArgs e)
        {
            ///Utilizando la clase ADO, obtener y mostrar a todos los usuarios
            ///

            //ESTO ANDA
            ADO.ApellidoUsuarioExistente += this.Manejador_apellidoExistenteLog;
            ADO.ApellidoUsuarioExistente += this.Manejador_apellidoExistenteJSON;

            this.lista = ADO.ObtenerTodos();
            this.dataGridView1.DataSource = this.lista;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        { 

            
            ///Agregar un nuevo usuario a la base de datos
            ///Utilizar FrmUsuario.
            ///Agregar manejadores de eventos (punto 14)
            
            FrmUsuario frm = new FrmUsuario();

            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lista.Add(frm.MiUsuario);
                ADO.Agregar(frm.MiUsuario);
            }
            this.lista = ADO.ObtenerTodos();
            this.dataGridView1.DataSource = this.lista;

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ///Modificar el usuario seleccionado (el DNI no se debe modificar, adecuar FrmUsuario)
            ///reutilizar FrmUsuario
            int i = this.dataGridView1.SelectedRows[0].Index;

            if (i < 0) { return; }

            Usuario user = this.lista[i];

            FrmUsuario frm = new FrmUsuario(user);
            frm.StartPosition = FormStartPosition.CenterParent;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ADO.ModificarUsuario(frm.MiUsuario);
            }
            this.lista = ADO.ObtenerTodos();
            this.dataGridView1.DataSource = this.lista;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ///Eliminar el usuario seleccionado 
            ///reutilizar FrmUsuario
            ///
            int i = this.dataGridView1.SelectedRows[0].Index;

            if (i < 0) { return; }

            Usuario user = this.lista[i];

            FrmUsuario frm = new FrmUsuario(user);
            frm.StartPosition = FormStartPosition.CenterParent;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ADO.Eliminar(frm.MiUsuario);
            }
            this.lista = ADO.ObtenerTodos();
            this.dataGridView1.DataSource = this.lista;
        }

        ///Si el apellido ya existe en la base, se disparará el evento ApellidoUsuarioExistente. 
        ///Diseñarlo (de acuerdo a las convenciones vistas) en la clase ADO. 
        ///Crear el manejador necesario para que, una vez capturado el evento, se guarde:
        ///1) en un archivo de texto: 
        ///la fecha (con hora, minutos y segundos) y en un nuevo renglón, el apellido y todos
        ///los correos electrónicos para ese apellido.
        ///Se deben acumular los mensajes. 
        ///El archivo se guardará con el nombre 'usuarios.log' en la carpeta 'Mis documentos' del cliente.
        ///2) en un archivo JSON:
        ///serializar todos los objetos de tipo Usuario cuyo apellido esté repetido.
        ///El archivo se guardará en el escritorio del cliente, con el nombre 'usuarios_repetidos.json'
        ///
        ///El manejador de eventos (Manejador_apellidoExistenteLog) invocará al método (de clase) 
        ///EscribirArchivo(List<Usuario>) (se alojará en la clase Manejadora en Entidades), 
        ///que retorna un booleano indicando si se pudo escribir o no.
        ///            
        ///El manejador de eventos (Manejador_apellidoExistenteJSON) invocará al método (de clase) 
        ///SerializarJSON(List<Usuario>, string) (se alojará en la clase Manejadora en Entidades), 
        ///que retorna un booleano indicando si se pudo escribir o no.
        ///



        private void Manejador_apellidoExistenteLog(object sender, EventArgs e)
        {
            MessageBox.Show("Apellido repetido log!!!");

            //bool todoOK = false;///Reemplazar por la llamada al método de clase Manejadora.EscribirArchivo
            if (Manejadora.EscribirArchivo(lista))
            {
                MessageBox.Show("Se escribió correctamente!!!");
            }
            else
            {
                MessageBox.Show("No se pudo escribir!!!");
            }
            
        }

        private void Manejador_apellidoExistenteJSON(object sender, EventArgs e)
        {
            string path = "C:\\Users\\santi\\OneDrive\\Escritorio\\Logs\\JSON.json";
            
            MessageBox.Show("Apellido repetido JSON!!!");

            if (Manejadora.SerealizarJson(lista, path))
            {
                MessageBox.Show("Se escribió correctamente!!!");
            }
            else
            {
                MessageBox.Show("No se pudo escribir!!!");
            }
        }

    }
}
