using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class AuthorRepository : CommonRepository<Author>, IAuthorRepository
{
    public AuthorRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "Authors",
            "AuthorId",
            new[]
            {
                "AuthorName",
                "Biography",
                "BirthDate",
                "DeathDate",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM BookAuthors
                    WHERE AuthorId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}