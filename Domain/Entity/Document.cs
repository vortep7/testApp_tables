public class Document
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public double TotalAmount { get; private set; }
    public List<Specification> Specifications { get; set; } = new();

    public void RecalculateTotal()
    {
        TotalAmount = Specifications.Sum(s => s.Amount);
    }
}