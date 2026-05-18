using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class ProductService
    {
        ProductRepo repo;
        Mapper mapper;
        public ProductService(ProductRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }
        public List<ProductDTO> Get(string? searchTerm = null, int? categoryId = null, bool lowStockOnly = false)
        {
            var data = repo.Get(searchTerm, categoryId, lowStockOnly);
            var res = mapper.Map<List<ProductDTO>>(data);
            return res;
        }
        public ProductDTO? Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<ProductDTO>(data);
            return res;
        }
        public bool Create(ProductDTO c)
        {
            var data = mapper.Map<Product>(c);
            var res = repo.Create(data);
            return res;

        }
        public bool Update(ProductDTO c)
        {
            var data = mapper.Map<Product>(c);
            var res = repo.Update(data);
            return res;
        }
        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
