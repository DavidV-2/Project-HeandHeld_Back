using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace handheld_beta_api.Model
{
    public class ObtenerPedido
    {
        public int numero { get; set; }
        public int id_detalle { get; set; }
        public DateTime fecha { get; set; }
        public string codigo { get; set; }
        public int pendiente { get; set; }
        public string descripcion { get; set; }
    }
}
