using Application.Features.Brands.DTOs;
using Domain.Interfaces;
using FluentValidation;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Features.Brands;

public class BrandService(IUnitOfWork unitOfWork, IValidator<GetBrandsQuery> getBrandsQueryValidator) : IBrandService
{
    public async Task<IEnumerable<BrandResponse>> QueryBrandsAsync(GetBrandsQuery query)
    {
        var validationResult = await getBrandsQueryValidator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        return (await unitOfWork.Brands.QueryActiveBrandsReadOnlyAsync(query.Top))
            .Select(brand => new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                LogoUrl = brand.LogoUrl
            });
    }
}