using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class ProductRepo
    {
        PmsCSp26Context db;
        public ProductRepo(PmsCSp26Context db)
        {
            this.db = db;
        }
        public bool Create(Product c)
        {
            db.Products.Add(c);
            return db.SaveChanges() > 0;
        }
        public List<Product> Get(string? searchTerm = null)
        {
            var query = db.Products.Include(x => x.CidNavigation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm));
            }

            return query.OrderBy(x => x.Name).ToList();
        }
        public Product? Get(int id)
        {
            return db.Products.Include(x => x.CidNavigation).FirstOrDefault(x => x.Id == id);
        }
        public bool Update(Product c)
        {
            var exobj = Get(c.Id);
            if (exobj == null)
            {
                return false;
            }

            db.Entry(exobj).CurrentValues.SetValues(c);
            return db.SaveChanges() > 0;
        }
        public bool Delete(int id)
        {
            var exobj = Get(id);
            if (exobj == null)
            {
                return false;
            }

            db.Products.Remove(exobj);
            return db.SaveChanges() > 0;
        }
    }
}
