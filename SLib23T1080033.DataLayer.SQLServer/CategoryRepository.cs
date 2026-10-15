using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class CategoryRepository
    : CommonRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "Categories",
            "CategoryId",
            new[]
            {
                "CategoryName",
                "ParentCategoryId",
                "Description",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN
                    EXISTS (
                        SELECT 1
                        FROM BookCategories
                        WHERE CategoryId = @Id
                    )
                    OR EXISTS (
                        SELECT 1
                        FROM Categories
                        WHERE ParentCategoryId = @Id
                    )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}
