namespace Domain.Entities.Addresses;

public class Ward
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ProvinceId { get; set; }

    public Province? Province { get; set; }
}