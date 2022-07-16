namespace EmailService.Models
{
    public class StatisticsAndUsers
    {
        private int countOfTriggers1;
        private int todayTriggers1;

        public List<User> Users { get; set; }
        public StatisticsAndUsers()
        {
            triggerDates = Database.outputDates();
            Users = new List<User>();
            Users = Repository.Responses.ToList();
            countOfTriggers = triggerDates.Count;
            todayTriggers = calculateTriggers(triggerDates);
        }

        public IEnumerator<User> GetEnumerator() => Users.GetEnumerator();
        public List<DateTime> triggerDates { get; set; }
        public int countOfTriggers { get => countOfTriggers1; set => countOfTriggers1 = value; }
        public int todayTriggers { get => todayTriggers1; set => todayTriggers1 = value; }

        private int calculateTriggers(List<DateTime> times)
        {
            int count = 0;
            foreach (DateTime time in times)
            {
                if(time.ToString()[..10] == DateTime.Now.ToString()[..10])
                {
                    count++;
                }
            }
            return count;
        }
    }
}
