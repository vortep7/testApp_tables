public class Document
{
    public int Id { get; set; } 
    public string Number { get; set; }
    public DateTime Date { get; set; } 
    public double TotalAmount { get; set; } 
    public string Notes { get; set; } 

    public List<Specification> Specifications { get; set; } = new List<Specification>();

    public void RecalculateTotal()
    {
        TotalAmount = Specifications.Sum(s => s.Amount); 
    }
}

