using WebApi.Model;

namespace WebApi.Service
{
    public class InMemoryProductService
    {
        private const string PROD_NOT_FOUND = "Product not found.";

        private static readonly List<Product> _products = [
            new() {
                Id = 100,
                Name = "Laptop",
                Price = 2500,
                IsActive = true,
                PostedDate = DateTime.UtcNow.AddDays(-6),
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalReason = string.Empty,
                State = ProductState.Created,
                CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
            new() {
                Id = 101,
                Name = "iPhone",
                Price = 1300,
                IsActive = true,
                PostedDate = DateTime.UtcNow.AddDays(-5),
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalReason = string.Empty,
                State = ProductState.Created,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new() {
                Id = 102,
                Name = "HP Laser Printer",
                Price = 500,
                IsActive = true,
                PostedDate = DateTime.UtcNow.AddDays(-4),
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalReason = string.Empty,
                State = ProductState.Created,
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },
        ];

        private static int _nextId = 103;

        // Get all active products
        public List<Product> GetProducts()
        {
            return _products.Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt).ToList();
            // New way
            //return [.. _products.Where(p => p.IsActive).OrderByDescending(p => p.CreatedAt)];
        }

        public List<Product> SearchProducts(string name, decimal? minPrice, decimal? maxPrice, DateTime? postedDateStart, DateTime? postedDateEnd)
        {
            var query = _products.AsQueryable();

            // Filter by product name
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by price range
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Filter by posted date range
            if (postedDateStart.HasValue)
            {
                query = query.Where(p => p.PostedDate >= postedDateStart.Value);
            }

            if (postedDateEnd.HasValue)
            {
                query = query.Where(p => p.PostedDate <= postedDateEnd.Value);
            }

            // Return the filtered list of products
            return [.. query];
        }

        // Get products in the approval queue
        public List<Product> GetProductsInApprovalQueue()
        {
            return [.. _products.Where(p => p.ApprovalStatus == ApprovalStatus.PendingApproval).OrderBy(p => p.ApprovalRequestDate)];
        }

        // Add a new product
        public bool CreateProduct(Product product)
        {
            // Business Logic 3
            // Product creation is not allowed when its price is more than 10000 dollars.
            if (product.Price > 10000)
                throw new InvalidOperationException("Product price cannot exceed $10,000.");

            // Business Logic 4:
            // Any product should be pushed to approval queue if its price is more than 5000 dollars at the
            // time creation or update
            if (product.Price > 5000)
            {
                product.ApprovalStatus = ApprovalStatus.PendingApproval;
                product.ApprovalReason = "Price exceeds $5000";
                product.ApprovalRequestDate = DateTime.UtcNow;
            }

            product.Id = _nextId++;
            product.CreatedAt = DateTime.UtcNow;
            product.IsActive = true;

            _products.Add(product);
            return true;
        }

        // Update an existing product
        public bool UpdateProduct(int id, Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new InvalidOperationException(PROD_NOT_FOUND);

            if (updatedProduct.Price > 10000)
                throw new InvalidOperationException("Product price cannot exceed $10,000.");

            // Business Logic 4:
            // Any product should be pushed to approval queue if its price is more than 5000 dollars at the
            // time creation or update

            // Business Logic 5:
            // Any product should be pushed to approval queue if its price is more than 50% of its previous price.
            if (updatedProduct.Price > 5000 || updatedProduct.Price > product.Price * 1.5m)
            {
                updatedProduct.ApprovalStatus = ApprovalStatus.PendingApproval;
                updatedProduct.ApprovalReason = "Price exceeds $5000 or 50% of previous price.";
                updatedProduct.ApprovalRequestDate = DateTime.UtcNow;
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.UpdatedAt = DateTime.UtcNow;

            return true;
        }

        // Delete a product
        public bool DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException(PROD_NOT_FOUND);

            // Business Logic 6: Product should be pushed to approval queue in case delete.  
            product.ApprovalStatus = ApprovalStatus.PendingApproval;
            product.ApprovalReason = "Delete request";
            product.ApprovalRequestDate = DateTime.UtcNow;

            return true;
        }

        // Approve a product
        public bool ApproveProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException(PROD_NOT_FOUND);

            product.ApprovalStatus = ApprovalStatus.Approved;
            product.ApprovalRequestDate = null;
            product.ApprovalReason = string.Empty;

            return true;
        }

        // Reject a product
        public bool RejectProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException(PROD_NOT_FOUND);

            product.ApprovalStatus = ApprovalStatus.Rejected;
            product.ApprovalRequestDate = null;
            product.ApprovalReason = string.Empty;

            return true;
        }
    }

}
