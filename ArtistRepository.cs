using JMTI_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMTI_Test
{
    public class ArtistRepository
    {
        dbmusicEntities db = new dbmusicEntities();

        public void Save(Artist artist)
        {
            db.Artists.Add(artist);
            db.SaveChanges();
        }

        public Artist GetById(int id)
        {
            var artist = db.Artists.SingleOrDefault(a => a.ArtistId == id);
            return artist;
        }

        public List<Artist> GetAll()
        {
            var artists = db.Artists.ToList();
            return artists;
        }

        public void Update(Artist artist)
        {
            db.Entry(artist).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(Artist artist)
        {
            db.Artists.Remove(artist);
            db.SaveChanges();
        }
    }
}