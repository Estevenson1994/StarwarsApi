namespace StarWarsApi.DataAccess.Entities
{
    public class FilmSpeciesMapping
    {
        public int FilmSpeciesMappingId { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int SpeciesId { get; set; }
        public Species Species { get; set; }

    }
}
