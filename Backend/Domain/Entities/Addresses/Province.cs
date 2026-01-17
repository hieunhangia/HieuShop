namespace Domain.Entities.Addresses;

public class Province
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Ward>? Wards { get; set; }
}