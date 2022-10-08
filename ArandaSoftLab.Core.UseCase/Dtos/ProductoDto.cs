namespace ArandaSoftLab.Core.UseCase.Dtos
{
    public class ProductoDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string Imagen { get; set; }
    }
}
