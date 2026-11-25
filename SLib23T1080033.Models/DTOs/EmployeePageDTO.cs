namespace SLib23T1080033.Models.DTOs;

public class EmployeePageDTO
{
    public IReadOnlyList<EmployeeDTO> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling((double)TotalCount / PageSize);
}
