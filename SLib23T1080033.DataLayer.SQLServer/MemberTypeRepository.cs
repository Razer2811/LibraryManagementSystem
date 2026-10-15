using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class MemberTypeRepository
    : CommonRepository<MemberType>, IMemberTypeRepository
{
    public MemberTypeRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "MemberTypes",
            "MemberTypeId",
            new[]
            {
                "TypeName",
                "MaxActiveLoans",
                "LoanPeriodDays",
                "MaxRenewals",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM Members
                    WHERE MemberTypeId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}
