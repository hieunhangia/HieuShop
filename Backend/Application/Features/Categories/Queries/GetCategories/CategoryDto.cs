namespace Application.Features.Categories.Queries.GetCategories;

public class CategoryDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string ImageUrl { get; set; }
}