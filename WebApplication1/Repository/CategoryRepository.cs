﻿using WebApplication1.Data;
using WebApplication1.interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);

            return Save();
        }

        public bool DeleteCategory(Category CategoryId)
        {
            _context.Remove(CategoryId);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int CategoryId)
        {
            return _context.pokemonCategories.Where(e => e.CategoryId == CategoryId).Select(e => e.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
