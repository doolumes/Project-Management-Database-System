namespace Group6Application.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Cost { get; set; }
        public int ProjectID { get; set; }
    }
    public class ExpenseView
    {
        public List<Expense> Expenses = new List<Expense>();
        public string? ProjectID { get; set; } // same for all expenses in list
    }
}
