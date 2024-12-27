namespace DIMARCore.UIEntities.DTOs
{
    public class CapitaniaSessionDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Sigla { get; set; }
        public int Categoria { get; set; }
        public string DescripcionCompleta => $"{Descripcion} - {Sigla}";
    }
}
