using SLib23T1080033.DataLayer.Interfaces;
using SLib23T1080033.Models.Entities;

namespace SLib23T1080033.DataLayer.SQLServer;

public class LocationRepository
    : CommonRepository<Location>, ILocationRepository
{
    public LocationRepository(DbHelper dbHelper)
        : base(
            dbHelper,
            "Locations",
            "LocationId",
            new[]
            {
                "LocationCode",
                "LocationName",
                "Floor",
                "Room",
                "Shelf",
                "Description",
                "IsActive"
            },
            """
            SELECT CAST(
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM BookCopies
                    WHERE LocationId = @Id
                )
                THEN 1 ELSE 0 END
            AS BIT)
            """)
    {
    }
}
