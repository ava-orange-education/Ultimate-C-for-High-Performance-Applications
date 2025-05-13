namespace DataTransformation
{
    class StandardData
    {
        public StandardData(string data)
        {
            ParseData(data);
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        private void ParseData(string data)
        {
            var names = data.Split(" ");

            FirstName = names[0];
            if (names.Length == 3)
            {
                MiddleName = names[1];
                LastName = names[2];
            }
            else
                LastName = names[1];
        }
    }
}
