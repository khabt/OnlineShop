using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Get list product by category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Product> ListByCategoryId(long categoryId)
        {
            return db.Products.Where(x => x.CategoryID == categoryId).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListRelatedProducts(long id)
        {
            var product = db.Products.Find(id);
            return db.Products.Where(x => x.ID != id && x.CategoryID == product.CategoryID).ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

    }
}
