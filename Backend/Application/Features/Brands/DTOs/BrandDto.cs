namespace Application.Features.Brands.DTOs;

public class BrandDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public required string LogoUrl { get; set; }
}