public class Specification
{
    public int Id { get; set; } 
    public string Name { get; set; } 
    public double Amount { get; set; } 
    public int DocumentId { get; set; } 

    public Document Document { get; set; } 
}