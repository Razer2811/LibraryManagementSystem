using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class PublisherRepository
    : CommonRepository<Publisher>, IPublisherRepository
{
    public PublisherRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "Publishers",
            "PublisherId",
            new[]
            {
                "PublisherName",
                "Address",
                "Phone",
                "Email",
                "Website",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM Books
                    WHERE PublisherId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}
