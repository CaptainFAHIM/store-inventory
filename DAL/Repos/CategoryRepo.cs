using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class CategoryRepo
    {
        PmsCSp26Context db;
        public CategoryRepo(PmsCSp26Context db) { 
            this.db = db;
        }
        public bool Create(Category c) { 
            db.Categories.Add(c);
            return db.SaveChanges() > 0;
        }
        public List<Category> Get() { 
            return db.Categories.OrderBy(x => x.Name).ToList();
        }
        public Category? Get(int id) {
            return db.Categories.Find(id);
        }
        public bool Update(Category c) {
            var exobj = Get(c.Id);
            if (exobj == null)
            {
                return false;
            }

            db.Entry(exobj).CurrentValues.SetValues(c);
            return db.SaveChanges() > 0;
        }
        public bool Delete(int id) {
            var exobj = Get(id);
            if (exobj == null)
            {
                return false;
            }

            if (db.Products.Any(x => x.Cid == id))
            {
                return false;
            }

            db.Categories.Remove(exobj);    
            return db.SaveChanges() > 0;
        }
    }
}
