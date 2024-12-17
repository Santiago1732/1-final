namespace Entidades.Final
{
    public class Usuario
    {
        protected string apellido;
        protected string clave;
        protected string correo;
        protected int dni;
        protected string nombre;

        public List<string> Correos { get; set; } = new List<string>();
        public string Apellido
        {
            get
            {
                return this.apellido;
            }
            set
            {
                this.apellido = value;
            }
        }

        public string Clave
        {
            get
            {
                return this.clave;
            }
        }

        public string Correo
        {
            get
            {
                return this.correo;
            }
            set
            {
                this.correo = value;
            }
        }

        public int Dni
        {
            get
            {
                return this.dni;
            }
            set
            {
                this.dni = value;
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public override string ToString()
        {
            return
                  $"{this.apellido}"
                + $"{this.clave}"
                + $"{this.correo}"
                + $"{this.dni}"
                + $"{this.nombre}";
        }

        public Usuario() { }

        public Usuario(string nombre, string apellido, int dni, string correo)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
            this.correo = correo;
        }

        public Usuario(string nombre, string apellido, int dni, string correo, string clave) :
            this(nombre, apellido, dni, correo)
        {
            this.clave = clave;
        }

    }
}
