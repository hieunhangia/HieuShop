namespace Application.Features.Brands.DTOs;

public class GetBrandsQuery
{
    public string SearchText { get; set; } = string.Empty;
    public int Top { get; set; } = 5;
}