using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class CategoryService
    {
        CategoryRepo repo;
        Mapper mapper;
        public CategoryService(CategoryRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }
        public List<CategoryDTO> Get() { 
            var data = repo.Get();
            var res = mapper.Map<List<CategoryDTO>>(data);
            return res;
        }
        public CategoryDTO Get(int id) {
            var data = repo.Get(id);
            var res = mapper.Map<CategoryDTO>(data);
            return res;
        }
        public bool Create(CategoryDTO c) {
            var data = mapper.Map<Category>(c);
            var res = repo.Create(data);
            return res;
        
        }
        public bool Update(CategoryDTO c) {
            var data = mapper.Map<Category>(c);
            var res = repo.Update(data);
            return res;
        }
        public bool Delete(int id) { 
            return repo.Delete(id);
        }
    }
}
