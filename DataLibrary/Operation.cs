namespace DataLibrary
{
    public class Operation
    {
        public Operation(int account, decimal credit, decimal debit)
        {
            AccountNumber = account;
            Credit = credit;
            Debit = debit;
        }

        public int AccountNumber { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}