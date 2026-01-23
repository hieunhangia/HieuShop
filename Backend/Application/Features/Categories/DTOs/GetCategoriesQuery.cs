namespace Application.Features.Categories.DTOs;

public class GetCategoriesQuery
{
    public string SearchText { get; set; } = string.Empty;
    public int Top { get; set; } = 5;
}