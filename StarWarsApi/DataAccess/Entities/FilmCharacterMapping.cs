using System;
namespace StarWarsApi.DataAccess.Entities
{
    public class FilmCharacterMapping
    {
        public int FilmCharacterMappingId { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
