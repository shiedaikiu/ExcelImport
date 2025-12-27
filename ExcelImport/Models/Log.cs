namespace ExcelImport.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }

        public DateTime CreateDate { get; set; }

        public long DurationMs { get; set; }
    }

}
