using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductswithBrandsAndTypes : BaseSpecifications<Product>
    {
        public ProductswithBrandsAndTypes(ProductSpecParam productParams) :
         base(x=>(string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
         (!productParams.BrandId.HasValue || x.ProductBrandId==productParams.BrandId)
          && (!productParams.TypeId.HasValue || x.ProductTypeId==productParams.TypeId))
        {
            AddIncludes(x=>x.ProductType);
            AddIncludes(x=>x.ProductBrand);
            OrderByAcse(x=>x.Name);
            
            ApplyPaging(productParams.pageSize,(productParams.pageSize*(productParams.pageIndex-1)));

            if (!String.IsNullOrWhiteSpace(productParams.Sort))
            {
                 if (productParams.Sort=="priceAsc")
            {
                OrderByAcse(x=>x.Price);
            }else if(productParams.Sort=="priceDesc")
            {
                OrderByDesc(x=>x.Price);
            }
            }
        }
        public ProductswithBrandsAndTypes(int id):base(x=>x.Id==id)
        {
            AddIncludes(x=>x.ProductType);
            AddIncludes(x=>x.ProductBrand);
        }
    }
}