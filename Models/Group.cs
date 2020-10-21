namespace F.Economy.Models
{
    public class Group
    {
        public string GroupName { get; set; }
        public int Salary { get; set; }
        public int Tax { get; set; }

        public Group(string name, int salary, int tax)
        {
            GroupName = name;
            Salary = salary;
            Tax = tax;
        }

        public Group() { }
    }
}

