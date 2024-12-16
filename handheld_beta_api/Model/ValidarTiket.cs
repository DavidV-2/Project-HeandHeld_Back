namespace handheld_beta_api.Model
{
    public class ValidarTiket
    {
        public string nit_proveedor {  get; set; }
        public decimal num_importacion {  get; set; }
        public int id_solicitud_det {  get; set; }
        public int numero_rollo {  get; set; }
        public decimal peso {  get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }

    }
}