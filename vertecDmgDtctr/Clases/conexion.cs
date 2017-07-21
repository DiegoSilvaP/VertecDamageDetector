using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
//using MySql;
//using MySql.Data;
using System.Threading;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.Data;
using Npgsql;

namespace vertecDmgDtctr.Clases
{
    public class conexion //Se hace publica la clase para poder utilizar los metodos
    {
        public NpgsqlConnection conection = new NpgsqlConnection();//Objeto para usar las funciones de abrir o cerrar la conexion
        //Se crea un string con los datos para la conexion
        public string cadenaConexion = "Server=localhost;Port=5432;User Id=postgres;Password=moviles;Database=postgres";
        // Se hace un metodo en donde comprueba que los datos de conexion no son nulos
        public NpgsqlConnection AbreConexion()
        {
            if (!string.IsNullOrWhiteSpace(cadenaConexion))
            {
                try //intenta hacer conexion a la base de datos, si sucede nos manda un mensaje de exito, de lo contrario nos dice la falla 
                {
                    conection = new NpgsqlConnection(cadenaConexion);
                    conection.Open();
                }
                catch (Exception)
                {
                    conection.Close();
                }
            }
            return conection;
        }
        // En este metodo recibe las rutas donde estan las imagenes y convierte cada una a un array de bytes
        public void GuardarImg(string id_img,string urlOriginal,string urlProcessed,string urlFilled, float devestprocessed, float meanprocessed, float entropyprocessed, float devestfilled, float meanfilled, float entropyfilled)
        {
            FileStream fileStream;
            BinaryReader binaryReader;
            try
            {
                byte[] ImageDataProcessed;
                fileStream = new FileStream(urlProcessed, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);
                ImageDataProcessed = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();

                byte[] ImageDataFilled;
                fileStream = new FileStream(urlFilled, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);
                ImageDataFilled = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();

                byte[] ImageDataOriginal;
                fileStream = new FileStream(urlOriginal, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);
                ImageDataOriginal = binaryReader.ReadBytes((int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();
                
                conection = new NpgsqlConnection(cadenaConexion);
                // Se abre la conexion
                //conection.Open();
                //Se declara la consulta deseada, en este caso se van a guardar las imagenes en la tabla publica post,en sus respectivos campos
                string consulta = "INSERT INTO public.images(id_images,imgoriginal,imgprocessed,imgfilled,devestprocessed,meanprocessed,entropyprocessed,devestfilled,meanfilled,entropyfilled) VALUES(@id_images,@imgoriginal,@imgprocessed,@imgfilled,@devestprocessed,@meanprocessed,@entropyprocessed,@devestfilled,@meanfilled,@entropyfilled)";
                NpgsqlCommand comand = new NpgsqlCommand(consulta, conection);
                // Se definen los parametros de las consultas
            
                comand.Parameters.Add("@id_images", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@id_images"].Value = id_img;
                comand.Parameters.Add("@imgoriginal", NpgsqlTypes.NpgsqlDbType.Bytea);
                comand.Parameters["@imgoriginal"].Value = ImageDataOriginal;
                comand.Parameters.Add("@imgprocessed", NpgsqlTypes.NpgsqlDbType.Bytea);
                comand.Parameters["@imgprocessed"].Value = ImageDataProcessed;
                comand.Parameters.Add("@imgfilled", NpgsqlTypes.NpgsqlDbType.Bytea);
                comand.Parameters["@imgfilled"].Value = ImageDataFilled;
                comand.Parameters.Add("@devestprocessed", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@devestprocessed"].Value = devestprocessed;
                comand.Parameters.Add("@meanprocessed", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@meanprocessed"].Value = meanprocessed;
                comand.Parameters.Add("@entropyprocessed", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@entropyprocessed"].Value = entropyprocessed;
                comand.Parameters.Add("@devestfilled", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@devestfilled"].Value = devestfilled;
                comand.Parameters.Add("@meanfilled", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@meanfilled"].Value = meanfilled;
                comand.Parameters.Add("@entropyfilled", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@entropyfilled"].Value = entropyfilled;
                // Se ejecuta el comando y si es exitoso nos manda un mensaje de exito, de lo contrario nos dice la falla
                comand.ExecuteNonQuery();
                //MessageBox.Show("Imagenes Agregadas !!!");
                Thread.Sleep(500);
                conection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo !!!\n" + ex);
                conection.Close();
            }
        }
        public void GuardarInfo(string imei, string fabricante, string marca, string modelo, string version,string serial,int daño)
        {
            try {
                conection = new NpgsqlConnection(cadenaConexion);
                // Se abre la conexion
                //conection.Open();
                //Se declara la consulta deseada, en este caso se van a guardar las imagenes en la tabla publica post,en sus respectivos campos
                string consulta = "INSERT INTO public.information(imei,fabricante,marca,modelo,version,serial,daño) VALUES(@imei,@fabricante,@marca,@modelo,@version,@serial,@daño)";
                NpgsqlCommand comand = new NpgsqlCommand(consulta, conection);
                // Se definen los parametros de las consultas
                comand.Parameters.Add("@imei", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@imei"].Value = imei;
                comand.Parameters.Add("@fabricante", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@fabricante"].Value = fabricante;
                comand.Parameters.Add("@marca", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@marca"].Value = marca;
                comand.Parameters.Add("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@modelo"].Value = modelo;
                comand.Parameters.Add("@version", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@version"].Value = version;
                comand.Parameters.Add("@serial", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@serial"].Value = serial;
                comand.Parameters.Add("@daño", NpgsqlTypes.NpgsqlDbType.Integer);
                comand.Parameters["@daño"].Value = daño;
                //MessageBox.Show(comand.CommandText);
                comand.ExecuteNonQuery();
            //MessageBox.Show("Informacion agregada !!!");
            Thread.Sleep(500);
            conection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo la info !!!\n" + ex);
                conection.Close();
            }
        }




        public void GuardarDatos(float sumatoriaoriginal, float sumatoriafill, float sumatoriaprocess, float mediarellenada, float mediaprocess, float stdfill, float stdprocess, string daño)
        {
            try
            {
                conection = new NpgsqlConnection(cadenaConexion);
                // Se abre la conexion
                conection.Open();
                //Se declara la consulta deseada, en este caso se van a guardar las imagenes en la tabla publica post,en sus respectivos campos
                string consulta = "INSERT INTO public.datos(sumatoriaoriginal,  sumatoriafill,  sumatoriaprocess,mediarellenada,mediaprocess, stdfill,  stdprocess,cantidaddaño) VALUES(@sumatoriaoriginal,  @sumatoriafill,  @sumatoriaprocess,@mediarellenada,@mediaprocess, @stdfill,  @stdprocess, @cantidaddaño)";
                NpgsqlCommand comand = new NpgsqlCommand(consulta, conection);
                // Se definen los parametros de las consultas
                comand.Parameters.Add("@sumatoriaoriginal", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@sumatoriaoriginal"].Value = sumatoriaoriginal;
                comand.Parameters.Add("@sumatoriafill", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@sumatoriafill"].Value = sumatoriafill;
                comand.Parameters.Add("@sumatoriaprocess", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@sumatoriaprocess"].Value = sumatoriaprocess;
                comand.Parameters.Add("@mediarellenada", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@mediarellenada"].Value = mediarellenada;
                comand.Parameters.Add("@mediaprocess", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@mediaprocess"].Value = mediaprocess;
                comand.Parameters.Add("@stdfill", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@stdfill"].Value = stdfill;
                comand.Parameters.Add("@stdprocess", NpgsqlTypes.NpgsqlDbType.Real);
                comand.Parameters["@stdprocess"].Value = stdprocess;
                comand.Parameters.Add("@cantidaddaño", NpgsqlTypes.NpgsqlDbType.Varchar);
                comand.Parameters["@cantidaddaño"].Value = daño;
                comand.ExecuteNonQuery();
                //MessageBox.Show("Informacion agregada !!!");
                Thread.Sleep(500);
                conection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo la info !!!\n" + ex);
                conection.Close();
            }
        }




    }
}