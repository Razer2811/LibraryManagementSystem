using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class LanguageRepository
    : CommonRepository<Language>, ILanguageRepository
{
    public LanguageRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "Languages",
            "LanguageId",
            new[]
            {
                "LanguageCode",
                "LanguageName",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM Books
                    WHERE LanguageId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}
